using UnityEngine;

public class Shotgun : Weapon
{
    protected override void ShotBehaviour()
    {
        int countOfBullets = 5;
        
        for (int i = 0; i < countOfBullets; i++)
        {
            GameObject tempBullet = Instantiate(bulletPrefab.gameObject, shotPosition.position, Quaternion.identity);
            tempBullet.SetActive(true);
            float directionAngle = GameInput.Instance.GetAimDirectionAngle();
            float spread = WeaponStats.spread;
            float randomisedDirectionAngle = UnityEngine.Random.Range(directionAngle - spread, directionAngle + spread);

            Vector2 shootDirection =  new Vector2
                (Mathf.Sin((randomisedDirectionAngle + 90) * Mathf.Deg2Rad), -Mathf.Cos((randomisedDirectionAngle + 90) * Mathf.Deg2Rad));

            tempBullet.GetComponent<Bullet>().Setup(shootDirection);
        }
    }
}
