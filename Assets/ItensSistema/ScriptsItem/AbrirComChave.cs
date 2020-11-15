using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbrirComChave : MonoBehaviour
{
	public string nomeChave;
	[HideInInspector]
	public bool aberto = false;
	//
	private SistemaItens _sistemaItens;

    // Start is called before the first frame update
    public void Start()
    {
        _sistemaItens = MovementScript.player.GetComponent<SistemaItens>();
    }

    public void Abrir(){
    	for(int i=0; i<_sistemaItens.itensBag.Length; i++){
    		if(_sistemaItens.itensBag[i].item!=null&&
    			_sistemaItens.itensBag[i].item.tipoItem==Item.tiposDeItem.chave&&
    			_sistemaItens.itensBag[i].item.nomeChave==nomeChave){
    			aberto = true;
    			_sistemaItens.itensBag[i].item = null;
    			_sistemaItens.itensBag[i].quantos = 0;
    			break;
    		};
    	}
    }
}
