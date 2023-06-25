using System;
using System.Collections;
using UnityEngine;

public class OnionBehaviour : EnemyBehaviourHandler
{
    
    public event EventHandler OnJerkAction;

    private float jerkTimer;
    private float jerkTimerMax = 5f;

    private float jerkDurationMax = 0.4f;
    private float currJerkDuration;

    private OnionState onionState = OnionState.Idle;
    private Vector2 jerkDirection;
    
    
    private void HandleJerkCooldown()
    {
        if (onionState == OnionState.OnCooldown)
        {
            jerkTimer += Time.deltaTime;
            if (jerkTimer >= jerkTimerMax)
            {
                onionState = OnionState.Idle;
                jerkTimer = 0;
            }
        }
    }

    private void HandleJerkDuration()
    {
        if (onionState == OnionState.Jerking)
        {
            float jerkSpeed = 15f;

            transform.position = Vector2.MoveTowards(Tr.position,
                jerkDirection, Time.deltaTime * jerkSpeed);
            currJerkDuration += Time.deltaTime;
            if (currJerkDuration >= jerkDurationMax)
            {
                onionState = OnionState.OnCooldown;
                currJerkDuration = 0;
            }
        }
    }
    protected override void HandleBehaviour()
    {
        HandleJerkCooldown();
        HandleJerkDuration();
        jerkDirection = Target().position - transform.position;
        if(onionState != OnionState.Idle)
            return;
        
        if (IsReachedEndDistance())
        {
            Jerk();
        }
    }

    private void Jerk()
    {
        OnJerkAction?.Invoke(this, EventArgs.Empty);
        onionState = OnionState.Jerking;
    }

    private enum OnionState
    {
        Idle,
        Jerking,
        OnCooldown
    }
}
