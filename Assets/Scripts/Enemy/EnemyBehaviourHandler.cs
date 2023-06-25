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
        if (Player.Instance == null)
            return null;
        return Player.Instance.transform;
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
        StartCoroutine(HandlePatroling());
        state = State.Patroling;
    }

    protected virtual void Start()
    {
        EnemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
    }

    private void EnemyHealth_OnDieEvent(object sender, System.EventArgs e)
    {
        DisableMovement();
        Destroy(followObject);
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
            StopCoroutine(HandlePatroling());
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
            if (hitObjects[i].TryGetComponent(out Player player))
            {
                OnFindPlayerAction?.Invoke(this, EventArgs.Empty);
                AIDestinationSetter.target = player.transform;
                AIPath.endReachedDistance = stopDistance;
                state = State.FollowingPlayer;
            }
        }
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