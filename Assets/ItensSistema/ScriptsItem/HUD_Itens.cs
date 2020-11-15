using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Itens : MonoBehaviour
{

	private SistemaItens _sistemaItens;
	//
	public Image[] itensMao = new Image[4];
	public Text[] itensMaoText = new Text[2];

    // Start is called before the first frame update
    void Start()
    {
        _sistemaItens = MovementScript.player.GetComponent<SistemaItens>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i=0; i<itensMao.Length; i++){
        	if(_sistemaItens.itensMao[i].item!=null){
        		itensMao[i].sprite = _sistemaItens.itensMao[i].item.imagem;
        		itensMao[i].color = Color.white;
        		//
                if(i>1){
                    itensMaoText[i-2].text = (_sistemaItens.itensMao[i].quantos+1).ToString();
                }
        	}
        	else{
        		itensMao[i].sprite = null;
        		itensMao[i].color = Color.clear;
        		//
                if(i>1){
        		  itensMaoText[i-2].text = "";
        	   }
            }
        }
    }
}
