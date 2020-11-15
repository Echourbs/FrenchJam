using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyBindManagerr : MonoBehaviour
{

    private IDictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();

    public Text cima, baixo, esquerda, direita, pular, dash, arma1, arma2, arma3, interagir, voltar, golpeFogo, golpeRaio;

    private GameObject chaveCorrespondente;

    private Color32 normal = new Color32(39, 171, 249, 255);
    private Color32 selecionado = new Color32(239, 116, 36, 255);

    

    void Start()
    {
        keys.Add("Cima", KeyCode.W);
        keys.Add("Baixo", KeyCode.A);
        keys.Add("Esquerda", KeyCode.S);
        keys.Add("Direita", KeyCode.D);
        keys.Add("Pular", KeyCode.Space);
        keys.Add("Dash", KeyCode.LeftShift);
        keys.Add("Arma1", KeyCode.E);
        keys.Add("Arma2", KeyCode.None);
        keys.Add("Arma3", KeyCode.None);
        keys.Add("Interagir", KeyCode.None);
        keys.Add("Voltar", KeyCode.None);
        keys.Add("Golpe de Fogo", KeyCode.None);
        keys.Add("Golpe de Raio", KeyCode.None);

        cima.text = keys["Cima"].ToString();
        baixo.text = keys["Baixo"].ToString();
        esquerda.text = keys["Esquerda"].ToString();
        direita.text = keys["Direita"].ToString();
        pular.text = keys["Pular"].ToString();
        dash.text = keys["Dash"].ToString();
        arma1.text = keys["Arma1"].ToString();
        arma2.text = keys["Arma2"].ToString();
        arma3.text = keys["Arma3"].ToString();
        interagir.text = keys["Interagir"].ToString();
        voltar.text = keys["Voltar"].ToString();
        golpeFogo.text = keys["Golpe de Fogo"].ToString();
        golpeRaio.text = keys["Golpe de Raio"].ToString();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(keys["Cima"]))
        {
            Debug.Log("Cima");
        }
        if (Input.GetKeyDown(keys["Baixo"]))
        {
            Debug.Log("Baixo");
        }
        if (Input.GetKeyDown(keys["Esquerda"]))
        {
            Debug.Log("Esquerda");
        }
        if (Input.GetKeyDown(keys["Direita"]))
        {
            Debug.Log("Direita");
        }
        if (Input.GetKeyDown(keys["Pular"]))
        {
            Debug.Log("Pular");
        }
        if (Input.GetKeyDown(keys["Dash"]))
        {
            Debug.Log("Dash");
        }
        if (Input.GetKeyDown(keys["Arma1"]))
        {
            Debug.Log("Arma1");
        }
        if (Input.GetKeyDown(keys["Arma2"]))
        {
            Debug.Log("Arma2");
        }
        if (Input.GetKeyDown(keys["Arma3"]))
        {
            Debug.Log("Arma3");
        }
        if (Input.GetKeyDown(keys["Interagir"]))
        {
            Debug.Log("Interagir");
        }
        if (Input.GetKeyDown(keys["Voltar"]))
        {
            Debug.Log("Voltar");
        }
        if (Input.GetKeyDown(keys["Golpe de Fogo"]))
        {
            Debug.Log("Golpe de Fogo");
        }
        if (Input.GetKeyDown(keys["Golpe de Raio"]))
        {
            Debug.Log("Golpe de Raio");
        }
    }

    void OnGUI()
    {
        if(chaveCorrespondente != null)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                keys[chaveCorrespondente.name] = e.keyCode;
                chaveCorrespondente.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                chaveCorrespondente.GetComponent<Image>().color = normal;
                chaveCorrespondente = null;

            }
        }
    }

    public void TrocadeKey(GameObject clikar)
    {
        if(chaveCorrespondente != null)
        {
            chaveCorrespondente.GetComponent<Image>().color = normal;
        }

        chaveCorrespondente = clikar;
        chaveCorrespondente.GetComponent<Image>().color = selecionado;
    }
}
