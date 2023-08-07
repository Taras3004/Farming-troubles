using UnityEngine;

public class ObstacleActivator : MonoBehaviour
{
    private readonly float showRadius = 10f;
    private readonly float distanceToUpdate = 3f;
    private Obstacle[] obstacles;
    private Vector3 playerPositionWhenUpdated;
    
    private void Start()
    {
        obstacles = FindObjectsOfType<Obstacle>(true);
        UpdateObstaclesState();
    }

    private void Update()
    {
        HandleUpdating();
    }

    private void HandleUpdating()
    {
        float distance = Vector2.Distance(transform.position, playerPositionWhenUpdated);
        if(distance > distanceToUpdate)
            UpdateObstaclesState();
    }
    
    private void UpdateObstaclesState()
    {
        foreach (var obstacle in obstacles)
        {
            float distance = Vector2.Distance(transform.position, obstacle.transform.position);
            if (distance > showRadius)
                obstacle.Disable();
            else
                obstacle.Enable();
        }

        playerPositionWhenUpdated = transform.position;
    }
}