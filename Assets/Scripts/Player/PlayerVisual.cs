using System;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player footstepFeedback;
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private MMF_Player jerkFeedback;
    [SerializeField] private MMF_Player pickupFeedback;
    [SerializeField] private MMF_Player dropFeedback;
    
    private PlayerHealth playerHealth;
    private PlayerWeaponHandler playerWeaponHandler;
    private Player player;

    private float footstepTimer;
    private float footstepTimerMax = 0.75f;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerWeaponHandler = GetComponent<PlayerWeaponHandler>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        playerHealth.OnHealthDecreased += PlayerHealth_OnHealthDecreased;
        playerWeaponHandler.OnWeaponPickupedAction += PlayerWeaponHandlerOnOnWeaponPickupedAction;
        playerWeaponHandler.OnWeaponDroppedAction += PlayerWeaponHandlerOnOnWeaponDroppedAction;
        player.OnJerkAction += Player_OnJerkAction;
    }

    private void PlayerWeaponHandlerOnOnWeaponDroppedAction(object sender, EventArgs e)
    {
        dropFeedback.PlayFeedbacks();
    }

    private void PlayerWeaponHandlerOnOnWeaponPickupedAction(object sender, EventArgs e)
    {
        pickupFeedback.PlayFeedbacks();
    }

    private void Player_OnJerkAction(object sender, EventArgs e)
    {
        jerkFeedback.PlayFeedbacks();
    }

    private void PlayerHealth_OnHealthDecreased(object sender, EventArgs e)
    {
        hitFeedback.PlayFeedbacks();
    }

    private void Update()
    {
        HandleFootstepSounds();
    }
    private void HandleFootstepSounds()
    {
        if (player.IsWalking() == false)
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

}
