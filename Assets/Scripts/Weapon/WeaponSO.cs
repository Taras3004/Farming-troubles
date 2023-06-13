using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon", fileName = "New weapon")]
public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public GameObject pickableWeaponPrefab;
    public WeaponStats weaponStats;

}
[Serializable]
public struct WeaponStats
{
    public int damage;
    public int capacity;
    public int maxBullets;
    public float shotRate;
    public float reloadTime;
    public float playerSpeed;
}