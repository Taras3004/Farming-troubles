using System;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour, IHasProgress
{
    public event EventHandler OnWeaponDroppedAction;
    public event EventHandler OnWeaponPickedAction;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnWeaponDropProgress;

    [SerializeField] private Transform weaponSpawnTransform;
    [SerializeField] private Transform chainsawTransform;

    private Weapon currentWeapon;

    private float weaponDropTimer;
    private readonly float weaponDropTimerMax = 0.2f;
    private bool isDropWeaponKeyHeld;

    public Transform GetWeaponSpawnTransform()
    {
        return weaponSpawnTransform;
    }
    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public bool IsFull()
    {
        return GetCurrentWeapon() != null;
    }

    private void Awake()
    {
        if (weaponSpawnTransform.GetChild(0).TryGetComponent(out Weapon weapon))
            currentWeapon = weapon;
    }

    private void Start()
    {
        GameInput.Instance.OnLookLeft += GameInput_OnLookLeft;
        GameInput.Instance.OnLookRight += GameInput_OnLookRight;
        GameInput.Instance.OnDropWeaponStartAction += GameInput_OnDropWeaponStartAction;
        GameInput.Instance.OnDropWeaponFinishAction += GameInput_OnDropWeaponFinishAction;
        GameInput.Instance.OnPickupWeaponAction += GameInput_OnPickupWeaponAction;
        GameInput.Instance.OnReloadWeaponAction += GameInput_OnReloadWeaponAction;

        OnWeaponDroppedAction += PlayerWeaponHandler_OnWeaponDroppedAction;
        OnWeaponPickedAction += PlayerWeaponHandlerOnWeaponPickedAction;

        PlayerHealth.Instance.OnDie += PlayerHealth_OnDie;

        if (IsFull())
        {
            chainsawTransform.gameObject.SetActive(false);

            int bulletAmount = currentWeapon.GetWeaponSO().weaponStats.capacity;
            int bulletAmountBalance = currentWeapon.GetWeaponSO().weaponStats.maxBullets;
            currentWeapon.Setup(bulletAmount, bulletAmountBalance);
        }
        else
        {
            chainsawTransform.gameObject.SetActive(true);
        }
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }
        weaponSpawnTransform.gameObject.SetActive(false);
    }

    private void PlayerWeaponHandlerOnWeaponPickedAction(object sender, EventArgs e)
    {
        DisableChainsaw();
    }

    private void PlayerWeaponHandler_OnWeaponDroppedAction(object sender, EventArgs e)
    {
        EnableChainsaw();
    }

    private void GameInput_OnReloadWeaponAction(object sender, EventArgs e)
    {
        if (IsFull())
        {
            if (currentWeapon.GetBulletAmountBalance() <= 0
                || currentWeapon.GetBulletAmount() == currentWeapon.GetWeaponSO().weaponStats.capacity)
                return;
            currentWeapon.StartReloading();
        }
    }

    private void GameInput_OnPickupWeaponAction(object sender, EventArgs e)
    {
        FindPickableWeapons();
    }

    private void Update()
    {
        HandleWeaponAngle();
        HandleWeaponDrop();
    }

    private void GameInput_OnDropWeaponStartAction(object sender, EventArgs e)
    {
        isDropWeaponKeyHeld = true;
        weaponDropTimer = 0;
    }

    private void GameInput_OnDropWeaponFinishAction(object sender, EventArgs e)
    {
        isDropWeaponKeyHeld = false;
    }

    private void GameInput_OnLookRight(object sender, EventArgs e)
    {
        SetRightPosition();
    }

    private void GameInput_OnLookLeft(object sender, EventArgs e)
    {
        SetLeftPosition();
    }

    private void HandleWeaponDrop()
    {
        if (isDropWeaponKeyHeld == false || currentWeapon == null)
            return;

        weaponDropTimer += Time.deltaTime;
        OnWeaponDropProgress?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            { progressNormalized = weaponDropTimer / weaponDropTimerMax });

        if (weaponDropTimer >= weaponDropTimerMax)
        {
            DropWeapon();
            weaponDropTimer = 0;
        }
    }

    private void HandleWeaponAngle()
    {
        float aimAngle = GameInput.Instance.GetAimDirectionAngle();
        weaponSpawnTransform.eulerAngles = new Vector3(0, 0, aimAngle);
    }

    private void SetLeftPosition()
    {
        Vector3 aimLeftLocalScale = Vector3.one;
        aimLeftLocalScale.y = -1f;
        weaponSpawnTransform.localScale = aimLeftLocalScale;

        float weaponPositionOffset = -0.2f;
        weaponSpawnTransform.localPosition = new Vector2(weaponPositionOffset, weaponSpawnTransform.localPosition.y);
    }

    private void SetRightPosition()
    {
        Vector3 aimRightLocalScale = Vector3.one;
        aimRightLocalScale.y = 1f;
        weaponSpawnTransform.localScale = aimRightLocalScale;

        float weaponPositionOffset = 0.2f;
        weaponSpawnTransform.localPosition = new Vector2(weaponPositionOffset, weaponSpawnTransform.localPosition.y);
    }

    private void DropWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dropDir = (mousePosition - transform.position).normalized;

        GameObject pickableWeaponObject = currentWeapon.GetWeaponSO().pickableWeaponPrefab;
        GameObject tempPickableObject = Instantiate(pickableWeaponObject, transform.position, Quaternion.identity);

        Rigidbody2D tempPickableObjectRb = tempPickableObject.GetComponent<Rigidbody2D>();
        PickableWeapon tempPickableWeapon = tempPickableObject.GetComponent<PickableWeapon>();

        tempPickableWeapon.SetupBulletAmount(currentWeapon.GetBulletAmount(), currentWeapon.GetBulletAmountBalance());

        float force = 8f;
        tempPickableObjectRb.AddForce(dropDir.normalized * force, ForceMode2D.Impulse);
        
        OnWeaponDroppedAction?.Invoke(this, EventArgs.Empty);

        Destroy(currentWeapon.gameObject);
        currentWeapon = null;
    }

    private void FindPickableWeapons()
    {
        if (IsFull())
            return;

        float interactRange = 1f;
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);

        foreach (Collider2D tempCollider in colliderArray)
        {
            if (tempCollider.TryGetComponent(out PickableWeapon weapon))
            {
                weapon.Interact(this);
                break;
            }
        }
    }

    public void PickupWeapon(WeaponSO weaponSO, int bulletAmount, int bulletAmountMax)
    {
        GameObject weaponPrefab = weaponSO.weaponPrefab;
        GameObject tempWeapon = Instantiate(weaponPrefab, weaponSpawnTransform.position, new Quaternion(0, 0, 0, 0),
            weaponSpawnTransform);
        currentWeapon = tempWeapon.GetComponent<Weapon>();
        currentWeapon.Setup(bulletAmount, bulletAmountMax);
        OnWeaponPickedAction?.Invoke(this, EventArgs.Empty);
    }
    
    

    private void EnableChainsaw()
    {
        chainsawTransform.gameObject.SetActive(true);
    }

    private void DisableChainsaw()
    {
        chainsawTransform.gameObject.SetActive(false);
    }
}