using UnityEngine;

public class BroccoliBehaviour : EnemyBehaviourHandler
{
    private float aimingTimeMax = 2.5f;
    private float currAimingTime;
    
    protected override void HandleBehaviour()
    {
        if (IsWalking() == false)
        {
            currAimingTime -= Time.deltaTime;
        }
        else
        {
            currAimingTime = aimingTimeMax;
        }
    }
}
