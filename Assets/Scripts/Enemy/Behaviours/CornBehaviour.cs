using UnityEngine;

public class CornBehaviour : EnemyBehaviourHandler
{
    [Header("Corn")] 
    [SerializeField] private int retreatDistance;

    private bool isWalking;
    private float retreatSpeed = 2f;
    public override bool IsWalking()
    {
        return isWalking;
    }

    protected override void HandleBehaviour()
    {
        float distance = Vector2.Distance(Tr.position, Target().position);

        Vector2 dir = Target().position - Tr.position;
        
        if (distance < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(Tr.position,
                -dir, Time.deltaTime * retreatSpeed);
            isWalking = true;
        }
        else if (AIPath.desiredVelocity != Vector3.zero)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }
}