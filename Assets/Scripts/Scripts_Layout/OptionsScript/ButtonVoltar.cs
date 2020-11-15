using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonVoltar : MonoBehaviour
{
    public GameObject videoCanvas, soundCanvas, controlCanvas;
    public Button video, sound, control;
    public Toggle checkboxWindowMode, checkboxMute;
    public Scrollbar sb;

    Navigation customNav;

    private void Start()
    {
        customNav = new Navigation();
        customNav.mode = Navigation.Mode.Explicit;
    }

    public void Update()
    {
        if (videoCanvas.active)
        {
            customNav.selectOnUp = checkboxWindowMode;
            customNav.selectOnDown = video;
            GetComponent<Button>().navigation = customNav;
        }
        else if (soundCanvas.active)
        {
            customNav.selectOnUp = checkboxMute;
            customNav.selectOnDown = sound;
            GetComponent<Button>().navigation = customNav;
        }
        else if (controlCanvas.active)
        {
            customNav.selectOnUp = sb;
            customNav.selectOnDown = control;
            GetComponent<Button>().navigation = customNav;
        }       
    }

    public void VoltarMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }
}
