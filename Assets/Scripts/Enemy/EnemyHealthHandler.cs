using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealthHandler : MonoBehaviour
{
    public event EventHandler OnHitEvent;
    public event EventHandler OnDieEvent;

    [SerializeField] private int maxHealth;
    private int currHealth;
    private Rigidbody2D rb;
    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
    }
    private void Start()
    {
        OnDieEvent += EnemyHealthHandler_OnDieEvent;
    }

    private void EnemyHealthHandler_OnDieEvent(object sender, EventArgs e)
    {
        coll.enabled = false;
    }

    public void Hit(Vector3 shootDirection)
    {
        currHealth--;
        OnHitEvent?.Invoke(this, EventArgs.Empty);

        if (currHealth <= 0)
        {
            Die(shootDirection);
        }
    }
    private void Die(Vector3 fallDirection)
    {
        float force = UnityEngine.Random.Range(500, 800);
        transform.right = fallDirection;
        transform.Rotate(new Vector3(transform.position.x, transform.position.y, transform.position.z - 90));

        rb.AddForce(fallDirection * force);
        OnDieEvent?.Invoke(this, EventArgs.Empty);
    }
}
