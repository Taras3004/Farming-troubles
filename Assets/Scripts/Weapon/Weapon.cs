using System;
using UnityEngine;

public class Weapon : MonoBehaviour, IHasProgress
{
    public event EventHandler<OnWeaponShootEventArgs> OnFireAction;
    public event EventHandler OnReloadStartedAction;
    public event EventHandler OnReloadFinishedAction;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnReloadProgressAction;

    public event EventHandler<OnBulletAmountChangedEventArgs> OnBulletAmountChanged;

    public class OnWeaponShootEventArgs : EventArgs
    {
        public Vector3 GunEndPointPosition;
        public Vector3 ShootDirection;
    }

    public class OnBulletAmountChangedEventArgs : EventArgs
    {
        public float Amount;
        public float BulletBalance;
    }

    [SerializeField] private WeaponSO weaponSO;

    [SerializeField] protected Transform shotPosition;

    [SerializeField] protected Bullet bulletPrefab;

    protected WeaponStats WeaponStats;
    private int bulletAmountBalance;
    private int bulletAmount;

    private float reloadTimerMax;
    private float reloadTimer;

    private float currShotRateTime;
    private WeaponState weaponState;

    public WeaponSO GetWeaponSO()
    {
        return weaponSO;
    }

    public int GetBulletAmountBalance()
    {
        return bulletAmountBalance;
    }

    public int GetBulletAmount()
    {
        return bulletAmount;
    }

    private void Awake()
    {
        bulletPrefab.gameObject.SetActive(false);
        WeaponStats = weaponSO.weaponStats;
        reloadTimerMax = WeaponStats.reloadTime;
        bulletAmountBalance = WeaponStats.maxBullets;
        bulletAmount = WeaponStats.capacity;
        currShotRateTime = WeaponStats.shotRate;
    }

    private void Start()
    {
        GameInput.Instance.OnFireStartAction += GameInput_OnFireAction;
        GameInput.Instance.OnFireFinishAction += GameInput_OnFireFinishAction;
        PlayerHealth.Instance.OnDie += PlayerHealth_OnDie;
        weaponState = WeaponState.Idle;
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        GameInput.Instance.OnFireStartAction -= GameInput_OnFireAction;
        GameInput.Instance.OnFireFinishAction -= GameInput_OnFireFinishAction;
    }

    private void Update()
    {
        HandleReloading();
        HandleShooting();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnFireStartAction -= GameInput_OnFireAction;
        GameInput.Instance.OnFireFinishAction -= GameInput_OnFireFinishAction;
    }

    public void Setup(int bulletAmount, int bulletAmountBalance)
    {
        this.bulletAmount = bulletAmount;
        this.bulletAmountBalance = bulletAmountBalance;
    }

    private void HandleShooting()
    {
        //bool isPlayerShooting = weaponState == WeaponState.Fire;
        //if (isPlayerShooting == false)
        //    return;

        currShotRateTime += Time.deltaTime;
        if (currShotRateTime >= WeaponStats.shotRate)
        {
            if (weaponState == WeaponState.Fire)
                Shot();
        }
    }

    private void HandleReloading()
    {
        bool isPlayerReloading = weaponState == WeaponState.Reloading;
        if (isPlayerReloading == false)
            return;

        reloadTimer += Time.deltaTime;
        OnReloadProgressAction?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            { progressNormalized = reloadTimer / reloadTimerMax });
        if (reloadTimer >= reloadTimerMax)
        {
            FinishReloading();
        }
    }

    private void GameInput_OnFireAction(object sender, EventArgs e)
    {
        if (currShotRateTime >= WeaponStats.shotRate && weaponState == WeaponState.Idle)
        {
            weaponState = WeaponState.Fire;
            Shot();
        }
    }

    private void GameInput_OnFireFinishAction(object sender, EventArgs e)
    {
        if (weaponState == WeaponState.Fire)
        {
            weaponState = WeaponState.Idle;
        }
    }

    private void Shot()
    {
        bool canShoot = weaponState != WeaponState.Reloading && bulletAmount > 0;
        if (canShoot == false)
            return;
        OnFireAction?.Invoke(this, new OnWeaponShootEventArgs()
        {
            GunEndPointPosition = shotPosition.position,
            ShootDirection = GameInput.Instance.GetAimDirectionVector()
        });

        ShotBehaviour();
        currShotRateTime = 0f;

        //weaponState = WeaponState.Fire;
        bulletAmount--;

        OnBulletAmountChanged?.Invoke(this, new OnBulletAmountChangedEventArgs
        {
            Amount = bulletAmount, BulletBalance = bulletAmountBalance
        });
        if (bulletAmount <= 0)
        {
            StartReloading();
        }
    }

    protected virtual void ShotBehaviour()
    {
        GameObject tempBullet = Instantiate(bulletPrefab.gameObject, shotPosition.position, Quaternion.identity);
        tempBullet.SetActive(true);
        float directionAngle = GameInput.Instance.GetAimDirectionAngle();
        float spread = WeaponStats.spread;
        float randomisedDirectionAngle = UnityEngine.Random.Range(directionAngle - spread, directionAngle + spread);

        Vector2 shootDirection = new Vector2
        (Mathf.Sin((randomisedDirectionAngle + 90) * Mathf.Deg2Rad),
            -Mathf.Cos((randomisedDirectionAngle + 90) * Mathf.Deg2Rad));

        tempBullet.GetComponent<Bullet>().Setup(shootDirection);
    }

    public void StartReloading()
    {
        if (bulletAmountBalance <= 0 || weaponState == WeaponState.Reloading)
            return;
        weaponState = WeaponState.Reloading;
        reloadTimer = 0f;
        OnReloadStartedAction?.Invoke(this, EventArgs.Empty);
    }

    private void FinishReloading()
    {
        weaponState = WeaponState.Idle;
        if (bulletAmountBalance > WeaponStats.capacity)
        {
            bulletAmountBalance = bulletAmountBalance + bulletAmount - WeaponStats.capacity;
            bulletAmount = WeaponStats.capacity;
        }
        else
        {
            if (bulletAmount + bulletAmountBalance > WeaponStats.capacity)
            {
                while (true)
                {
                    bulletAmount++;
                    bulletAmountBalance--;
                    if (bulletAmount == WeaponStats.capacity)
                        break;
                }
            }
            else
            {
                bulletAmount += bulletAmountBalance;
                bulletAmountBalance = 0;
            }
        }

        OnReloadFinishedAction?.Invoke(this, EventArgs.Empty);
        OnBulletAmountChanged?.Invoke(this, new OnBulletAmountChangedEventArgs
        {
            Amount = bulletAmount,
            BulletBalance = bulletAmountBalance
        });
    }

    private enum WeaponState
    {
        Idle,
        Fire,
        Reloading
    }
}