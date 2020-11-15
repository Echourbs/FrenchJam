using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderFogDamageScript : MonoBehaviour
{
    GameObject player;
    bool touchingPlayer;

    //pega as variaveis de dano do boss
    float damageOverTime;
    float timeUntilNextHit;

    [HideInInspector]
    public bool delayLoaded = false; 

    // Start is called before the first frame update
    void Start()
    {
        player = MovementScript.player;

        FogKeeperScript boss = FogKeeperScript.fogKeeperBoss.GetComponent<FogKeeperScript>();
        damageOverTime = boss.damageUnderFog;
        timeUntilNextHit = boss.delayTimeForFogUnderSkirtDamage;

        if(boss.fogUnderSkirtControl == null)
        {
            boss.fogUnderSkirtControl = gameObject;
        }

        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        InvokeRepeating("DoDamageOnPlayer", 0.35f, timeUntilNextHit);
    }

    void OnDisable()
    {
        CancelInvoke("DoDamageOnPlayer");
    }

    void delayLoadDone()
    {
        delayLoaded = true;
    }


    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject == player)
        {
            touchingPlayer = true;
        }
    }

    void DoDamageOnPlayer()
    {
        if (touchingPlayer == true)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(Mathf.RoundToInt(damageOverTime));
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject == player)
        {
            touchingPlayer = false;
        }
    }

}
