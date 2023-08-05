using System;
using UnityEngine;

public class CarrotBehaviour : EnemyBehaviourHandler
{
    [SerializeField] private AudioSource carrotAudioSource;
    
    private readonly float detonateRadius = 1.5f;
    private bool onetime;
    
    protected override void Start()
    {
        base.Start();
        EnemyHealth.OnDieEvent += EnemyHealthOnOnDieEvent;
        OnFindPlayerAction += OnOnFindPlayerAction;
    }

    private void OnOnFindPlayerAction(object sender, EventArgs e)
    {
        carrotAudioSource.gameObject.SetActive(true);
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
                playerHealth.Hit(Vector3.zero);
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
