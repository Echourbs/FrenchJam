using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Improviso/Efeitos Consumiveis")]
public class EfeitosConsumiveis : ScriptableObject
{

    public void Cura(float porcentagemDaVida){
    	PlayerHealth ph = MovementScript.player.GetComponent<PlayerHealth>();
    	//
    	int quantoCura = (int)(ph.maxHealth*porcentagemDaVida);
    	ph.currentHealth+=quantoCura;
    	//
    	if(ph.currentHealth>ph.maxHealth){
    		ph.currentHealth=ph.maxHealth;
    	}
    	//
    	ph.healthBar.SetHealth(ph.currentHealth);
    }

    public void Monster(float porcentagem){
    	MovementScript _movementsScript = MovementScript.player.GetComponent<MovementScript>();
    	//
        _movementsScript.numeroMonstersUsado++;
        //
    	_movementsScript.StartCoroutine(MonsterExecucao(porcentagem, _movementsScript));
    }

    IEnumerator MonsterExecucao(float porcentagem, MovementScript _movementsScript){
        if(_movementsScript.numeroMonstersUsado==1){
            _movementsScript.speed *= 2f;
            while(_movementsScript.numeroMonstersUsado>0){
                yield return new WaitForSeconds(5f);
                _movementsScript.numeroMonstersUsado--;
            }
            _movementsScript.speed /= 2f;
        }
        yield return new WaitForSeconds(0f);
    }
}
