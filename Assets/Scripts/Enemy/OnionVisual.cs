using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

[RequireComponent(typeof(OnionBehaviour))]
public class OnionVisual : MonoBehaviour
{
    [SerializeField] private MMF_Player jerkFeedback;
    private OnionBehaviour onionBehaviour;

    private void Awake()
    {
        onionBehaviour = GetComponent<OnionBehaviour>();
    }

    private void Start()
    {
        onionBehaviour.OnJerkAction += OnionBehaviourOnOnJerkAction;
    }

    private void OnionBehaviourOnOnJerkAction(object sender, EventArgs e)
    {
        jerkFeedback.PlayFeedbacks();
    }
    
}
