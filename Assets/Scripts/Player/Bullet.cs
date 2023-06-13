using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public event EventHandler OnObstacleHit;
    public event EventHandler OnEnemyHit;
    private Rigidbody2D rb;
    private Vector3 shootDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb.gravityScale = 0f;
    }
    public void Setup(Vector3 shootDirection)
    {
        float bulletSpeed = 30f;
        this.shootDirection = shootDirection;
        transform.right = shootDirection;
        rb.velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * bulletSpeed;

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Obstacle obstacle))
        {
            OnObstacleHit?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
        else if (collision.TryGetComponent(out EnemyHealthHandler enemy))
        {
            OnEnemyHit?.Invoke(this, EventArgs.Empty);
            enemy.Hit(shootDirection);
            Destroy(gameObject);
        }
    }
}
