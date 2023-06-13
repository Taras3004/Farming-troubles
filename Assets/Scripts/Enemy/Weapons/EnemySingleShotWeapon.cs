using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySingleShotWeapon : EnemyWeapon
{
    [SerializeField] private float spread = 1.5f;
    protected override void ShotBehaviour()
    {
        int countOfBullets = 1;
        Shot(countOfBullets, spread);
    }
}
