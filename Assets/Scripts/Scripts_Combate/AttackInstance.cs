using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInstance : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<string, int> damage = new Dictionary<string, int>{
        {"physic", 0},
        {"thunder", 0},
        {"void", 0},
        {"fire", 0},
        {"ice", 0}
    };
    [HideInInspector]
    public GameObject master;
    [HideInInspector]
    public Effect damageEffect = new Effect();

    void Start()
    {
        Destroy(this.gameObject, 0.05f);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<EnemyStatus>() && c.gameObject != master)
        {
            c.gameObject.GetComponent<EnemyStatus>().CastMultipleTypeDamage(damage);
            //
            damageEffect.Invoke(c.gameObject);
            //
            CameraController.Hit();
        }
        if(c.gameObject.GetComponent<PlayerHealth>() && c.gameObject != master)
        {
            bool escudoApontadoInimigo = c.GetComponent<Combat_Player>().shield&&
                ((c.transform.position.x<=master.transform.position.x&&c.transform.right.x==1)||
                (master.transform.position.x<=c.transform.position.x&&c.transform.right.x==-1));
            //
            c.gameObject.GetComponent<PlayerHealth>().TakeMultipleTypeDamage(damage, master.transform.position);
            if(escudoApontadoInimigo){ 
                MovementScript.player.GetComponent<SistemaItens>().itensMao[1].item.efeito.Invoke(master);
            }
            else{
                damageEffect.Invoke(c.gameObject);
            }
        }
        //
        Destroy(this.gameObject);
    }

    public void AtribuirValores(Dictionary<string, int> damage, GameObject master){
        this.damage = damage;
        this.master = master;
    }


    //_______________________________________________________________________________________________________________________________________
    //Dicionarios

    static public Dictionary<string, int> CreateDamageDictionary(int physic = 0, int thunder = 0, int void_dark = 0, int fire = 0, int ice = 0){
        return new Dictionary<string, int>{
            {"physic", physic},
            {"thunder", thunder},
            {"void", void_dark},
            {"fire", fire},
            {"ice", ice}
        };
    }

    static public Dictionary<string, int> CreateDamageDictionaryType(string damageType, int valueDamage){
        Dictionary<string, int> myDamageDictionary = new Dictionary<string, int>{};
        //
        if(damageType=="physic"){
            myDamageDictionary.Add("physic", valueDamage);
            myDamageDictionary.Add("thunder", 0);
            myDamageDictionary.Add("void", 0);
            myDamageDictionary.Add("fire", 0);
            myDamageDictionary.Add("ice", 0);
        }
        else if(damageType=="thunder"){
            myDamageDictionary.Add("physic", 0);
            myDamageDictionary.Add("thunder", valueDamage);
            myDamageDictionary.Add("void", 0);
            myDamageDictionary.Add("fire", 0);
            myDamageDictionary.Add("ice", 0);
        }
        else if(damageType=="void"){
            myDamageDictionary.Add("physic", 0);
            myDamageDictionary.Add("thunder", 0);
            myDamageDictionary.Add("void", valueDamage);
            myDamageDictionary.Add("fire", 0);
            myDamageDictionary.Add("ice", 0);
        }
        else if(damageType=="fire"){
            myDamageDictionary.Add("physic", 0);
            myDamageDictionary.Add("thunder", 0);
            myDamageDictionary.Add("void", 0);
            myDamageDictionary.Add("fire", valueDamage);
            myDamageDictionary.Add("ice", 0);
        }
        else if(damageType=="ice"){
            myDamageDictionary.Add("physic", 0);
            myDamageDictionary.Add("thunder", 0);
            myDamageDictionary.Add("void", 0);
            myDamageDictionary.Add("fire", 0);
            myDamageDictionary.Add("ice", valueDamage);
        }
        else{
            Debug.Log("Erro: nao foi possivel encontrar o tipo de dano // Script: AttackInstance // Funcao: CreateDamageDictionaryType");
            //
            myDamageDictionary.Add("physic", 0);
            myDamageDictionary.Add("thunder", 0);
            myDamageDictionary.Add("void", 0);
            myDamageDictionary.Add("fire", 0);
            myDamageDictionary.Add("ice", 0);
        }

        return myDamageDictionary;
    }

    static public Dictionary<string, int> CreateWeaponDamageDictionary(Item arma){
        return CreateDamageDictionary((int)arma.physicDamage, (int)arma.thunderDamage, (int)arma.voidDamage, (int)arma.fireDamage, (int)arma.iceDamage);
    }

}
