using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
public class EnemyPathfinder : MonoBehaviour
{
    [SerializeField] private LayerMask obstaclesLayer;

    [SerializeField] protected int stopDistance;

    protected Transform Tr;
    protected AIPath AIPath;
    protected EnemyHealthHandler EnemyHealth;
    private AIDestinationSetter aiDestinationSetter;

    private float defaultEndDistance;

    public bool IsWalking()
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
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        EnemyHealth = GetComponent<EnemyHealthHandler>();
        Tr = GetComponent<Transform>();
        StartCoroutine(SetTarget());
    }

    protected virtual void Start()
    {
        EnemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
    }

    private void EnemyHealth_OnDieEvent(object sender, System.EventArgs e)
    {
        DisableMovement();
    }

    private void DisableMovement()
    {
        AIPath.canMove = false;
    }

    private void Update()
    {
        if (AIPath.canMove == false)
            return;
        HandleBehaviour();
    }

    protected virtual void HandleBehaviour()
    {
    }

    private IEnumerator SetTarget()
    {
        yield return new WaitUntil(() => Target() != null);
        aiDestinationSetter.target = Target();
        AIPath.endReachedDistance = stopDistance;
    }
}