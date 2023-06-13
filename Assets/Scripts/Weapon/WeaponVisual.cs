using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player shotFeedback;
    [SerializeField] private MMF_Player reloadFeedback;

    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }
    private void Start()
    {
        weapon.OnFireAction += Weapon_OnFireAction;
        weapon.OnReloadStartedAction += Weapon_OnReloadStartedAction;
    }

    private void Weapon_OnReloadStartedAction(object sender, System.EventArgs e)
    {
        reloadFeedback.PlayFeedbacks();
    }

    private void Weapon_OnFireAction(object sender, System.EventArgs e)
    {
        shotFeedback.PlayFeedbacks();
    }
}
