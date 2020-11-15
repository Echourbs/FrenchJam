using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo_IA : MonoBehaviour
{
    public EnemyStatus status;

    Transform player;

    [SerializeField]
    float range;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb;

    float moveMin;

    float moveMax;

    int dir = 1;

    private Animator anim;

    void Start()
    {
        //Captura dois pontos min e max que o inimigo se localiza, assim ficando em um looping indo de um lado para o outro
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        moveMax = transform.position.x + 1;
        moveMin = transform.position.x - 1;

        Physics2D.queriesStartInColliders = true;

        player = MovementScript.player.transform;

        if(status == null)
        {
            status = this.GetComponent<EnemyStatus>();
        }
    }


    void Update()
    {
        //Captura a distancia entre o inimigo e o jogador
        float distanciaPlayer = Vector2.Distance(transform.position, player.position);

        if(distanciaPlayer < range)
        {
            SeguirJogador();
        }
        else
        {
            PararJogador();
        }

        transform.Translate(dir * Time.deltaTime * 3f, 0, 0);

        if((transform.position.x > moveMax  && dir == 1 || transform.position.x < moveMin && dir == -1))
        {
            dir *= -1;
        }
                
        if(status.life <= 0)
        {
            Die();
        }   
        
        

    }

    //Segue o jogador caso ele estiver dentro do raio do inimigo
    void SeguirJogador()
    {
       if(transform.position.x < player.position.x)
       {
            //inimigo vai para esquerda atras do jogador, caso ele esteje na direita
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
       else 
       {
            //inimigo vai para direita atras do jogador, caso ele esteje na esquerda
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);          
       }
    }

    //Para de seguir o jogador caso ele estiver fora do raio do inimigo
    void PararJogador()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Die()
    {
        //Destroi o Inimigo atraves da tag que se localiza
        Destroy(this.gameObject);
        //gameObject.tag = "Neutralized";
    }

}


