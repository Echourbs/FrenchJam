using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    #region VARIABLES
    public static SoundManager instance;
    public AudioSource audioSource;
    public bool isMuted = false;
    public float maxVolume = 1, currentVolume = 1;
    #endregion

    #region METHODS
    private void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Mutes the music playing at the audio source from this manager
    /// </summary>
    /// <param name="mute">New state</param>
    public void Mute(bool mute) {
        audioSource.mute = mute;
        instance.isMuted = mute;
    }

    /// <summary>
    /// Sets a new current volume at this manager
    /// </summary>
    /// <param name="volume">New volume level</param>
    public void SetCurrentVolume(float volume) {
        currentVolume = volume;
        audioSource.volume = volume;
    }

    /// <summary>
    /// Plays a sound at a certain position in world space
    /// </summary>
    /// <param name="audio">The clip to be played</param>
    /// <param name="position">The position where the clip will be played</param>
    public void PlaySoundAtPoint(AudioClip audio, Vector3 position) {
        AudioSource.PlayClipAtPoint(audio, position, currentVolume);
    }

    /// <summary>
    /// Plays a 2D sound
    /// </summary>
    /// <param name="clip">The clip to be played</param>
    public void PlaySound2D(AudioClip clip) {
        audioSource.PlayOneShot(clip, currentVolume);
    }
    #endregion
}
