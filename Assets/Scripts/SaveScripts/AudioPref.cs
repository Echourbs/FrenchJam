using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPref : MonoBehaviour
{
    public static bool mudo;
    public static float musica, SFX, Ambiente;

    public static void Load()
    {
        try
        {
            musica = PlayerPrefs.GetFloat("musica");
        }
        catch (System.Exception)
        {
            musica = 1;
            throw;
        }
        try
        {
            SFX = PlayerPrefs.GetFloat("SFX");
        }
        catch (System.Exception)
        {
            SFX = 1;
            throw;
        }
        try
        {
            Ambiente = PlayerPrefs.GetFloat("Ambiente");
        }
        catch (System.Exception)
        {
            Ambiente = 1;
            throw;
        }
        try
        {
            if (PlayerPrefs.GetInt("mudo") == 0)
                mudo = true;
            else
                mudo = false;
        }
        catch (System.Exception)
        {
            mudo = false;
            throw;
        }
        
        
    }

    public static void Save()
    {
        if (mudo)
        {
            PlayerPrefs.SetInt("Mudo", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Mudo", 1);
        }

        PlayerPrefs.SetFloat("musica", musica);
        PlayerPrefs.SetFloat("SFX", SFX);
        PlayerPrefs.SetFloat("Ambiente", Ambiente);
    }
}
