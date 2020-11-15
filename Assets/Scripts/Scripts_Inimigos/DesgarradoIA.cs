using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesgarradoIA : MonoBehaviour
{
    public EnemyStatus status;

    [SerializeField]
    Transform player;

    [SerializeField]
    float rangeFollow;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    GameObject lifeBar;

    Rigidbody2D rb;

    private Vector3 initialPosition;
    private Vector2 direction;
    private Animator anim;
    private SpriteRenderer sr;
    private EnemyStatus es;

    private bool isDead;

    private float distancePlayer;

    void Start()
    {
        player = MovementScript.player.transform;

        //Captura dois pontos min e max que o inimigo se localiza, assim ficando em um looping indo de um lado para o outro
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        es = GetComponent<EnemyStatus>();


        if (status == null)
        {
            status = GetComponent<EnemyStatus>();
        }
        anim.SetBool("RUN", false);
        anim.SetBool("REBIRTH", false);

        isDead = false;
        lifeBar.SetActive(true);
    }


    void Update()
    {
        //Captura a distancia entre o inimigo e o jogador
        distancePlayer = Vector2.Distance(transform.position, player.position);

        if (distancePlayer < rangeFollow && !es.inDamage && !isDead)
        {
            FollowPlayer();
        }
        else if(!es.inDamage && !isDead)
        {
            StopFollow();
        }

        if (status.life <= 0)
        {
            isDead = true;
            Die();
        }

        if (es.inDamage)
        {
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        es.inDamage = false;
        anim.SetBool("DAMAGE", true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(0.5f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetBool("DAMAGE", false);
    }

    //Segue o jogador caso ele estiver dentro do raio do inimigo
    void FollowPlayer()
    {
        if (transform.position.x < player.position.x)
        {
            //inimigo vai para esquerda atras do jogador, caso ele esteje na direita
            sr.flipX = false;
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            anim.SetBool("RUN", true);
            direction = new Vector2(-1, 1);
            transform.localScale = direction;

        }
        else
        {
            //inimigo vai para direita atras do jogador, caso ele esteje na esquerda
            sr.flipX = false;
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            anim.SetBool("RUN", true);
            direction = new Vector2(1, 1);
            transform.localScale = direction;
        }
    }

    //Para de seguir o jogador caso ele estiver fora do raio do inimigo
    void StopFollow()
    {
        anim.SetBool("RUN", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Die()
    {
        lifeBar.SetActive(false);
        moveSpeed = 0;
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), MovementScript.player.GetComponent<CapsuleCollider2D>(), true);
        anim.SetBool("DIE", true);
        anim.SetBool("REBIRTH", false);
        Invoke("Rebirth", 2f);
    }

    void Rebirth()
    {
        anim.SetBool("DIE", false);
        anim.SetBool("REBIRTH", true);
        es.life = 90;
        isDead = false;      
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), MovementScript.player.GetComponent<CapsuleCollider2D>(), false);
    }

    public void LastFrameOnRebirth()
    {
        moveSpeed = 4;
        lifeBar.SetActive(true);
        es.CastDamage(0, EnemyStatus.damageTypes.physical);

    }
}
