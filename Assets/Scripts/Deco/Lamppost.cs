using MoreMountains.Feedbacks;
using UnityEngine;

public class Lamppost : MonoBehaviour
{
    [SerializeField] private bool isActiated = true;
    [SerializeField] private MMF_Player activateFeedback;

    private void Awake()
    {
        if(isActiated)
            activateFeedback.PlayFeedbacks();
    }
}
