using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Improviso/Efeitos Ferramentas")]
public class EfeitosFerramentas : ScriptableObject
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
}
