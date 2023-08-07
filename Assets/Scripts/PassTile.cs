using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassTile : MonoBehaviour
{
    [SerializeField] private MMF_Player openFeedback;
    private bool isLevelPassed;
    private int currLevelName;

    private void Start()
    {
        LevelPassChecker.Instance.OnLevelPassed += InstanceOnOnLevelPassed;
        currLevelName = PlayerPrefs.GetInt(GamePlayerPrefs.CURRENT_LEVEL_NAME, 1);
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
        if (isLevelPassed)
        {
            int nextLevelName = currLevelName + 1;
            var scenePath = SceneUtility.GetScenePathByBuildIndex(nextLevelName);
            
            if (scenePath != "")
            {
                SceneManager.LoadScene(nextLevelName);
                PlayerPrefs.SetInt(GamePlayerPrefs.CURRENT_LEVEL_NAME, nextLevelName);
                PlayerPrefs.Save();
            }
        }
    }
}