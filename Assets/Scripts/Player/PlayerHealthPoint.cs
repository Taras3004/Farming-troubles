using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthPoint : MonoBehaviour
{
    [SerializeField] private MMF_Player spawnFeedback;
    [SerializeField] private MMF_Player destroyFeedback;

    public void Initialize()
    {
        spawnFeedback.PlayFeedbacks();
    }
    public void Destroy()
    {
        destroyFeedback.PlayFeedbacks();
    }
}
