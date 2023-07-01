using System;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;

    private Rigidbody2D rb;
    
    private int bulletAmount;
    private int bulletAmountMax;

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
    public void Interact(PlayerWeaponHandler player)
    {
        player.PickupWeapon(weaponSO, bulletAmount, bulletAmountMax);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            rb.velocity = Vector2.zero;
        }
    }
}
