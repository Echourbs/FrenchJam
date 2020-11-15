using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMg : MonoBehaviour
{
    private bool ativarEdesativado;

    [SerializeField] GameObject exitPanel;

    void Start()
    {
        ativarEdesativado = false;
        exitPanel.SetActive(ativarEdesativado);
    }

    
    void Update()
    {
        //Por mais que o input esteje desativado, seu objetivo é aparecer o PopUp do menu de pause
        if (Input.GetKeyDown(KeyCode.None))
        {
            ativarEdesativado = !ativarEdesativado;
            exitPanel.SetActive(ativarEdesativado);
        }
    }
    public void ClickSN(int clicou)
    {
        // se cliclou for igual a 1 então é o botão sim e se clicou for igual a 0 então é o botão Não
        if (clicou == 1)
        {
            Application.Quit();
        }
        exitPanel.SetActive(false);
        
    }

}
