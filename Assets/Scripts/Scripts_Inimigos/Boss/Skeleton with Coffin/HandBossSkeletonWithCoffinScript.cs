using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBossSkeletonWithCoffinScript : MonoBehaviour
{
    public bool alreadyHittedPlayerOnThisFrame = false;
    BossZombieSkeletonGruntScript boss;
    float damageDealt;
    float playerImpulseForce;
    public EnemyStatus.damageTypes damageTypeDealt;
    

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.layer = 0;

        alreadyHittedPlayerOnThisFrame = false;
        boss = BossZombieSkeletonGruntScript.bossSkeletonGrunt.GetComponent<BossZombieSkeletonGruntScript>();
        damageDealt = boss.meleeDamage;
        playerImpulseForce = boss.meleeImpulseForce;
    }

    void OnEnable()
    {
        alreadyHittedPlayerOnThisFrame = false;
    }

    void OnTriggerEnter2D(Collider2D targetHit)
    {
        if(targetHit.gameObject == MovementScript.player || targetHit.gameObject.GetComponent<PlayerSensor>() == true)
        {
            //trava multiplos acertos
            if(alreadyHittedPlayerOnThisFrame == false)
            {
                //da dano
                MovementScript.player.GetComponent<PlayerHealth>().TakeDamage(Mathf.RoundToInt(damageDealt), damageTypeDealt);

                //impulsiona na direção oposta
                //calcula direção
                float impulseDirection = MovementScript.player.transform.position.x - boss.gameObject.transform.position.x;
                Vector2 dir = Vector2.zero;
                dir.x = impulseDirection;
                //impulsiona
                MovementScript.player.GetComponent<Rigidbody2D>().AddForce(dir.normalized * playerImpulseForce);


                alreadyHittedPlayerOnThisFrame = true;
            }
        }

    }


}
