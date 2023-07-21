using System;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{
    public event EventHandler OnStartedDamaging;
    public event EventHandler OnFinishedDamaging;

    private List<EnemyHealthHandler> hitEnemies = new();
    private readonly float damagingDelayMax = 0.3f;
    private float damagingDelay;
    private Color particlesColor;

    private bool IsDamaging()
    {
        return hitEnemies != null;
    }
    public Color ColorOfParticles()
    {
        return particlesColor;
    }

    private void Update()
    {
        HandleDamaging();
    }
    private void HandleDamaging()
    {
        if (IsDamaging() == false)
            return;

        damagingDelay += Time.deltaTime;
        if(damagingDelay >= damagingDelayMax)
        {
            for(int i = 0; i < hitEnemies.Count; i ++)
            {
                hitEnemies[i].Hit(GameInput.Instance.GetAimDirectionVector());
            }
            damagingDelay = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealthHandler enemyHealthHandler))
        {
            particlesColor = collision.GetComponent<EnemyHealthHandler>().BloodColor();
            hitEnemies.Add(enemyHealthHandler);
            OnStartedDamaging?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealthHandler enemyHealthHandler))
        {
            hitEnemies.Remove(enemyHealthHandler);
            if (hitEnemies.Count == 0)
            {
                OnFinishedDamaging?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
