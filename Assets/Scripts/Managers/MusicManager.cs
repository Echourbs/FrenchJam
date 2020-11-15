using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    #region VARIABLES
    public static MusicManager instance;
    public float musicTransitionDuration = 2f, currentVolume, maxVolume = .15f; 
    public bool isMuted = false;
    [HideInInspector]
    public AudioSource audioSource;
    private bool fadingIn, fadingOut, changeMusic;    
    private AudioClip nextMusic;
    #endregion

    #region METHODS
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if (fadingIn) {
            audioSource.volume += .2f * Time.deltaTime / musicTransitionDuration;

            if (audioSource.volume >= currentVolume) {
                fadingIn = false;
                audioSource.volume = currentVolume;
            }
        } else if (fadingOut) {
            audioSource.volume -= currentVolume * Time.deltaTime / musicTransitionDuration;

            if (audioSource.volume <= 0) {
                if (changeMusic) {
                    fadingIn = true;
                    if (nextMusic != null) SetNewMusic(nextMusic);
                    audioSource.Play();
                    changeMusic = false;
                    nextMusic = null;
                }
                fadingOut = false;
                audioSource.volume = 0;
            }
        }
    }

    /// <summary>
    /// Instantaneously sets a new audio clip to the audio source at this manager
    /// </summary>
    /// <param name="volume">New volume level</param>
    public void SetNewMusic(AudioClip audioClip) {
        audioSource.clip = audioClip;
    }

    /// <summary>
    /// Fades out the old audio and fades in the new one.
    /// </summary>
    /// <param name="audio">The new audio to be played</param>
    public void ChangeMusicSmoothly(AudioClip audio) {
        instance.fadingOut = true;
        instance.fadingIn = false;
        instance.changeMusic = true;
        instance.nextMusic = audio;
    }


    /// <summary>
    /// Don't care about the current music playing, just plays a new one using a fade in effect
    /// </summary>
    public void PlayMusicImmediatelyWithFadeIn(AudioClip audio) {
        audioSource.volume = 0;
        SetNewMusic(audio);
        audioSource.Play();
        instance.fadingOut = false;
        instance.fadingIn = true;
    }

    /// <summary>
    /// Returns the audio clip currently playing
    /// </summary>
    /// <returns>Audio clip</returns>
    public AudioClip GetAudioClipPlaying() {
        return audioSource.clip;
    }

    /// <summary>
    /// Sets a new current volume to this manager
    /// </summary>
    /// <param name="volume">New volume level</param>
    public void SetNewMusicVolume(float volume) {
        currentVolume = volume;
        SetAudioSourceVolume(volume);
    }

    /// <summary>
    /// Sets a new volume to the audio source
    /// </summary>
    /// <param name="newVolume">New volume level</param>
    private void SetAudioSourceVolume(float newVolume) {
        audioSource.volume = newVolume;
    }

    /// <summary>
    /// Sets the current volume to the max volume
    /// </summary>
    public void SetCurrentMusicVolumeToMax() {
        currentVolume = maxVolume;
        SetAudioSourceVolume(currentVolume);
    }

    /// <summary>
    /// Mutes the audio source at this manager
    /// </summary>
    /// <param name="mute">New state</param>
    public void Mute(bool mute) {
        audioSource.mute = mute;
        instance.isMuted = mute;
    }
    #endregion
}
