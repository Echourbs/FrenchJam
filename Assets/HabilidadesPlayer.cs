using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabilidadesPlayer : MonoBehaviour
{
    public GameObject HabilidadeFogo;

    public GameObject HabilidadeTerra;

    [SerializeField]
    Transform pontoFinal;

    void Start()
    {

    }

    void Update()
    {
        Button();
    }

    public void Button()
    {
        if (Input.GetKey(KeyCode.J))
        {
            HabilidadeA();
        }

        else if (Input.GetKey(KeyCode.K))
        {
            HabilidadeB();
        }
    }


    void HabilidadeA()
    {
        Instantiate(HabilidadeFogo, pontoFinal.position, pontoFinal.rotation);
    }

    void HabilidadeB()
    {
        Instantiate(HabilidadeTerra, pontoFinal.position, pontoFinal.rotation);
    }
}
