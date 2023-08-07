using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    [SerializeField] private Collider2D renderCollider;
    public event EventHandler OnDashAction;

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;

    private readonly float moveSpeed = 5f;
    private bool isCanWalk = true;
    private Vector3 moveDir;

    private readonly float dashTimerMax = 1f;
    private float dashTimer;
    private bool isCanDash;

    public bool IsWalking()
    {
        return moveDir != Vector3.zero;
    }

    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        rb.freezeRotation = true;
        GameInput.Instance.OnDashAction += InstanceOnDashAction;
        playerHealth.OnDie += PlayerHealth_OnDie;
        LevelReloader.Instance.OnLevelReloaded += LevelReloader_OnLevelReloaded;
    }

    private void LevelReloader_OnLevelReloaded(object sender, EventArgs e)
    {
        EnableMovement();
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        DisableMovement();
    }

    private void InstanceOnDashAction(object sender, EventArgs e)
    {
        Dash();
    }

    private void Update()
    {
        HandleMovement();
        HandleDashCooldown();
    }

    public void DisableMovement()
    {
        isCanWalk = false;
    }

    public void EnableMovement()
    {
        isCanWalk = true;
    }
    
    private void HandleDashCooldown()
    {
        if (isCanDash == false)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isCanDash = true;
            }
        }
        else
        {
            dashTimer = dashTimerMax;
        }
    }

    private void HandleMovement()
    {
        if (!isCanWalk)
        {
            moveDir = Vector3.zero;
            return;
        }
        
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
    }

    private void CheckCanMove(Vector3 direction, out bool canMove)
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        canMove = true;

        RaycastHit2D[] raycastHits = Physics2D.CapsuleCastAll(renderCollider.bounds.center, renderCollider.bounds.size,
            CapsuleDirection2D.Vertical, 0, direction, moveDistance);
        foreach (RaycastHit2D hit in raycastHits)
        {
            Collider2D hitCollider = hit.collider;
            
            if(hitCollider.isTrigger)
                continue;

            if (hitCollider != null)
            {
                canMove = false;
                break;
            }
            canMove = true;
        }
    }

    private void Dash()
    {
        if (isCanDash == false)
            return;

        isCanDash = false;
        float force = 12.3004f;

        Vector2 dashDirection =
            GameInput.Instance.GetPlayerMovementVectorNormalized() != Vector2.zero
                ? GameInput.Instance.GetPlayerMovementVectorNormalized()
                : GameInput.Instance.GetAimDirectionVector();

        OnDashAction?.Invoke(this, EventArgs.Empty);
        rb.AddForce(dashDirection * force, ForceMode2D.Impulse);
    }
}