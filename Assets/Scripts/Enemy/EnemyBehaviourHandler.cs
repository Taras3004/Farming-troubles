using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
public class EnemyBehaviourHandler : MonoBehaviour
{
    public event EventHandler OnFindPlayerAction;

    [SerializeField] private LayerMask obstaclesLayer;
    [SerializeField] protected int stopDistance;
    [SerializeField] private float startFollowDistance;

    private GameObject followObject;
    private State state;
    protected Transform Tr;
    protected AIPath AIPath;
    protected EnemyHealthHandler EnemyHealth;
    protected AIDestinationSetter AIDestinationSetter;

    public virtual bool IsWalking()
    {
        return AIPath.desiredVelocity != Vector3.zero;
    }

    public Transform Target()
    {
        if (PlayerMovement.Instance == null)
            return null;
        return PlayerMovement.Instance.transform;
    }

    public LayerMask ObstaclesLayer()
    {
        return obstaclesLayer;
    }

    public bool IsReachedEndDistance()
    {
        return AIPath.reachedEndOfPath;
    }

    protected virtual void Awake()
    {
        AIPath = GetComponent<AIPath>();
        AIDestinationSetter = GetComponent<AIDestinationSetter>();
        EnemyHealth = GetComponent<EnemyHealthHandler>();
        Tr = GetComponent<Transform>();
        //StartCoroutine(HandlePatroling());
    }

    protected virtual void Start()
    {
        state = State.Patroling;
        EnemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
        EnemyHealth.OnHitEvent += EnemyHealthOnOnHitEvent;
    }

    private void EnemyHealth_OnDieEvent(object sender, EventArgs e)
    {
        DisableMovement();
        Destroy(followObject);
    }

    private void EnemyHealthOnOnHitEvent(object sender, EventArgs e)
    {
        if(Target() != null)
            return;
        
        Transform player = PlayerMovement.Instance.transform;
        SetPlayer(player);
    }

    private void DisableMovement()
    {
        AIPath.canMove = false;
    }

    protected virtual void Update()
    {
        if (AIPath.canMove == false)
            return;
        if (state == State.Patroling)
        {
            FindPlayer();
        }
        else if (state == State.FollowingPlayer)
        {
            //StopCoroutine(HandlePatroling());
            HandleBehaviour();
        }
    }

    private IEnumerator HandlePatroling()
    {
        if (state != State.Patroling)
            yield break;
        float timeToChangePatrolTarget = Random.Range(8, 17);
        // ReSharper disable once UselessComparisonToIntegralConstant
        for (int i = 0; i < Mathf.Infinity; i++)
        {
            SetPatrolTarget();
            yield return new WaitForSeconds(timeToChangePatrolTarget);
            if (state != State.Patroling)
                yield break;
        }
    }

    private void SetPatrolTarget()
    {
        if (followObject == null)
        {
            string followObjectName = "FollowObject " + gameObject.name;
            followObject = new GameObject(followObjectName);
        }

        int randX = Random.Range(-3, 4);
        int randY = Random.Range(-3, 4);
        followObject.transform.position = transform.position + new Vector3(randX, randY);
        AIPath.endReachedDistance = 0;
        AIDestinationSetter.target = followObject.transform;
    }

    private void FindPlayer()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position,
            startFollowDistance);
        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (hitObjects[i].TryGetComponent(out PlayerMovement player))
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position,
                    startFollowDistance, obstaclesLayer);

                if (hit.collider == null)
                    return;
                if (hit.collider.gameObject != player.gameObject)
                    return;

                SetPlayer(player.transform);
            }
        }
    }

    private void SetPlayer(Transform playerTransform)
    {
        OnFindPlayerAction?.Invoke(this, EventArgs.Empty);
        AIDestinationSetter.target = playerTransform;
        AIPath.endReachedDistance = stopDistance;
        state = State.FollowingPlayer;
    }

    protected virtual void HandleBehaviour()
    {
    }

    public enum State
    {
        Patroling,
        FollowingPlayer
    }
}