using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ShieldedSkeletonDamageCast : MonoBehaviour
{
    [HideInInspector]
    public GameObject master;
    [HideInInspector]
    public int damage;
    [HideInInspector]
    public Effect damageEffect = new Effect();

    bool teste = true;
    private void Awake()
    {
        teste = true;
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if (true)
        {
            teste = false;
            if (c.gameObject.GetComponent<EnemyStatus>() && c.gameObject != master)
            {
                c.gameObject.GetComponent<EnemyStatus>().CastDamage(damage, EnemyStatus.damageTypes.physical);
                //
                damageEffect.Invoke(c.gameObject);
            }
            if (c.gameObject.GetComponent<PlayerHealth>() && c.gameObject != master)
            {
                bool escudoApontadoInimigo = c.GetComponent<Combat_Player>().shield&&
                ((c.transform.position.x<=master.transform.position.x&&c.transform.right.x==1)||
                (master.transform.position.x<=c.transform.position.x&&c.transform.right.x==-1));
                //
                c.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage, EnemyStatus.damageTypes.physical, master.transform.position);
                if(escudoApontadoInimigo){ MovementScript.player.GetComponent<SistemaItens>().itensMao[1].item.efeito.Invoke(master);}
                else{
                    damageEffect.Invoke(c.gameObject);
                }
            }
        }
        //
        //Destroy(this.gameObject);
    }
}
