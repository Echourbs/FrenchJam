using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmasTela : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    //chama a tela de armas
    public void Armas()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ArmasScene");
    }

}
