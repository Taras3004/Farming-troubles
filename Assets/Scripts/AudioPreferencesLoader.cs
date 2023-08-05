using System;
using MoreMountains.Tools;
using UnityEngine;

public class AudioPreferencesLoader : MonoBehaviour
{
    private void Start()
    {
        float sfxVolume = PlayerPrefs.GetFloat(GamePlayerPrefs.SFX_VOLUME, 0.5f);
        SetupAudio(VolumeType.Sfx, sfxVolume);
        
        float musicVolume = PlayerPrefs.GetFloat(GamePlayerPrefs.MUSIC_VOLUME, 0.5f);
        SetupAudio(VolumeType.Music, musicVolume);
    }

    private void SetupAudio(VolumeType volumeType, float value)
    {
        if (volumeType == VolumeType.Sfx)
            MMSoundManager.Instance.SetVolumeSfx(value);
        else if (volumeType == VolumeType.Music)
            MMSoundManager.Instance.SetVolumeMusic(value);
    }
}
