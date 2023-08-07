using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IResetable
{
    public static PlayerHealth Instance;

    public event EventHandler OnHealthDecreased;
    public event EventHandler OnHealthIncreased;
    public event EventHandler OnDie;

    private Rigidbody2D rb;

    [SerializeField] private SpriteRenderer render;
    private readonly int maxHealth = 5;

    private Vector3 startPosition;
    private int currHealth;

    public int CurrentHealth()
    {
        return currHealth;
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        startPosition = transform.position;
    }
    
    public void Hit(Vector3 shootDirection)
    {
        if (currHealth <= 0)
            return;

        currHealth--;
        OnHealthDecreased?.Invoke(this, EventArgs.Empty);

        if (currHealth <= 0)
        {
            Die(shootDirection);
        }
    }

    public void Heal()
    {
        currHealth++;
        OnHealthIncreased?.Invoke(this, EventArgs.Empty);
    }

    private void Die(Vector3 shootDirection)
    {
        float force = 2.3004f;
        render.transform.up = shootDirection;
        rb.AddForce(shootDirection * force, ForceMode2D.Impulse);
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    public void Reset()
    {
        currHealth = maxHealth;
        transform.position = startPosition;
        render.transform.up = new Vector3(0, 0, 0);
    }
}