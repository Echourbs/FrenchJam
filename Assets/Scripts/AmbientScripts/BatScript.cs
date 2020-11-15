using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : MonoBehaviour
{
   
    Transform player;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float range;

    public GameObject bat;

    float moveMax;

    int direction = 1;

    private Animator an;


    void Start()
    {
        player = MovementScript.player.transform;

        rb = GetComponent<Rigidbody2D>();

        an = GetComponent<Animator>();

        moveMax = transform.position.x + 1;

        bat = this.gameObject;

    }

    void Update()
    {
        //calcula a distancia entre o jogador e o morcego 
        float distanciaPlayer = Vector2.Distance(transform.position, player.position);

        if (distanciaPlayer < range)
        {
            FugirJogador();

        }

        if (distanciaPlayer > range)
        {
            DestruirObjeto();
        }

        //fisica para mover o morcego 
        //transform.Translate(direction * Time.deltaTime * 3f, 0, 0);

        if (transform.position.x > moveMax && direction == 1 || transform.position.x > moveMax && direction == -1)
        {
            direction *= -1; 
        }

    }


    void FugirJogador()
    {
        if (transform.position.x < player.position.x)
        {
            //começa a fugir do jogador quando entra no range do player
            rb.velocity = new Vector2(moveSpeed,3f);
            transform.localScale = new Vector2(1, -1);
            rb.gravityScale = 0.1f;
            an.SetBool("RUN", true);
            
        }
    }

    void DestruirObjeto()
    {
        //Destroi o morcego quando ele bater um range maior do que o player
        if (bat.transform.position.y > range)
        {
           Destroy(bat,10f);
        }
    }
}
