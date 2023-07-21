using System;
using UnityEngine;

[RequireComponent(typeof(EnemyWholeMagazineWeaponVisual))]
public class EnemyWholeMagazineWeapon : EnemyWeapon, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnWeaponReloadProgress;
    public event EventHandler OnReloadAction;

    [SerializeField] private EnemyHealthHandler enemyHealth;
    [SerializeField] private int countOfBullets;

    private int currCountOfBullets;

    private bool isReloading;
    private float reloadTimer;
    private float reloadTimerMax = 3.5f;

    public EnemyHealthHandler EnemyHealth()
    {
        return enemyHealth;
    }
    
    private void Start()
    {
        currCountOfBullets = countOfBullets;
    }

    private void LateUpdate()
    {
        HandleReload();
    }

    private void HandleReload()
    {
        if (isReloading)
        {
            OnWeaponReloadProgress?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = reloadTimer / reloadTimerMax
            });
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTimerMax)
            {
                isReloading = false;
                reloadTimer = 0;
            }
        }
    }

    protected override void ShotBehaviour()
    {
        if(isReloading)
            return;
        Shot();
        currCountOfBullets--;
        if (currCountOfBullets <= 0)
        {
            Reload();
        }
    }

    private void Reload()
    {
        OnReloadAction?.Invoke(this, EventArgs.Empty);
        isReloading = true;
        currCountOfBullets = countOfBullets;
        
    }
}
