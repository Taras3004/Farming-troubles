using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    private const string MENU_SCENE_NAME = "Menu";
    private const string SFX_MIXER_VOLUME = "SfxVolume";
    private const string MUSIC_MIXER_VOLUME = "MusicVolume";

    [Header("Buttons")] 
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;

    [Header("Sliders")] 
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [Header(" ")] 
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        InitializeVolume();
        InitializeUIEvents();
    }

    private void InitializeUIEvents()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
    }
    private void InitializeVolume()
    {
        float sfxVolume = PlayerPrefs.GetFloat(GamePlayerPrefs.SFX_VOLUME, 0.5f);
        SetupVolume(sfxSlider, SFX_MIXER_VOLUME, sfxVolume);
        float musicVolume = PlayerPrefs.GetFloat(GamePlayerPrefs.MUSIC_VOLUME, 0.5f);
        SetupVolume(musicSlider, MUSIC_MIXER_VOLUME, musicVolume);
    }
    private void SetupVolume(Slider slider, string audioMixerVolume, float value)
    {
        slider.value = value;
        audioMixer.SetFloat(audioMixerVolume, value);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        audioMixer.SetFloat(MUSIC_MIXER_VOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(GamePlayerPrefs.MUSIC_VOLUME, value);
        PlayerPrefs.Save();
    }

    private void OnSfxSliderValueChanged(float value)
    {
        audioMixer.SetFloat(SFX_MIXER_VOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(GamePlayerPrefs.SFX_VOLUME, value);
        PlayerPrefs.Save();
    }

    private void OnExitButtonClicked()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }

    private void OnResumeButtonClicked()
    {
        DisablePanel();
    }

    public void EnablePanel()
    {
        gameObject.SetActive(true);
        GameInput.Instance.DisablePlayerActions();
        PlayerMovement.Instance.DisableMovement();
    }

    private void DisablePanel()
    {
        gameObject.SetActive(false);
        GameInput.Instance.EnablePlayerActions();
        PlayerMovement.Instance.EnableMovement();
    }
}