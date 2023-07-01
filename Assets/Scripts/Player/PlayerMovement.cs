using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private Collider2D renderCollider;
    public event EventHandler OnJerkAction;

    private Rigidbody2D rb;

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

    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
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
        if (!canMove)
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

        RaycastHit2D[] raycastHit = Physics2D.CapsuleCastAll(renderCollider.bounds.center, renderCollider.bounds.size,
            CapsuleDirection2D.Vertical, 0, direction, moveDistance);
        for (int i = 0; i < raycastHit.Length; i++)
        {
            if (raycastHit[i].collider.TryGetComponent(out PickableWeapon weapon))
                continue;

            if (raycastHit[i].collider.TryGetComponent(out PlayerMovement player) == false)
            {
                canMove = false;
                break;
            }

            canMove = true;
        }
    }

    private void Jerk()
    {
        if (isCanJerk == false)
            return;

        isCanJerk = false;
        float force = 12.3004f;

        Vector2 jerkDirection =
            GameInput.Instance.GetPlayerMovementVectorNormalized() != Vector2.zero
                ? GameInput.Instance.GetPlayerMovementVectorNormalized()
                : GameInput.Instance.GetAimDirectionVector();

        OnJerkAction?.Invoke(this, EventArgs.Empty);
        rb.AddForce(jerkDirection * force, ForceMode2D.Impulse);
    }
}