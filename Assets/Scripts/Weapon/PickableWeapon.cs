using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;

    private Rigidbody2D rb;
    
    private int bulletAmount;
    private int bulletAmountMax;

    private Vector2 GetVelocity()
    {
        return rb.velocity;
    }
    private float GetSummaryVelocity()
    {
        return Mathf.Abs(GetVelocity().x + GetVelocity().y);
    }
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        bulletAmount = weaponSO.weaponStats.capacity;
        bulletAmountMax = weaponSO.weaponStats.maxBullets;
    }

    public void SetupBulletAmount(int bulletAmount, int bulletAmountMax)
    {
        this.bulletAmount = bulletAmount;
        this.bulletAmountMax = bulletAmountMax;

    }

    private void Update()
    {
        Vector2 velocity = GetVelocity();
        transform.Rotate(Vector3.forward * (Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y)), Space.World);
    }

    public void Interact(PlayerWeaponHandler player)
    {
        player.PickupWeapon(weaponSO, bulletAmount, bulletAmountMax);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyHealthHandler enemy))
        {
            float damageableVelocity = 1.5f;
            if (GetSummaryVelocity() > damageableVelocity)
            {
                enemy.Hit(GameInput.Instance.GetAimDirectionVector());
            }
        }

        if (!other.TryGetComponent(out PlayerHealth player) && !other.isTrigger)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
