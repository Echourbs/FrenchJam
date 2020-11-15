using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArvoreTela : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //Chama a tela da arvore de habilidade
    public void ChamaArvore()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ArvoreScene");
    }

}
