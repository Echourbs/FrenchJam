using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioLoder_Menu : MonoBehaviour
{
    void Start()
    {

        AudioPref.Load();

        print(AudioPref.mudo);
        print(AudioPref.musica);
        print(AudioPref.SFX);
        print(AudioPref.Ambiente);
    }
}
