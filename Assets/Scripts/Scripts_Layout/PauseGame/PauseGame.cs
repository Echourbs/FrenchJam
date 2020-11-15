using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PauseGame : MonoBehaviour
{
    public static bool GameIsPaused;
    bool inOptions = false;

    public GameObject pauseMenuUI;
    public GameObject pauseMenuButtons;
    public GameObject pauseOptions;
    public GameObject itensCanvas;
    public GameObject cursor;
    public GameObject continueButton;
    public GameObject optionsButton;


    void Update()
    {
        inOptions = pauseOptions.active;
        print("InOptions: " + inOptions);

        if(Input.GetButtonDown("Cancel") && GameIsPaused && !inOptions)
        {
            Resume();
        }

        if (Input.GetButtonDown("Pause") && !itensCanvas.active)
        {
            if (GameIsPaused && !inOptions)
            {
                Resume();
            }
            else if(!GameIsPaused && !inOptions)
            {
                Pause();
            }
        }
        if (Input.GetButtonDown("Cancel") && inOptions)
        {
            pauseOptions.GetComponent<OptionsScript>().EnterVideo();
            pauseOptions.SetActive(false);
            pauseMenuButtons.SetActive(true);
            GoBackToMenu();
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        //CursorInput.ativo = false;
        GameIsPaused = false;
    }

    public void Pause()
    {
        EventSystem.current.SetSelectedGameObject(continueButton);
        pauseMenuUI.SetActive(true);      
        //CursorInput.ativo = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GoBackToMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsButton);
    }

    public void VoltarGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        //cursor.GetComponent<CursorInput>().ativar = false;
        GameIsPaused = false;        
    }
}
