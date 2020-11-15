using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditosScript : MonoBehaviour
{
    //Chama a tela de Créditos
    public void ChamaCreditos()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CreditosScene");
    }
}
