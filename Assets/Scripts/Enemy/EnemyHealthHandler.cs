using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealthHandler : MonoBehaviour
{
    public event EventHandler OnHitEvent;
    public event EventHandler OnDieEvent;

    [SerializeField] private Color bloodColor;
    [SerializeField] private int maxHealth;
    [SerializeField] private EnemyLoot loot;
    private int currHealth;
    private Rigidbody2D rb;
    private Collider2D coll;

    public Color BloodColor()
    {
        return bloodColor;
    }
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
        GameObserver.Instance.AddDeadEnemy(this);
    }

    public void Hit(Vector3 shootDirection)
    {
        currHealth--;
        OnHitEvent?.Invoke(this, EventArgs.Empty);

        BloodParticleSystemHandler.Instance.SpawnBlood(transform.position, shootDirection, bloodColor);
        
        if (currHealth <= 0)
        {
            Die(shootDirection);
        }
    }
    private void Die(Vector3 fallDirection)
    {
        float force = UnityEngine.Random.Range(200, 400);
        transform.right = fallDirection;
        transform.Rotate(new Vector3(transform.position.x, transform.position.y, transform.position.z - 90));
        
        rb.AddForce(fallDirection * force);
        OnDieEvent?.Invoke(this, EventArgs.Empty);
        
        if(loot == null)
            return;
        if(Random.value < 0.33f)
            Instantiate(loot, transform.position, Quaternion.identity);
    }
}
