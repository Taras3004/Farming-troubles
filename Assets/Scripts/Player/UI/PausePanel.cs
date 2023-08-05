using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
        SetupVolume(sfxSlider, sfxVolume, VolumeType.Sfx);
        float musicVolume = PlayerPrefs.GetFloat(GamePlayerPrefs.MUSIC_VOLUME, 0.5f);
        SetupVolume(musicSlider, musicVolume, VolumeType.Music);
    }

    private void SetupVolume(Slider slider, float value, VolumeType volumeType)
    {
        slider.value = value;
        if (volumeType == VolumeType.Sfx)
            MMSoundManager.Instance.SetVolumeSfx(value);
        else if (volumeType == VolumeType.Music)
            MMSoundManager.Instance.SetVolumeMusic(value);
    }

    private void OnMusicSliderValueChanged(float value)
    {
        MMSoundManager.Instance.SetVolumeMusic(value);
        PlayerPrefs.SetFloat(GamePlayerPrefs.MUSIC_VOLUME, value);
        PlayerPrefs.Save();
    }

    private void OnSfxSliderValueChanged(float value)
    {
        MMSoundManager.Instance.SetVolumeSfx(value);
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

public enum VolumeType
{
    Sfx,
    Music
}