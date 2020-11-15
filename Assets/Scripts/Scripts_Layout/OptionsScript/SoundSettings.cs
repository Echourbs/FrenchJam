using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    public static AudioMixer audioMixer;
    public MusicManager mm;
    public SoundManager sm;

    private string musicVolumePlayerPrefKey = "Music";
    private string sfxVolumePlayerPrefKey = "Sfx";
    private string muteSoundPlayerPrefKey = "Mute";

    [Header("Buttons and Sliders")]
    public Toggle mute;
    public Slider musicVolume;
    public Slider sfxVolume;

    void Start()
    {        
        LoadSettings();
    }

    public void LoadSettings()
    {
        PlayerPrefs.GetString(muteSoundPlayerPrefKey);      
        sfxVolume.value = PlayerPrefs.GetFloat(sfxVolumePlayerPrefKey, AudioPref.SFX);
        musicVolume.value = PlayerPrefs.GetFloat(musicVolumePlayerPrefKey, AudioPref.musica);
        mute.isOn = PlayerPrefs.GetInt(muteSoundPlayerPrefKey, AudioPref.mudo ? 1 : 0) > 0;
    }

    public void SetVolumeMusic(float music)
    {
        MusicManager.instance.SetNewMusicVolume(music);
        PlayerPrefs.SetFloat(musicVolumePlayerPrefKey, music);
        AudioPref.musica = music;
        //audioMixer.SetFloat("music", music);
    }

    public void SetVolumeSFX(float sfx)
    {
        MusicManager.instance.SetNewMusicVolume(sfx);
        PlayerPrefs.SetFloat(sfxVolumePlayerPrefKey, sfx);
        AudioPref.SFX = sfx;
    }

    public void Mute(bool mute)
    {
        MusicManager.instance.Mute(mute);
        AudioPref.mudo = mute;
        if (mute)
        {
            PlayerPrefs.SetInt(muteSoundPlayerPrefKey, mute ? 1 : 0);
        }
        else
        {
            PlayerPrefs.SetInt(muteSoundPlayerPrefKey, mute ? 1 : 0);
        }
    }
}
