using UnityEngine;

public class EnemyRenderHandler : MonoBehaviour
{
    [SerializeField] private EnemyPathfinder enemyPathfinder;

    private EnemyHealthHandler enemyHealth;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private bool isDead = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth = enemyPathfinder.GetComponent<EnemyHealthHandler>();
    }
    private void Start()
    {
        enemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
    }

    private void EnemyHealth_OnDieEvent(object sender, System.EventArgs e)
    {
        isDead = true;
    }

    private void Update()
    {
        if (isDead == true) 
            return;

        HandleRotation();
        HandleAnimation();
    }
    private void HandleAnimation()
    {
        animator.SetBool(IS_WALKING, enemyPathfinder.IsWalking());
    }

    private void HandleRotation()
    {
        if (enemyPathfinder.Target().transform.position.x >= transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (enemyPathfinder.Target().transform.position.x <= transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }
}
