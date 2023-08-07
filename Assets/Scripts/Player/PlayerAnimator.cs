using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private SpriteRenderer spriteRenderer;

    private const string IS_WALKING = "IsWalking";
    private const string IS_DIED = "IsDied";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        GameInput.Instance.OnLookRight += GameInput_OnLookRight;
        GameInput.Instance.OnLookLeft += GameInput_OnLookLeft;
        PlayerHealth.Instance.OnDie += PlayerHealth_OnDie;
        LevelReloader.Instance.OnLevelReloaded += LevelReloader_OnLevelReloaded;
    }

    private void LevelReloader_OnLevelReloaded(object sender, EventArgs e)
    {
        animator.SetBool(IS_DIED, false);
        GameInput.Instance.OnLookRight += GameInput_OnLookRight;
        GameInput.Instance.OnLookLeft += GameInput_OnLookLeft;
    }

    private void PlayerHealth_OnDie(object sender, EventArgs e)
    {
        animator.SetBool(IS_DIED, true);
        GameInput.Instance.OnLookRight -= GameInput_OnLookRight;
        GameInput.Instance.OnLookLeft -= GameInput_OnLookLeft;
    }

    private void GameInput_OnLookLeft(object sender, EventArgs e)
    {
        LookLeft();
    }

    private void GameInput_OnLookRight(object sender, EventArgs e)
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
