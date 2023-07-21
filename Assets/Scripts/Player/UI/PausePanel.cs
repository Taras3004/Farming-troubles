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

    [FormerlySerializedAs("playButton")]
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
        float sfxVolume = PlayerPrefs.GetFloat(GamePlayerPrefs.SFX_VOLUME, 0.5f);
        SetupSlider(sfxSlider, sfxVolume);
        float musicVolume = PlayerPrefs.GetFloat(GamePlayerPrefs.MUSIC_VOLUME, 0.5f);
        SetupSlider(sfxSlider, musicVolume);

        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
    }

    private void SetupSlider(Slider slider, float value)
    {
        slider.value = value;
    }

    private void OnMusicSliderValueChanged(float value)
    {
        audioMixer.SetFloat(MUSIC_MIXER_VOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(GamePlayerPrefs.MUSIC_VOLUME, value);
    }

    private void OnSfxSliderValueChanged(float value)
    {
        audioMixer.SetFloat(SFX_MIXER_VOLUME, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(GamePlayerPrefs.SFX_VOLUME, value);
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