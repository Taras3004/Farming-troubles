using System;
using UnityEngine;

public class EnemyRenderHandler : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_DIED = "IsDied";

    [SerializeField] private EnemyBehaviourHandler enemyBehaviourHandler;

    private EnemyHealthHandler enemyHealth;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead;
    private bool isTargetFound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth = enemyBehaviourHandler.GetComponent<EnemyHealthHandler>();
    }

    private void Start()
    {
        enemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
        enemyBehaviourHandler.OnFindPlayerAction += EnemyBehaviourHandler_OnFindPlayerAction;
        LevelReloader.Instance.OnLevelReloaded += LevelReloader_OnLevelReloaded;
    }

    private void LevelReloader_OnLevelReloaded(object sender, EventArgs e)
    {        
        isDead = false;
        animator.SetBool(IS_DIED, false);
    }

    private void EnemyBehaviourHandler_OnFindPlayerAction(object sender, EventArgs e)
    {
        isTargetFound = true;
    }

    private void EnemyHealth_OnDieEvent(object sender, EventArgs e)
    {
        isDead = true;
        isTargetFound = false;
        animator.SetBool(IS_DIED, true);
    }

    private void Update()
    {
        if (isDead)
            return;

        HandleAnimation();
        if (isTargetFound)
            HandleRotation();
    }

    private void HandleAnimation()
    {
        animator.SetBool(IS_WALKING, enemyBehaviourHandler.IsWalking());
    }

    private void HandleRotation()
    {
        bool isRight = enemyBehaviourHandler.Target().transform.position.x <= transform.position.x;
        spriteRenderer.flipX = isRight;
    }
}