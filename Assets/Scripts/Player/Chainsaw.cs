using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chainsaw : MonoBehaviour
{
    public event EventHandler OnStartedDamaging;
    public event EventHandler OnFinishedDamaging;

    private List<EnemyHealthHandler> hittedEnemies = new List<EnemyHealthHandler>();
    private float damagingDelayMax = 0.3f;
    private float damagingDelay;
    private Color particlesColor;

    private bool isDamaging()
    {
        return hittedEnemies != null;
    }
    public Color PariclesColor()
    {
        return particlesColor;
    }

    private void Update()
    {
        HandleDamaging();
    }
    private void HandleDamaging()
    {
        if (isDamaging() == false)
            return;

        damagingDelay += Time.deltaTime;
        if(damagingDelay >= damagingDelayMax)
        {
            for(int i = 0; i < hittedEnemies.Count; i ++)
            {
                hittedEnemies[i].Hit(GameInput.Instance.GetAimDirectionVector());
            }
            damagingDelay = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealthHandler enemyHealthHandler))
        {
            particlesColor = collision.GetComponent<EnemyHealthHandler>().BloodColor();
            hittedEnemies.Add(enemyHealthHandler);
            OnStartedDamaging?.Invoke(this, EventArgs.Empty);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyHealthHandler enemyHealthHandler))
        {
            hittedEnemies.Remove(enemyHealthHandler);
            if (hittedEnemies.Count == 0)
            {
                OnFinishedDamaging?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
