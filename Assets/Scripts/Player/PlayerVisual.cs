using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player footstepFeedback;
    [SerializeField] private MMF_Player hitFeedback; 
    [SerializeField] private MMF_Player dashFeedback;
    [SerializeField] private MMF_Player dieFeedback;
    [SerializeField] private MMF_Player pickupFeedback;
    [SerializeField] private MMF_Player dropFeedback;
    
    private PlayerHealth playerHealth;
    private PlayerWeaponHandler playerWeaponHandler;
    private PlayerMovement playerMovement;

    private float footstepTimer;
    private readonly float footstepTimerMax = 0.5f;
    
    private float nextSpawnDirtTime;
    private readonly float dirtSpawnRate = 0.2f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        playerHealth.OnHealthDecreased += PlayerHealth_OnHealthDecreased;
        playerWeaponHandler.OnWeaponPickedAction += PlayerWeaponHandlerOnOnWeaponPickedAction;
        playerWeaponHandler.OnWeaponDroppedAction += PlayerWeaponHandlerOnOnWeaponDroppedAction;
        playerMovement.OnDashAction += PlayerMovementOnDashAction;
        playerHealth.OnDie += PlayerHealth_OnDie;
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        dieFeedback.PlayFeedbacks();
    }

    private void PlayerWeaponHandlerOnOnWeaponDroppedAction(object sender, EventArgs e)
    {
        dropFeedback.PlayFeedbacks();
    }

    private void PlayerWeaponHandlerOnOnWeaponPickedAction(object sender, EventArgs e)
    {
        pickupFeedback.PlayFeedbacks();
    }

    private void PlayerMovementOnDashAction(object sender, EventArgs e)
    {
        dashFeedback.PlayFeedbacks();
    }

    private void PlayerHealth_OnHealthDecreased(object sender, EventArgs e)
    {
        hitFeedback.PlayFeedbacks();
    }

    private void Update()
    {
        HandleFootstepSounds();
        
        TrySpawnDirtParticles();
    }
    private void HandleFootstepSounds()
    {
        if (playerMovement.IsWalking() == false)
        {
            footstepTimer = footstepTimerMax;
            return;
        }
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;
            footstepFeedback.PlayFeedbacks();
        }
    }
    
    private void TrySpawnDirtParticles()
    {
        if (playerHealth.CurrentHealth() <= 0)
            return;
        if (Time.time < nextSpawnDirtTime)
            return;

        if (playerMovement.IsWalking())
        {
            DirtParticleSystemHandler.Instance.SpawnDirt(playerMovement.transform.position + new Vector3(0, -0.3f),
                playerMovement.GetMoveDir() * -1f);
            nextSpawnDirtTime = Time.time + dirtSpawnRate;
        }
    }

}
