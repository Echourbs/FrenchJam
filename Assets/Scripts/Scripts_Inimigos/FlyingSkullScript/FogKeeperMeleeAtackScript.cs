using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogKeeperMeleeAtackScript : MonoBehaviour
{

    //esse codigo funciona para dar dano melee ao player
    //ele é ativado via animação (ja que a colisao so funciona quando o objeto esta ligado e o objeto so é ligado durante alguns frames da aimação de ataque melee do boss fogkeeper)

    [HideInInspector]
    public float damageCasted;
    [HideInInspector]
    public float impulseForce;

    GameObject player;
    GameObject boss;

    //trava para impedir que o boss acerte o player varias vezes
    bool safeLock;

    void Start()
    {
        //procura o boss e o player
        player = MovementScript.player;
        boss = FogKeeperScript.fogKeeperBoss;
        safeLock = true;
    }

    //como a colisao é ativada com o ligar do objeto, o uso dessa função vai "recarregar" a booleana
    void OnEnable()
    {
        safeLock = true;
    }


    void OnTriggerEnter2D(Collider2D C)
    {
        //pergunta se durante a colisao ele bateu no player
        if (C.gameObject == player && safeLock == true)
        {
            //da dano no jogador
            player.GetComponent<PlayerHealth>().TakeDamage(Mathf.RoundToInt(damageCasted));

            //faz uma verificação para aonde deveria impulsionar o boss
            float direction = Mathf.Sign(player.transform.position.x - boss.GetComponent<FogKeeperScript>().bossPseudoCenter.transform.position.x);

            player.GetComponent<Rigidbody2D>().AddForce(player.transform.right * direction * impulseForce + Vector3.up * 300);
            safeLock = false;
        }
    }



}
