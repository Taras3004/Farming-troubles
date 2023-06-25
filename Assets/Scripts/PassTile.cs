using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class PassTile : MonoBehaviour
{
    [SerializeField] private MMF_Player openFeedback;
    private bool isLevelPassed;

    private void Start()
    {
        GameObserver.Instance.OnLevelPassed += InstanceOnOnLevelPassed;
    }

    private void InstanceOnOnLevelPassed(object sender, EventArgs e)
    {
        Open();
        openFeedback.PlayFeedbacks();
    }

    private void Open()
    {
        isLevelPassed = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(isLevelPassed)
            Debug.Log("next level");
    }
}
