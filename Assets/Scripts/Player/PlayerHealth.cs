using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public event EventHandler OnHealthDecreased;
    public event EventHandler OnHealthIncreased;
    public event EventHandler OnDie;

    private Rigidbody2D rb;

    [SerializeField] private SpriteRenderer render;
    [SerializeField] private int maxHealth;
    
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
    }

    private void Start()
    {
        OnDie += PlayerHealth_OnDie;
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        GameInput.Instance.OnRespawnAction += InstanceOnOnRespawnAction;
    }

    private void InstanceOnOnRespawnAction(object sender, EventArgs e)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
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
}