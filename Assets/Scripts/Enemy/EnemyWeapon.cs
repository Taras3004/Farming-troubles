using System;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public event EventHandler OnFireAction;

    [SerializeField] protected EnemyBullet enemyBullet;
    [SerializeField] protected Transform shotPosition;
    [SerializeField] private EnemyWeaponHandler weaponHandler;
    [SerializeField] private float shootRate = 0.2f;

    private float currShootRate;
    private bool canShoot = true;

    private Vector2 ShotDirection()
    {
        if (weaponHandler == null) throw new NullReferenceException();

        return weaponHandler.DirToTarget();
    }
    private void Awake()
    {
        enemyBullet.gameObject.SetActive(false);
    }
    private void Update()
    {
        HandleShooting();
    }
    private void HandleShooting()
    {
        if (canShoot)
            return;

        currShootRate += Time.deltaTime;
        if (currShootRate >= shootRate)
            canShoot = true;
    }
    public void TryShoot()
    {
        if (canShoot == false)
            return;
        ShotBehaviour();
    }
    protected virtual void ShotBehaviour()
    {

    }
    protected void Shot(int countOfBullets = 1, float weaponSpread = 0.5f)
    {
        for(int i = 0; i < countOfBullets; i++)
        {
            EnemyBullet bullet = Instantiate(enemyBullet, shotPosition.position, Quaternion.identity);
            bullet.gameObject.SetActive(true);
            Vector2 randomisedShotDirection = new Vector2
                (UnityEngine.Random.Range(ShotDirection().x - weaponSpread, ShotDirection().x + weaponSpread),
                UnityEngine.Random.Range(ShotDirection().y - weaponSpread, ShotDirection().y + weaponSpread )).normalized;
            bullet.Setup(randomisedShotDirection);
        }

        OnFireAction?.Invoke(this, EventArgs.Empty);
        canShoot = false;
        currShootRate = 0;
    }
}
