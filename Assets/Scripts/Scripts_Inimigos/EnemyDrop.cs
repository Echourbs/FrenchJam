using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
	[Serializable]
	public class DropItem{
		public Item item;
		[Range(0f, 100f)]
		public float porcentagem;
		//
		public int quantosPodeDropar;
	}

	[Header("Básico")]
	public GameObject enxofrePrefab;
	public GameObject itemQuedaPrefab;

	[Header("Enxofre")]
	public int numeroMoedasEnxofreMin;
	public int numeroMoedasEnxofreMax;
	//

	[Header("Itens")]
	public DropItem[] itemDrop;

    public void Drop()
    {
    	//Enxofre
    	int numeroMoedasEnxofre = UnityEngine.Random.Range(numeroMoedasEnxofreMin, numeroMoedasEnxofreMax);
    	//
    	for(int i=0; i<numeroMoedasEnxofre; i++){
    		CriarObjetoComForca(enxofrePrefab, transform.position, 10f);
    	}
    	//Itens
    	foreach(DropItem itemDropCopy in itemDrop){
    		float numeroAleatorio = UnityEngine.Random.Range(0f, 100f);
    		if(numeroAleatorio<=itemDropCopy.porcentagem){
    			QuedaItem objGerado_quedaItem = CriarObjetoComForca(itemQuedaPrefab, transform.position, 10f).GetComponent<QuedaItem>();
    			objGerado_quedaItem.item = itemDropCopy.item;
    			objGerado_quedaItem.quantos = UnityEngine.Random.Range(0, itemDropCopy.quantosPodeDropar-1);
    		}
    	}
    }

    public void DropEspecifico( int numeroMoedasEnxofreMin, int numeroMoedasEnxofreMax, DropItem[] itemDrop){
    	//Enxofre
    	int numeroMoedasEnxofre = UnityEngine.Random.Range(numeroMoedasEnxofreMin, numeroMoedasEnxofreMax);
    	//
    	for(int i=0; i<numeroMoedasEnxofre; i++){
    		CriarObjetoComForca(enxofrePrefab, transform.position, 10f);
    	}
    	//Itens
    	foreach(DropItem itemDropCopy in itemDrop){
    		float numeroAleatorio = UnityEngine.Random.Range(0f, 100f);
    		if(numeroAleatorio<=itemDropCopy.porcentagem){
    			QuedaItem objGerado_quedaItem = CriarObjetoComForca(itemQuedaPrefab, transform.position, 10f).GetComponent<QuedaItem>();
    			objGerado_quedaItem.item = itemDropCopy.item;
    			objGerado_quedaItem.quantos = UnityEngine.Random.Range(0, itemDropCopy.quantosPodeDropar-1);
    		}
    	}
    }

    GameObject CriarObjetoComForca(GameObject objeto, Vector3 posCentral, float forca){
    	GameObject objGerado = Instantiate(objeto, posCentral + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0f), Quaternion.Euler(0f, 0f, 0f));
    	//
    	Vector3 direcao = objeto.transform.position-transform.position;
        direcao = new Vector3(Mathf.Clamp(direcao.x, -1f, 1f), Mathf.Clamp(direcao.y, -1f, 1f), 0f);
        objGerado.GetComponent<Rigidbody2D>().velocity = direcao*forca;
    	//
    	return objGerado;
    }
}
