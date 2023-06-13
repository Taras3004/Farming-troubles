using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnJerkAction;

    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    
    private float moveSpeed = 5f;
    private bool isWalking;
    private Vector3 moveDir;

    private float jerkTimer;
    private float jerkTimerMax = 1f;
    private bool isCanJerk;

    public bool IsWalking()
    {
        return isWalking;
    }
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        rb.freezeRotation = true;
        GameInput.Instance.OnJerkAction += Instance_OnJerkAction;
    }

    private void Instance_OnJerkAction(object sender, System.EventArgs e)
    {
        Jerk();
    }

    private void Update()
    {
        HandleMovement();
        HandleJerkCooldown();
    }
    private void HandleJerkCooldown()
    {
        if (isCanJerk == false)
        {
            jerkTimer -= Time.deltaTime;
            if (jerkTimer <= 0)
            {
                isCanJerk = true;
            }
        }
        else
        {
            jerkTimer = jerkTimerMax;
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetPlayerMovementVectorNormalized();

        moveDir = new Vector3(inputVector.x, inputVector.y, 0);

        float moveDistance = moveSpeed * Time.deltaTime;

        CheckCanMove(moveDir, out bool canMove);
        if(!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            CheckCanMove(moveDirX, out canMove);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirY = new Vector3(0, moveDir.y, 0).normalized;
                CheckCanMove(moveDirY, out canMove);
                if (canMove)
                {
                    moveDir = moveDirY;
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
    }
    private void CheckCanMove(Vector3 direction, out bool canMove)
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        canMove = true;

        RaycastHit2D[] raycastHit = Physics2D.CapsuleCastAll(coll.bounds.center, coll.bounds.size, CapsuleDirection2D.Vertical, 0, direction, moveDistance);
        for (int i = 0; i < raycastHit.Length; i++)
        {
            if (raycastHit[i].collider.TryGetComponent(out PickableWeapon weapon)) 
                continue;

            if (raycastHit[i].collider.TryGetComponent(out Player player) == false)
            {
                canMove = false;
                break;
            }
            else
            {
                canMove = true;
            }
        }
    }
    private void Jerk()
    {
        if (isCanJerk == false)
            return;

        isCanJerk = false;
        
        Vector2 jerkDirection = 
            GameInput.Instance.GetPlayerMovementVectorNormalized() != Vector2.zero ?
            GameInput.Instance.GetPlayerMovementVectorNormalized() : 
            GameInput.Instance.GetAimDirectionVector();
        
        float force = 12.3004f;

        OnJerkAction?.Invoke(this, EventArgs.Empty);
        rb.AddForce(jerkDirection * force, ForceMode2D.Impulse);
    }
}
