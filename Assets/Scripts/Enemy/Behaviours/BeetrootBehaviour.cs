using UnityEngine;

public class BeetrootBehaviour : EnemyBehaviourHandler
{
    private float defaultStopDistance;
    private float decreasedStopDistance;
    private bool isDistanceToPlayerDecreased;

    private new void Awake()
    {
        base.Awake();
        defaultStopDistance = stopDistance;
        decreasedStopDistance = stopDistance / 2;
    }

    protected override void HandleBehaviour()
    {
        HandleDistanceToPlayer();
    }

    private void HandleDistanceToPlayer()
    {
        Vector2 dirToTarget = Target().position - transform.position;
        RaycastHit2D raycastHit = Physics2D.Raycast(Tr.position,
            dirToTarget, Mathf.Infinity, ObstaclesLayer());

        if (raycastHit.collider.gameObject == Target().gameObject)
        {
            if (isDistanceToPlayerDecreased)
            {
                ReturnEndDistanceToDefault();
            }
        }
        else
        {
            DecreaseEndDistance();
        }

        if (!isDistanceToPlayerDecreased) return;
        
        float distanceToTarget =
            Vector2.Distance(Tr.position, Target().position);

        if (distanceToTarget > defaultStopDistance)
        {
            ReturnEndDistanceToDefault();
        }
    }

    private void DecreaseEndDistance()
    {
        isDistanceToPlayerDecreased = true;
        AIPath.endReachedDistance = decreasedStopDistance;
    }

    private void ReturnEndDistanceToDefault()
    {
        AIPath.endReachedDistance = defaultStopDistance;
    }
}