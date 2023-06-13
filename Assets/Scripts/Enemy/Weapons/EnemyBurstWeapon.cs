using UnityEngine;

public class EnemyBurstWeapon : EnemyWeapon
{
    protected override void ShotBehaviour()
    {
        int countOfBullets = 5;
        float spread = 2f;
        Shot(countOfBullets, spread);
    }
}
