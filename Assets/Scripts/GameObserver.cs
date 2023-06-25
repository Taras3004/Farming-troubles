using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObserver : MonoBehaviour
{
    public static GameObserver Instance;

    public event EventHandler OnLevelPassed;
    
    private EnemyHealthHandler[] enemies;
    private List<EnemyHealthHandler> deadEnemies = new List<EnemyHealthHandler>();

    private void Awake()
    {
        Instance = this;
        
        enemies = FindObjectsOfType<EnemyHealthHandler>();
        
    }

    public void AddDeadEnemy(EnemyHealthHandler enemy)
    {
        deadEnemies.Add(enemy);

        if (enemies.Length == deadEnemies.Count)
        {
            OnLevelPassed?.Invoke(this, EventArgs.Empty);
        }
    }
}
