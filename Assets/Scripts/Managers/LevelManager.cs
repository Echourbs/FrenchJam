using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        if (MusicManager.instance.GetAudioClipPlaying() == null)
        {
            MusicManager.instance.PlayMusicImmediatelyWithFadeIn(audioClip);
        }
        else
        {
            if (!MusicManager.instance.GetAudioClipPlaying().name.Equals(audioClip.name))
            {
                MusicManager.instance.ChangeMusicSmoothly(audioClip);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
