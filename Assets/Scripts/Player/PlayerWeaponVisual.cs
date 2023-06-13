using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponVisual : MonoBehaviour
{
    [SerializeField] private Image dropProgressSprite;
    [SerializeField] private Image reloadProgressSprite;
    [SerializeField] private TMP_Text ammoText;

    private PlayerWeaponHandler playerWeaponHandler;

    private void Awake()
    {
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
    }
    private void Start()
    {
        dropProgressSprite.gameObject.SetActive(false);
        reloadProgressSprite.gameObject.SetActive(false);

        playerWeaponHandler.OnWeaponDropProgress += PlayerWeaponHandler_OnWeaponDropProgress;
        playerWeaponHandler.OnWeaponDroppedAction += PlayerWeaponHandler_OnWeaponDroppedAction;
        playerWeaponHandler.OnWeaponPickupedAction += PlayerWeaponHandler_OnWeaponPickupedAction;
        GameInput.Instance.OnDropWeaponFinishAction += GameInput_OnDropWeaponFinishAction;
        GameInput.Instance.OnDropWeaponStartAction += GameInput_OnDropWeaponStartAction;

        SubscribeToWeaponEvents();
    }

    private void PlayerWeaponHandler_OnWeaponPickupedAction(object sender, System.EventArgs e)
    {
        SubscribeToWeaponEvents();
        UpdateAmmoText();
    }

    private void PlayerWeaponHandler_OnWeaponDroppedAction(object sender, System.EventArgs e)
    {
        UnsubscribeWeaponEvents();
    }

    private void SubscribeToWeaponEvents()
    {
        if (playerWeaponHandler.IsFull())
        {
            UpdateAmmoText();
            playerWeaponHandler.GetCurrentWeapon().OnBulletAmountChanged += CurrentWeapon_OnBulletAmountChanged;
            playerWeaponHandler.GetCurrentWeapon().OnReloadProgressAction += CurrentWeapon_OnReloadProgressAction;
            playerWeaponHandler.GetCurrentWeapon().OnReloadStartedAction += PlayerVisual_OnReloadStartedAction;
            playerWeaponHandler.GetCurrentWeapon().OnReloadFinishedAction += PlayerVisual_OnReloadFinishedAction;
        }
    }
    private void UpdateAmmoText()
    {
        ammoText.text = $"{playerWeaponHandler.GetCurrentWeapon().GetBulletAmount()} / {playerWeaponHandler.GetCurrentWeapon().GetBulletAmountBalance()}";
    }
    private void PlayerVisual_OnReloadFinishedAction(object sender, System.EventArgs e)
    {
        reloadProgressSprite.gameObject.SetActive(false);
    }

    private void PlayerVisual_OnReloadStartedAction(object sender, System.EventArgs e)
    {
        reloadProgressSprite.gameObject.SetActive(true);
    }

    private void CurrentWeapon_OnReloadProgressAction(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        reloadProgressSprite.fillAmount = e.progressNormalized;
    }

    private void UnsubscribeWeaponEvents()
    {
        playerWeaponHandler.GetCurrentWeapon().OnBulletAmountChanged -= CurrentWeapon_OnBulletAmountChanged;
        ammoText.text = "";
    }

    private void CurrentWeapon_OnBulletAmountChanged(object sender, Weapon.OnBulletAmountChangedEventArgs e)
    {
        ammoText.text = $"{e.amount} / {playerWeaponHandler.GetCurrentWeapon().GetBulletAmountBalance()}";
    }

    private void GameInput_OnDropWeaponStartAction(object sender, System.EventArgs e)
    {
        if (playerWeaponHandler.IsFull())
        {
            dropProgressSprite.gameObject.SetActive(true);
        }
        reloadProgressSprite.gameObject.SetActive(false);
    }

    private void GameInput_OnDropWeaponFinishAction(object sender, System.EventArgs e)
    {
        dropProgressSprite.gameObject.SetActive(false);
    }

    private void PlayerWeaponHandler_OnWeaponDropProgress(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        dropProgressSprite.fillAmount = e.progressNormalized;

        if (e.progressNormalized >= 1f)
            dropProgressSprite.gameObject.SetActive(false);
    }
}
