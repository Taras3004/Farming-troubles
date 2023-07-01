using CodeMonkey.Utils;
using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private ShellParticleSystemHandler.ShellUVIndex weaponType;
    
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

    private void Weapon_OnFireAction(object sender, Weapon.OnWeaponShootEventArgs e)
    {
        shotFeedback.PlayFeedbacks();
        
        Vector3 quadPosition = e.gunEndPointPosition;
        quadPosition += (e.shootDirection * -1f) * 0.5f;

        Vector3 ShellMoveDir = UtilsClass.ApplyRotationToVector(e.shootDirection, Random.Range(-85f, -95f));
        ShellParticleSystemHandler.Instance.SpawnShell(quadPosition, ShellMoveDir, weaponType);
    }
}
