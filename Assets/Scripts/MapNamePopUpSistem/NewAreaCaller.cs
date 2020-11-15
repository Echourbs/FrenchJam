using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAreaCaller : MonoBehaviour
{
    [Header("nome das areas")]
    public string areaPositive = "area A";
    public string areaNegative = "area b";

    [Header("define se a transiçao do mapa é da esquerda para a direita (TRUE) ou se é para cima e para baixo (FALSE)")]
    public bool leftRight;

    void OnTriggerExit2D(Collider2D other)
    {
        //verifica se é o player
        if (other.GetComponent<MovementScript>())
        {
            //direção do player com a area de contato
            float Contact = 0;

            //verifica a direção de acordo com a boleana de esquerda-direita ou cima-baixo
            if (leftRight)
            {
                Contact = other.transform.position.x - transform.position.x;
            }
            else
            {
                Contact = other.transform.position.y - transform.position.y;
            }

            //envia o comando de troca de nome e fade atraves da area/direçao do player
            if (Mathf.Sign(Contact) > 0)
            {
                PopUpNameMapSistem.popUpNameBackAndControler.GetComponent<PopUpNameMapSistem>().NewAreaNameCaller(areaPositive);
            }
            else if (Mathf.Sign(Contact) < 0)
            {
                PopUpNameMapSistem.popUpNameBackAndControler.GetComponent<PopUpNameMapSistem>().NewAreaNameCaller(areaNegative);
            }

        }
    }


}
