using System;
using UnityEngine;

public class OnionBehaviour : EnemyBehaviourHandler
{
    public event EventHandler OnDashAction;

    private Collider2D coll;

    private float dashTimer;
    private readonly float dashTimerMax = 5f;

    private readonly float dashDurationMax = 0.6f;
    private float currDashDuration;

    private OnionState onionState = OnionState.Idle;
    private Vector2 dashDirection;


    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
    }

    private void HandleDashCooldown()
    {
        if (onionState == OnionState.OnCooldown)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashTimerMax)
            {
                onionState = OnionState.Idle;
                dashTimer = 0;
            }
        }
    }

    private void HandleDashDuration()
    {
        if (onionState == OnionState.Dashing)
        {
            float dashSpeed = 6f;
            CheckCanMove(dashDirection, dashSpeed, out bool canMove);
            if (canMove)
            {
                transform.position = Vector2.MoveTowards(Tr.position,
                    dashDirection, Time.deltaTime * dashSpeed);
            }

            currDashDuration += Time.deltaTime;
            if (currDashDuration >= dashDurationMax)
            {
                onionState = OnionState.OnCooldown;
                currDashDuration = 0;
            }
        }
    }

    private void CheckCanMove(Vector3 direction, float speed, out bool canMove)
    {
        float moveDistance = speed * Time.deltaTime;
        canMove = true;
    
        Bounds bounds = coll.bounds;
        RaycastHit2D[] raycastHits = Physics2D.CapsuleCastAll(bounds.center, bounds.size,
            CapsuleDirection2D.Vertical, 0, direction, moveDistance, ObstaclesLayer());
        foreach (RaycastHit2D hit in raycastHits)
        {
            if (hit.collider.TryGetComponent(out PickableWeapon weapon))
                continue;
    
            if (hit.collider.TryGetComponent(out PlayerMovement player) == false)
            {
                canMove = false;
                break;
            }
    
            canMove = true;
        }
    }

    protected override void HandleBehaviour()
    {
        HandleDashCooldown();
        HandleDashDuration();
        dashDirection = transform.position - Target().position;
        if (onionState != OnionState.Idle)
            return;

        if (IsReachedEndDistance())
        {
            Dash();
        }
    }

    private void Dash()
    {
        OnDashAction?.Invoke(this, EventArgs.Empty);
        onionState = OnionState.Dashing;
    }

    private enum OnionState
    {
        Idle,
        Dashing,
        OnCooldown
    }
}