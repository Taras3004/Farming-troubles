using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private Color bloodColor = Color.red;

    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private MMF_Player dieFeedback;

    private EnemyHealthHandler enemyHealth;

    public Color BloodColor()
    {
        return bloodColor;
    }

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealthHandler>();
    }
    private void Start()
    {
        enemyHealth.OnHitEvent += EnemyHealth_OnHitEvent;
        enemyHealth.OnDieEvent += EnemyHealth_OnDieEvent;
    }

    private void EnemyHealth_OnDieEvent(object sender, System.EventArgs e)
    {
        dieFeedback.PlayFeedbacks();
    }
    private void EnemyHealth_OnHitEvent(object sender, System.EventArgs e)
    {
        hitFeedback.PlayFeedbacks();
    }
}
