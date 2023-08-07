using System;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyHealthHandler : MonoBehaviour, IResetable
{
    public event EventHandler OnHitEvent;
    public event EventHandler OnDieEvent;

    [SerializeField] private Color bloodColor;
    [SerializeField] private int maxHealth;
    [SerializeField] private EnemyLoot loot;
    [SerializeField] private SpriteRenderer render;
    private int currHealth;
    private Rigidbody2D rb;
    private AIDestinationSetter aiDestinationSetter;
    private Vector3 startPosition;

    public Color BloodColor()
    {
        return bloodColor;
    }
    private void Awake()
    {
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        currHealth = maxHealth;
        startPosition = transform.position;
    }
    private void Start()
    {
        OnDieEvent += EnemyHealthHandler_OnDieEvent;
    }

    private void EnemyHealthHandler_OnDieEvent(object sender, EventArgs e)
    {
        LevelPassChecker.Instance.AddDeadEnemy(this);
    }

    public void Hit(Vector3 shootDirection)
    {
        currHealth--;
        OnHitEvent?.Invoke(this, EventArgs.Empty);

        BloodParticleSystemHandler.Instance.SpawnBlood(transform.position, shootDirection, bloodColor);
        
        if (currHealth == 0)
        {
            Die(shootDirection);
        }
    }
    private void Die(Vector3 fallDirection)
    {
        aiDestinationSetter.target = null;
        
        float force = Random.Range(200, 400);
        transform.right = fallDirection;
        transform.Rotate(new Vector3(0, 0, transform.rotation.z - 90));
        
        rb.AddForce(fallDirection * force);
        OnDieEvent?.Invoke(this, EventArgs.Empty);
        
        if(loot == null)
            return;
        if(Random.value < 0.33f)
            Instantiate(loot, transform.position, Quaternion.identity);
    }

    public void Reset()
    {
        transform.position = startPosition;
        render.transform.up = new Vector3(0, 0, 0);
        currHealth = maxHealth;
    }
}
