using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public event EventHandler OnBulletHitted;
    private Rigidbody2D rb;
    private float bulletLifetime;
    private float bulletLifetimeMax = 3;

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
        if(bulletLifetime >= bulletLifetimeMax)
        {
            Destroy(gameObject);
        }
    }
    public void Setup(Vector3 shootDirection)
    {
        float bulletSpeed = 10f;
        transform.right = shootDirection;
        rb.velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * bulletSpeed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth player))
        {
            OnBulletHitted?.Invoke(this, EventArgs.Empty);
            rb.velocity = Vector2.zero;
            //player.Hit();
        }
    }
}