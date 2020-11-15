using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatScript : MonoBehaviour
{

    Transform player;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float range;

    public GameObject rat;

    float moveMax;

    int direction = 1;

    private Animator an;
    
    
    void Start()
    {
        player = MovementScript.player.transform;

        rb = GetComponent<Rigidbody2D>();

        an = GetComponent<Animator>();

        moveMax = transform.position.x +1;

        rat = this.gameObject;
        
    }

    void Update()
    {
        //Calcula a distancia entre o jogador e o rato
        float distanciaPlayer = Vector2.Distance(transform.position, player.position);

        
        if(distanciaPlayer < range)
        {
            FugirJogador();
            
        }

        if(distanciaPlayer > range)
        {
            DestruirObjeto();
        }

        //fisica do rato
        //transform.Translate(direction * Time.deltaTime * 3f, 0, 0);


        if(transform.position.x > moveMax && direction == 1 || transform.position.x > moveMax && direction == -1)
        {
            direction *= -1;
        }
   
    }


    void FugirJogador()
    {
        //começa a fugir do jogador quando entra no range do player
        if (transform.position.x < player.position.x)
        {
            rb.velocity = new Vector2(moveSpeed, 0);
            transform.localScale = new Vector2(-1, 1);
            an.SetBool("RUN", true);
        }
    }

    void DestruirObjeto()
    {
        
        //Destroi o rato quando ele bater um range maior do que o player
        if(rat.transform.position.x > range)
        {
            Destroy(rat,10f);
        }
    }

   

}
