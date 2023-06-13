
using UnityEngine;

public class CornBehaviour : EnemyPathfinder
{
    [Header("Corn")]
    [SerializeField] private int retreatDistance;

    private Rigidbody2D rb;
    private float retreatSpeed = 20f;
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void HandleBehaviour()
    {
        float distance = Vector2.Distance(Tr.position, Target().position);

        Vector2 dir =  Target().position - Tr.position;
        
        if(distance < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(Tr.position, 
                -dir, Time.deltaTime * retreatSpeed);
            
        }
    }
}
