using System;
using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(OnionBehaviour))]
public class OnionVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player dashFeedback;
    private OnionBehaviour onionBehaviour;

    private void Awake()
    {
        onionBehaviour = GetComponent<OnionBehaviour>();
    }

    private void Start()
    {
        onionBehaviour.OnDashAction += OnionBehaviourOnOnDashAction;
    }

    private void OnionBehaviourOnOnDashAction(object sender, EventArgs e)
    {
        dashFeedback.PlayFeedbacks();
    }
    
}
