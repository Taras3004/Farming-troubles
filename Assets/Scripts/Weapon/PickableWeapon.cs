using System;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponSO;

    private int bulletAmount;
    private int bulletAmountMax;

    private void Awake()
    {
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
}
