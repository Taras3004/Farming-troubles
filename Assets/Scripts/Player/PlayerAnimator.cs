using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimator : MonoBehaviour
{
    [FormerlySerializedAs("player")] [SerializeField] private PlayerMovement playerMovement;
    private PlayerWeaponHandler playerWeaponAim;

    private SpriteRenderer spriteRenderer;

    private const string IS_WALKING = "IsWalking";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerWeaponAim = playerMovement.GetComponent<PlayerWeaponHandler>();
    }
    private void Start()
    {
        GameInput.Instance.OnLookRight += GameInput_OnLookRight;
        GameInput.Instance.OnLookLeft += GameInput_OnLookLeft;
        PlayerHealth.Instance.OnDie += InstanceOnOnDie;
    }

    private void InstanceOnOnDie(object sender, EventArgs e)
    {
        GameInput.Instance.OnLookRight -= GameInput_OnLookRight;
        GameInput.Instance.OnLookLeft -= GameInput_OnLookLeft;
    }

    private void GameInput_OnLookLeft(object sender, System.EventArgs e)
    {
        LookLeft();
    }

    private void GameInput_OnLookRight(object sender, System.EventArgs e)
    {
        LookRight();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, playerMovement.IsWalking());
    }

    private void LookRight()
    {
        spriteRenderer.flipX = false;
    }
    private void LookLeft()
    {
        spriteRenderer.flipX = true;
    }
}
