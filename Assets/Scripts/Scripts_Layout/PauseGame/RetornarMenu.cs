using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetornarMenu : MonoBehaviour
{
    public bool GameIsPaused;

    //chama a tela de menu
    public void VoltarMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");

    }
}
