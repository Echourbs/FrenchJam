using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{

    [Header("se vai executar na perda de contato, deixar falso caso seja na entrada")]
    [SerializeField]
    bool byContactExit;

    bool touchedPlayer = false;

    [Header("deve destruir quando executar")]
    [SerializeField]
    bool keepAliveAfter;

    [Header("lista de objetos que sao ativados")]
    public GameObject[] objectsToActivave;


    void Start()
    {
        touchedPlayer = false;
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == MovementScript.player)
        {
            touchedPlayer = true;
        }

        if(byContactExit == false)
        {
            for (int i = 0; i < objectsToActivave.Length; i++)
            {
                objectsToActivave[i].gameObject.SetActive(true);

                if(keepAliveAfter == false)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if(touchedPlayer == true)
        {
            if (byContactExit == true)
            {
                for (int i = 0; i < objectsToActivave.Length; i++)
                {
                    objectsToActivave[i].gameObject.SetActive(true);

                    if (keepAliveAfter == false)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }



}
