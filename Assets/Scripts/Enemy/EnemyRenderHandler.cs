using System;
using UnityEngine;

public class EnemyRenderHandler : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    
    [SerializeField] private EnemyBehaviourHandler enemyBehaviourHandler;

    private EnemyHealthHandler enemyHealth;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isDead ;
    private bool isTargetFinded;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth = enemyBehaviourHandler.GetComponent<EnemyHealthHandler>();
    }
    private void Start()
    {
        enemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
        enemyBehaviourHandler.OnFindPlayerAction += EnemyBehaviourHandlerOnOnFindPlayerAction;
    }

    private void EnemyBehaviourHandlerOnOnFindPlayerAction(object sender, EventArgs e)
    {
        isTargetFinded = true;
    }

    private void EnemyHealth_OnDieEvent(object sender, EventArgs e)
    {
        isDead = true;
    }

    private void Update()
    {
        if (isDead) 
            return;

        HandleAnimation();
        if(isTargetFinded)
            HandleRotation();
    }

    private void HandleAnimation()
    {
        animator.SetBool(IS_WALKING, enemyBehaviourHandler.IsWalking());
    }

    private void HandleRotation()
    {
        if (enemyBehaviourHandler.Target().transform.position.x >= transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else if (enemyBehaviourHandler.Target().transform.position.x <= transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
    }
}
