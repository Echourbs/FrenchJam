using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class KillBox : MonoBehaviour
{
    BoxCollider2D bc;
    Rigidbody2D rb;
    [HideInInspector]
    public Effect damageEffect = new Effect();
    [HideInInspector]
    public GameObject master;

    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
        bc.size = new Vector2(2000000, 20); 

        rb = gameObject.GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Static;
        
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.GetComponent<EnemyStatus>())
        {
            c.gameObject.GetComponent<EnemyStatus>().CastDamage(20000, EnemyStatus.damageTypes.physical);
            //
            damageEffect.Invoke(c.gameObject);
        }
        if (c.gameObject.GetComponent<PlayerHealth>())
        {
            c.gameObject.GetComponent<PlayerHealth>().TakeDamage(20000);
            if (MovementScript.player.GetComponent<Combat_Player>().usingShield) { MovementScript.player.GetComponent<SistemaItens>().itensMao[1].item.efeito.Invoke(master); }
            //
            damageEffect.Invoke(c.gameObject);
        }
    }


}
