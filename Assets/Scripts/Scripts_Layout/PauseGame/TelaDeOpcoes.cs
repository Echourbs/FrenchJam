using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelaDeOpcoes : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //Chama a tela de opções
    public void Opcoes()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("OptionsScene");
    }

}
