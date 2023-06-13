using System;
using MoreMountains.Tools;
using UnityEngine;

public class CarrotBehaviour : EnemyPathfinder
{
    private readonly float detonateRadius = 1.5f;
    private bool onetime;
    
    protected override void Start()
    {
        base.Start();
        EnemyHealth.OnDieEvent += EnemyHealthOnOnDieEvent;
    }

    private void EnemyHealthOnOnDieEvent(object sender, EventArgs e)
    {
        Detonate();
    }

    private void Detonate()
    {
        transform.localRotation = new Quaternion(0, 0, 0, 0);
        Collider2D[] colliders = Physics2D.OverlapCircleAll
            (transform.position, detonateRadius);
        foreach (Collider2D i in colliders)
        {
            if (i.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.Hit();
            }
        }
    }
    protected override void HandleBehaviour()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll
            (transform.position, detonateRadius);
        foreach (Collider2D i in colliders)
        {
            if (i.TryGetComponent(out PlayerHealth playerHealth) && onetime == false)
            {
                onetime = true;
                EnemyHealth.Hit(Vector3.zero);
                break;
            }
        }
    }
}
