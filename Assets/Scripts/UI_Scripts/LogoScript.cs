using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LogoScript : MonoBehaviour
{
    VideoPlayer vp;
    public GameObject fundo;

    void Start()
    {
        fundo.SetActive(false);
        vp = GetComponent<VideoPlayer>();
    }

    void Update()
    {
        if (vp.frame == 59)
        {
            fundo.SetActive(true);
            Invoke("CallMenuScene", 0.2f);
        }
        else
        {
            fundo.SetActive(false);
        }
    }

    void CallMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
