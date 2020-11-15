using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsScript : MonoBehaviour
{
    [Header("Canvas Video Buttons")]
    public GameObject allButtons;
    public GameObject btnBack;
    public GameObject btnControl;
    public GameObject btnVideo;
    public GameObject btnAudio;

    [Header("Pause Buttons")]
    public GameObject continueButton;

    [Header("Pause Canvas")]
    public GameObject canvasControl;
    public GameObject canvasVideo;
    public GameObject canvasAudio;
    

    [SerializeField]
    private int wichMenu;

    private void Start()
    {
        /*if (wichMenu == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(btnVideo);
        }     */

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        if (wichMenu == 0 || wichMenu == 1)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(btnVideo);
        }
        else if (wichMenu == 2)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(continueButton);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && wichMenu == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
        }
    }

    public void EnterAudio()
    {
        if (canvasVideo.active)
        {
            allButtons.GetComponent<Animator>().Play("VideoToAudio");
        }
        else if (canvasControl.active)
        {
            allButtons.GetComponent<Animator>().Play("ControlsToAudio");
        }
    }

    public void EnterVideo()
    {
        if (canvasAudio.active)
        {
            allButtons.GetComponent<Animator>().Play("AudioToVideo");
        }
        else if (canvasControl.active)
        {
            allButtons.GetComponent<Animator>().Play("ControlsToVideo");
        }
    }

    public void EnterControl()
    {
        if (canvasAudio.active)
        {
            allButtons.GetComponent<Animator>().Play("AudioToControls");
        }
        else if (canvasVideo.active)
        {
            allButtons.GetComponent<Animator>().Play("VideoToControls");
        }
    }

    /*public void PauseInit()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton);
    }*/

    //Chama a tela de Options
    public void ChamaOpcoes()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OptionsScene");
    }

}
