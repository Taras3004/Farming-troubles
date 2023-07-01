using CodeMonkey.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class Testing : MonoBehaviour
{
    private PlayerWeaponHandler playerWeaponHandler;
    private PlayerMovement playerMovement;
    private bool isSubscribedToWeaponEvents;

    private float nextSpawnDirtTime;
    
    private void Start()
    {
        //playerWeaponHandler = PlayerWeaponHandler.Instance;
        playerMovement = PlayerMovement.Instance;
    }
    private void Update()
    {
        //HandleWeaponEvent();
        TrySpawnDirtParticles();
    }

    private void HandleWeaponEvent()
    {
        if (playerWeaponHandler.GetCurrentWeapon() != null && isSubscribedToWeaponEvents == false)
        {
            playerWeaponHandler.GetCurrentWeapon().OnFireAction += Weapon_OnFireAction;
            isSubscribedToWeaponEvents = true;
        }
        else if (playerWeaponHandler.GetCurrentWeapon() == null && isSubscribedToWeaponEvents == true)
        {
            playerWeaponHandler.GetCurrentWeapon().OnFireAction -= Weapon_OnFireAction;
            isSubscribedToWeaponEvents = false;
        }
    }

    private void TrySpawnDirtParticles()
    {
        if(Time.time < nextSpawnDirtTime)
            return;
        
        if (playerMovement.IsWalking())
        {
            DirtParticleSystemHandler.Instance.SpawnDirt(playerMovement.transform.position + new Vector3(0, -0.5f), playerMovement.GetMoveDir() * -1f);
            nextSpawnDirtTime = Time.time + 0.2f;
        }
    }
    private void Weapon_OnFireAction(object sender, Weapon.OnWeaponShootEventArgs e)
    {
        Vector3 quadPosition = e.gunEndPointPosition;
        quadPosition += (e.shootDirection * -1f) * 0.5f;

        Vector3 ShellMoveDir = UtilsClass.ApplyRotationToVector(e.shootDirection, Random.Range(-85f, -95f));
        //ShellParticleSystemHandler.Instance.SpawnShell(quadPosition, ShellMoveDir);
    }

}
