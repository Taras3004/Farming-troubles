using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializeField] private MMF_Player useFeedback;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.Heal();
            useFeedback.PlayFeedbacks();
            Destroy(gameObject);
        }
    }
}
