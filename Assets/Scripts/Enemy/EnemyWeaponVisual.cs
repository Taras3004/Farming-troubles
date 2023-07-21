using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyWeaponVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player shotFeedback;

    private EnemyWeapon weapon;

    private void Awake()
    {
        weapon = GetComponent<EnemyWeapon>();
    }
    private void Start()
    {
        weapon.OnFireAction += Weapon_OnFireAction;
    }

    private void Weapon_OnFireAction(object sender, System.EventArgs e)
    {
        shotFeedback.PlayFeedbacks();
    }
}
