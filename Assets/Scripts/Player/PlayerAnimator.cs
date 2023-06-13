using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player player;
    private PlayerWeaponHandler playerWeaponAim;

    private SpriteRenderer spriteRenderer;

    private const string IS_WALKING = "IsWalking";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerWeaponAim = player.GetComponent<PlayerWeaponHandler>();
    }
    private void Start()
    {
        GameInput.Instance.OnLookRight += GameInput_OnLookRight;
        GameInput.Instance.OnLookLeft += GameInput_OnLookLeft;
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
        animator.SetBool(IS_WALKING, player.IsWalking());
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
