using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    [Header(" ")] 
    [SerializeField] private RectTransform optionsPanel;
    
    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void OnOptionsButtonClicked()
    {
        optionsPanel.gameObject.SetActive(true);
    }

    private void OnPlayButtonClicked()
    {
        int currLevel = PlayerPrefs.GetInt(GamePlayerPrefs.CURRENT_LEVEL_NAME, 1);
        SceneManager.LoadScene(currLevel);
    }
}
