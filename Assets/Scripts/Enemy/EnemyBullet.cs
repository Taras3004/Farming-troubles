using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    public event EventHandler OnBulletHit;
    private Rigidbody2D rb;
    private float bulletLifetime;
    private readonly float bulletLifetimeMax = 3;
    private Vector3 shootDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        bulletLifetime += Time.deltaTime;
        if (bulletLifetime >= bulletLifetimeMax)
        {
            Destroy(gameObject);
        }
    }

    public void Setup(Vector3 shootDirection)
    {
        this.shootDirection = shootDirection;
        transform.right = shootDirection;
        rb.velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * speed;
    }

    private void DestroyBullet()
    {
        OnBulletHit?.Invoke(this, EventArgs.Empty);
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth player))
        {
            DestroyBullet();
            player.Hit(shootDirection);
        }
        else if (!collision.isTrigger && !collision.TryGetComponent(out EnemyHealthHandler enemy))
        {
            DestroyBullet();
        }
    }
}