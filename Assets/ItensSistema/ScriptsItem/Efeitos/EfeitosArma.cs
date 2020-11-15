using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Improviso/Efeitos Arma")]
public class EfeitosArma : ScriptableObject
{
    public void Empurrar(GameObject obj){
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
    	if(rb!=null){ rb.AddForce(300f*MovementScript.player.transform.right);}
    }

    public void JogarPraCima(GameObject obj){
    	obj.GetComponent<Rigidbody2D>().AddForce(450f*Vector3.up);
    }

    public void Ignite(GameObject obj){
    	obj.GetComponent<EnemyStatus>().StartCoroutine(IgniteExecucao(obj));
    }

    IEnumerator IgniteExecucao(GameObject obj){
    	EnemyStatus _enemyStatus = obj.GetComponent<EnemyStatus>();
    	for(int i=0; i<5; i++){
    		_enemyStatus.CastDamage(15, EnemyStatus.damageTypes.fire);
    		//
    		yield return new WaitForSeconds(1);
    	}
    }

    public void Enraizar(GameObject obj){
    	obj.GetComponent<EnemyStatus>().StartCoroutine(EnraizarExecucao(obj));
    }

    IEnumerator EnraizarExecucao(GameObject obj){
    	Vector3 pos = obj.transform.position;
    	for(int i=0; i<3/Time.deltaTime;i++){
    		obj.transform.position = pos;
    		//
    		yield return new WaitForSeconds(Time.deltaTime);
    	}

    }

    public void Congelar(GameObject obj){
    	obj.GetComponent<EnemyStatus>().StartCoroutine(CongelarExecucao(obj));
    }

    IEnumerator CongelarExecucao(GameObject obj){
    	MonoBehaviour[] componentes = obj.GetComponents<MonoBehaviour>();
    	//
    	for(int i=0; i<componentes.Length; i++){
    		componentes[i].enabled = false;
    	}
    	obj.GetComponent<Collider2D>().enabled = true;
    	obj.GetComponent<EnemyStatus>().enabled = true;
    	obj.GetComponent<SpriteRenderer>().enabled = true;
    	//
    	yield return new WaitForSeconds(5f);
    	//
    	for(int i=0; i<componentes.Length; i++){
    		componentes[i].enabled = true;
    	}
    }

    public void Diminuir(GameObject obj){
    	obj.GetComponent<EnemyStatus>().StartCoroutine(DiminuirExecucao(obj));
    }

    IEnumerator DiminuirExecucao(GameObject obj){
    	obj.transform.localScale = obj.transform.localScale*0.5f;
    	yield return new WaitForSeconds(5f);
    	obj.transform.localScale = obj.transform.localScale/0.5f;
    }
}
