using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkeletonWithCoffinCoffinScript : MonoBehaviour
{
    public GameObject mySprite;
    public float spriteRotateSpeed;

    bool alreadyHittedPlayerOnThisFrame = false;

    float damageDealt = 30f;

    public EnemyStatus.damageTypes damageTypeDealt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, spriteRotateSpeed * 360f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D targetHit)
    {
        if (targetHit.gameObject == MovementScript.player || targetHit.gameObject.GetComponent<PlayerSensor>() == true)
        {
            //trava multiplos acertos
            if (alreadyHittedPlayerOnThisFrame == false)
            {
                //da dano
                MovementScript.player.GetComponent<PlayerHealth>().TakeDamage(Mathf.RoundToInt(damageDealt), damageTypeDealt);
                GameObject boss = BossZombieSkeletonGruntScript.bossSkeletonGrunt;

                //impulsiona na direção oposta
                //calcula direção
                float impulseDirection = MovementScript.player.transform.position.x - boss.gameObject.transform.position.x;
                Vector2 dir = Vector2.zero;
                dir.x = impulseDirection;
                //impulsiona
                MovementScript.player.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 400f);

                Invoke("reload", 0.7f);
                alreadyHittedPlayerOnThisFrame = true;
            }
        }
    }

    void reload()
    {
        alreadyHittedPlayerOnThisFrame = false;
    }
}
