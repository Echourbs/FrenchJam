using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaItens : MonoBehaviour
{

	[Serializable]
	public class Slot{
		public Item item;
		public int quantos = 0;

		public Slot(Item item, int quantos){
			this.item = item;
			this.quantos = quantos;
		}
	}

	[Header("Para Funcionar")]
	public GameObject quedaItemPrefab;
	//
	[Header("Info Slots")]
	public Slot[] itensMao = new Slot[4];
	public Slot[] itensBag = new Slot[32];
	//
	[Header("Excessão - que não podem ser Dropados")]
	public Item[] itensExcessao;
	//
	private bool clickConsumivel1;
	private bool clickConsumivel2;

	void Update(){
		if(Input.GetAxis("Consumivel1")<0&&!clickConsumivel1&&!Menu_Item.ativo&&!PauseGame.GameIsPaused){
			clickConsumivel1 = true;
			UsarConsumivel1();
		}
		else if(!(Input.GetAxis("Consumivel1")<0)&&clickConsumivel1&&!Menu_Item.ativo&&!PauseGame.GameIsPaused){
			clickConsumivel1 = false;
		}
		//
		if(Input.GetAxis("Consumivel2")>0&&!clickConsumivel2&&!Menu_Item.ativo&&!PauseGame.GameIsPaused){
			clickConsumivel2 = true;
			UsarConsumivel2();
		}
		else if(!(Input.GetAxis("Consumivel2")>0)&&clickConsumivel2&&!Menu_Item.ativo&&!PauseGame.GameIsPaused){
			clickConsumivel2 = false;
		}
	}

	public void UsarConsumivel1(){
		if(itensMao[2].item!=null){
			itensMao[2].item.efeito.Invoke(MovementScript.player);
			itensMao[2].quantos--;
			if(itensMao[2].quantos<0){
				itensMao[2].item = null;
			}
		}
	}

	public void UsarConsumivel2(){
		if(itensMao[3].item!=null){
			itensMao[3].item.efeito.Invoke(MovementScript.player);
			itensMao[3].quantos--;
			if(itensMao[3].quantos<0){
				itensMao[3].item = null;
			}
		}
	}

	//Trocar Item

	public void TrocarItemMao(int bagIndex1, int maoIndex2){
		Slot itemIndex1 = itensBag[bagIndex1];
		Slot itemIndex2 = itensMao[maoIndex2];
		//
		int valorItem = 0;
		bool entreValorNulo = false;
		if(itemIndex1.item!=null){
			switch(itemIndex1.item.tipoItem){
				case Item.tiposDeItem.consumivel:
					valorItem = 0;
					break;
				case Item.tiposDeItem.chave:
					valorItem = 1;
					break;
				case Item.tiposDeItem.arma:
					valorItem = 2;
					break;
				case Item.tiposDeItem.outro:
					valorItem = 3;
					break;
				default:
					break;
			}
		}
		else{
			entreValorNulo = true;
		}
		//
		if((maoIndex2==0&&valorItem==2)||
			(maoIndex2==1&&valorItem==2)||
			(maoIndex2==2&&valorItem==0)||
			(maoIndex2==3&&valorItem==0)||
			entreValorNulo
			){
			itensBag[bagIndex1] = itemIndex2;
			itensMao[maoIndex2] = itemIndex1;
		}
	}

	public void TrocarPosicaoItemBag(int index1, int index2){
		Slot itemIndex1 = itensBag[index1];
		Slot itemIndex2 = itensBag[index2];
		//
		itensBag[index1] = itemIndex2;
		itensBag[index2] = itemIndex1;
	}

	public void TrocarItensMao(int index1, int index2){
		Slot itemIndex1 = itensMao[index1];
		Slot itemIndex2 = itensMao[index2];
		//
		if((index1==2&&index2==3)||
			(index1==3&&index2==2)||
			(index1==0&&index2==1)||
			(index1==1&&index2==0)
			){

			itensMao[index1] = itemIndex2;
			itensMao[index2] = itemIndex1;
		}
	}

	//______________


	public bool AdicionarNovoItem(Item itemAdicionar, int quantos){
		bool conseguiuAdicionar = false;
		int tipoItem = 0;
		//
		switch(itemAdicionar.tipoItem){
			case Item.tiposDeItem.consumivel:
				tipoItem = 0;
				break;
			case Item.tiposDeItem.chave:
				tipoItem = 1;
				break;
			case Item.tiposDeItem.arma:
				tipoItem = 2;
				break;
			case Item.tiposDeItem.outro:
				tipoItem = 3;
				break;
			default:
				break;
		}
		//Adicionar em um Slot com esse Item 
		if(!conseguiuAdicionar&&(tipoItem==0||tipoItem==3)){
		for(int i=0; i<itensMao.Length; i++){
			if((itensMao[i].item==itemAdicionar)&&
				((i==0&&tipoItem==2)||
				(i==1&&tipoItem==2)||
				(i==2&&tipoItem==0)||
				(i==3&&tipoItem==0))
				){
				//
				conseguiuAdicionar = true;
				itensMao[i].quantos += 1+quantos;
				break;
			}
		}}
		//
		if(!conseguiuAdicionar&&(tipoItem==0||tipoItem==3)){
		for(int i=0; i<itensBag.Length; i++){
			if(itensBag[i].item==itemAdicionar){
				conseguiuAdicionar = true;
				itensBag[i].quantos += 1+quantos;
				break;
			}
		}}
		//Adicionar em um Slot Vazio
		if(!conseguiuAdicionar){
		for(int i=0; i<itensMao.Length; i++){
			if((itensMao[i].item==null)&&
				((i==0&&tipoItem==2)||
				(i==1&&tipoItem==2)||
				(i==2&&tipoItem==0)||
				(i==3&&tipoItem==0))
				){
				//
				conseguiuAdicionar = true;
				itensMao[i] = new Slot(itemAdicionar, quantos);
				break;
			}
		}}
		//
		if(!conseguiuAdicionar){
		for(int i=0; i<itensBag.Length; i++){
			if(itensBag[i].item==null){
				conseguiuAdicionar = true;
				itensBag[i] = new Slot(itemAdicionar, quantos);
				break;
			}
		}}
		//
		return conseguiuAdicionar;
	}

	public void DroparItem(int index, bool mao){
		if(mao&&itensMao[index].item!=null){
			bool souExcessao = false;
			for(int i=0; i<itensExcessao.Length; i++){
				if(itensMao[index].item == itensExcessao[i]){ souExcessao = true; break;}
			}
			if(!souExcessao){
				GameObject quedaItemGerado = Instantiate(quedaItemPrefab, MovementScript.player.transform.position, Quaternion.Euler(0f, 0f, 0f));
				quedaItemGerado.GetComponent<QuedaItem>().item = itensMao[index].item;
				quedaItemGerado.GetComponent<QuedaItem>().quantos = itensMao[index].quantos;
				itensMao[index].item = null;
				itensMao[index].quantos = 0;
			}	
		}
		else if(!mao&&itensBag[index].item!=null){
			bool souExcessao = false;
			for(int i=0; i<itensExcessao.Length; i++){
				if(itensBag[index].item == itensExcessao[i]){ souExcessao = true; break;}
			}
			if(!souExcessao){
				GameObject quedaItemGerado = Instantiate(quedaItemPrefab, MovementScript.player.transform.position, Quaternion.Euler(0f, 0f, 0f));
				quedaItemGerado.GetComponent<QuedaItem>().item = itensBag[index].item;
				quedaItemGerado.GetComponent<QuedaItem>().quantos = itensBag[index].quantos;
				itensBag[index].item = null;
				itensBag[index].quantos = 0;
			}	
		}
	}
}
