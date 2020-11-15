using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiabreteIA : MonoBehaviour
{
    public EnemyStatus status;

    Transform player;

    [SerializeField]
    float rangeFollow, rangeAttack;

    [SerializeField]
    GameObject vidaFundo;

    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb;

    float Andarmin;

    float Andarmax;

    int Direcao = 1;

    private bool isFollowing;
    public float delayAttack;
    private bool canAttack = true;
    private bool isAttacking;

    public Transform pontoFranco;
    private Animator anim;

    public GameObject prefabDano;
    public Transform ancoraAttack;
    public float damage;

    void Start()
    {
        player = MovementScript.player.transform;

        //Captura dois pontos min e max que o inimigo se localiza, assim ficando em um looping indo de um lado para o outro
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        Andarmax = transform.position.x + 1;
        Andarmin = transform.position.x - 1;

        Physics2D.queriesStartInColliders = true;

        if (status == null)
        {
            status = GetComponent<EnemyStatus>();
        }

        anim.SetBool("RUN", true);

        isFollowing = false;
        canAttack = true;
        vidaFundo.SetActive(true);
    }


    void Update()
    {
        //Captura a distancia entre o inimigo e o jogador
        float distancePlayer = Vector2.Distance(transform.position, player.position);

        //Se tiver no range do ataque
        if (distancePlayer < rangeAttack)
        {
            if (canAttack)
            {
                anim.SetBool("ATTACK", true);
                anim.SetBool("RUN", false);
                Invoke("AttackDelay", delayAttack);
                canAttack = false;
            }

        }

        //Se tiver no range de seguir
        else if (distancePlayer < rangeFollow && distancePlayer > rangeAttack)
        {
            FollowPlayer();
        }

        else
        {
            StopFollow();
        }

        if (!isFollowing)
        {
            transform.Translate(Direcao * Time.deltaTime * moveSpeed, 0, 0);
        }

        if ((transform.position.x > Andarmax && Direcao == 1 && !isFollowing || transform.position.x < Andarmin && Direcao == -1 && !isFollowing))
        {
            Direcao *= -1;
        }

        if (status.life <= 0)
        {
            Die();
        }
    }

    void AttackDelay()
    {
        canAttack = true;
    }

    public void AttackLastFrame()
    {
        anim.SetBool("ATTACK", false);
        isAttacking = false;
    }

    public void AttackFirstFrame()
    {
        isAttacking = true;
    }

    //Segue o jogador caso ele estiver dentro do raio do inimigo
    void FollowPlayer()
    {
        isFollowing = true;
        anim.SetBool("RUN", true);

        if (!isAttacking)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), moveSpeed * Time.deltaTime);
        }

        if (transform.position.x > player.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (transform.position.x < player.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }

    //Para de seguir o jogador caso ele estiver fora do raio do inimigo
    void StopFollow()
    {
        isFollowing = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Die()
    {
        anim.SetBool("DIE", true);
        moveSpeed = 0;
        vidaFundo.SetActive(false);
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), MovementScript.player.GetComponent<CapsuleCollider2D>(), true);
        Destroy(gameObject, 10f);
    }

    public void DamegeOn()
    {
        AttackInstance isDamage = Instantiate(prefabDano, ancoraAttack.transform).GetComponent<AttackInstance>();
        isDamage.master = this.gameObject;
        isDamage.transform.parent = null;
        isDamage.damage = AttackInstance.CreateDamageDictionary(Mathf.RoundToInt(damage));
    }
}
