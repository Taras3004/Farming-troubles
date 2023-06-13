using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullpupWeapon : EnemyWeapon
{
    protected override void ShotBehaviour()
    {
        StartCoroutine(FireBurstOfShots());
    }
    private IEnumerator FireBurstOfShots()
    {
        int countOfShots = 3;
        float shootRate = 0.2f;
        for(int i = 0; i < countOfShots; i++)
        {
            Shot();
            yield return new WaitForSeconds(shootRate);
        }
    }
}
