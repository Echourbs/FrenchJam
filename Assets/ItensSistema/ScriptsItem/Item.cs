using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "RPG Generator/Player/Create Item")]
public class Item : ScriptableObject
{
    [Header("Info Basico")]
    public string nome;
    //
    [TextArea(3, 10)]
    public string descricao;
    //
	public Sprite imagem;
	//
    [Header("Custo")]
    public int custo;
    public enum tipoMoeda{
    	enxofre,
    	penaDeFenix
    }
    public tipoMoeda moeda;
    //
    public enum tiposRaridade{
        comum, 
        incomum, 
        raro,
        epico,
        lendario
    }
    [Header("Raridade")]
    public tiposRaridade raridade;
    //
    [Header("Efeito")]
    public Effect efeito;
    //
    public enum tiposDeItem{
        consumivel,
        chave,
        arma,
        outro
    }
    [Header("Tipo de Item")]
    public tiposDeItem tipoItem;
    //
    //________________________________
    //Consumivel

    //________________________________
    //Item Chave
    [HideInInspector]
    public string nomeChave;
    //________________________________
    //Arma
    //Arco e Arma
    [HideInInspector]
    public float physicDamage;
    [HideInInspector]
    public float thunderDamage;
    [HideInInspector]
    public float voidDamage;
    [HideInInspector]
    public float fireDamage;
    [HideInInspector]
    public float iceDamage;
    //Escudo e Arma
    [HideInInspector]
    public float physicDefense;
    [HideInInspector]
    public float thunderDefense;
    [HideInInspector]
    public float voidDefense;
    [HideInInspector]
    public float fireDefense;
    [HideInInspector]
    public float iceDefense;
    //Arco e Arma
    [HideInInspector]
    public float velocidade;
    //
    public enum tiposDeArma{
        espada,
        adaga,
        martelo,
        bastao,
        machado,
        escudo,
        arco,
        gancho
    }
    [HideInInspector]
    public tiposDeArma tipoArma;
    //________________________________
    //Outro
    [HideInInspector]
    public string itemID;

    public int tipoArmaToInteger(){
        int valorItem = 0;
        switch(tipoArma){
            case tiposDeArma.espada:
                valorItem = 0;
                break;
            case tiposDeArma.escudo:
                valorItem = 1;
                break;
            case tiposDeArma.arco:
                valorItem = 2;
                break;
            case tiposDeArma.gancho:
                valorItem = 3;
                break;
            case tiposDeArma.adaga:
                valorItem = 4;
                break;
            case tiposDeArma.martelo:
                valorItem = 5;
                break;
            case tiposDeArma.bastao:
                valorItem = 6;
                break;
            case tiposDeArma.machado:
                valorItem = 7;
                break;
            default:
                valorItem = -1;
                break;
        }
        //
        return valorItem;
    }

    public string tipoArmaToString(){
        string valorItem = "";
        switch(tipoArma){
            case tiposDeArma.espada:
                valorItem = "espada";
                break;
            case tiposDeArma.escudo:
                valorItem = "escudo";
                break;
            case tiposDeArma.arco:
                valorItem = "arco";
                break;
            case tiposDeArma.gancho:
                valorItem = "gancho";
                break;
            case tiposDeArma.adaga:
                valorItem = "adaga";
                break;
            case tiposDeArma.martelo:
                valorItem = "martelo";
                break;
            case tiposDeArma.bastao:
                valorItem = "bastao";
                break;
            case tiposDeArma.machado:
                valorItem = "machado";
                break;
            default:
                valorItem = "";
                break;
        }
        //
        return valorItem;
    }

}

[Serializable]
public class Effect : UnityEvent<GameObject>{
    //
	public void Call(GameObject obj){
        this.Invoke(obj);
    }
    //
}