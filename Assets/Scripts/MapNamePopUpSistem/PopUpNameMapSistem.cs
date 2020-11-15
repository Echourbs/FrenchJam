using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpNameMapSistem : MonoBehaviour
{
    
    public string startMapName = "nome inicial do mapa";

    [Header("tempo de display")]
    public float timeOnScreen = 4.5f;
    
    //public float FadeSpeedMultiply;


    //este objeto é colocado em estatico para que possa ser utilizado o codigo;
    public static GameObject popUpNameBackAndControler;

    //componentes de texto, para trocar a cor/alpha e texto
    Text textEnd;
    Text textFront;

    //valor de controle do alpha
    float desirableAlpha = 0;

    //valor atual do alpha
    float mainAlpha;
    
    
    //indica se o valor esta ficando transparente ou nao
    bool risingAlpha = false;

    void Awake()
    {
        //fala que o primeiro/ texto ao fundo texto é o objeto que tem esse codigo.
        popUpNameBackAndControler = this.gameObject;
        //pega o componente text deste objeto
        textEnd = gameObject.GetComponent<Text>();


        //pega o texto da frente, que é o primeiro filho do texto de fundo
        textFront = gameObject.transform.GetChild(0).GetComponent<Text>();


        //DesirableAlpha = 0;
        mainAlpha = textEnd.color.a;
    }


    void Start()
    {
        //coloca a cor alpha e a cor dos textos para 0;
        mainAlpha = 0;
        textEnd.color = new Color(textEnd.color.r, textEnd.color.g, textEnd.color.b, mainAlpha);
        textFront.color = new Color(textFront.color.r, textFront.color.g, textFront.color.b, mainAlpha);
        
        //chama funçao de display e fade de tela
        NewAreaNameCaller(startMapName);
    }


    //funçao de display do novo texto e fade de tela
    public void NewAreaNameCaller(string areaName)
    {
        //verifica se a area atual do player é nova
        if (textEnd.text != areaName)
        {

            //reseta o cor alpha
            textEnd.color = new Color(textEnd.color.r, textEnd.color.g, textEnd.color.b, 0);
            textFront.color = new Color(textFront.color.r, textFront.color.g, textFront.color.b, 0);
            mainAlpha = 0;


            //troca os textos
            textEnd.text = areaName;
            textFront.text = areaName;

            //indica novo valor desejavel de cor alpha
            desirableAlpha = 1;

            //indicia se é fade in ou fade out
            risingAlpha = true;

            //chama void para atualizar os valores
            PseudoUpdate();
        }

        
    }  



    //void que funciona como update, mas quando acaba o if para de ser chamado (a fins de economizar processamento)
    void PseudoUpdate()
    {
        
        //executa a função do alpha e finaliza o void quando acabar
        if (mainAlpha != desirableAlpha)
        {
            //print(Time.time);
            //troca novo valor de cor alpha
            mainAlpha = Mathf.MoveTowards(mainAlpha, desirableAlpha, Time.deltaTime);
            textEnd.color = new Color(textEnd.color.r, textEnd.color.g, textEnd.color.b, mainAlpha);
            textFront.color = new Color(textFront.color.r, textFront.color.g, textFront.color.b, mainAlpha);

            //quando o valor da cor alpha chega no valor maximo, adiciona o valor do tempo e o reduz por time delta time, assim o display fica na tela pelo tempo preciso da variavel de time on screen
            if (mainAlpha == 1 && risingAlpha == true)
            {
                mainAlpha += timeOnScreen;
                desirableAlpha = 0;
                risingAlpha = false;
            }


            //re chama a função ate o display acabar
            Invoke("PseudoUpdate", Time.deltaTime);
        }
        
    }

    



}
