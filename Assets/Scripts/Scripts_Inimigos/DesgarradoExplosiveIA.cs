using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesgarradoExplosiveIA : MonoBehaviour
{
    public EnemyStatus status;

    Transform player;

    [SerializeField]
    LayerMask layer;
    [SerializeField]
    Transform sensorFloor1;
    [SerializeField]
    Transform sensorFloor2;
    [SerializeField]
    Transform sensorWall1;
    [SerializeField]
    Transform sensorWall2;

    [SerializeField]
    float rangeFollow, rangeAttack;

    [SerializeField]
    GameObject lifeBar;

    Rigidbody2D rb;

    [SerializeField]
    bool SmartPatrol;

    [SerializeField]
    private int dano;

    [SerializeField]
    float moveSpeed, walkRange, visionRange;
    

    [SerializeField]
    bool PodeReviver;

    float Andarmin, Andarmax;
    float distancePlayer;

    int direction = 1;
    private int xp;

    private bool isFollowing;
    private bool canAttack = true;
    private bool isDead;
    private bool rebirth;
    private bool awakeningAfterRebirth;

    private Animator anim;

    void Start()
    {
        player = MovementScript.player.transform;

        //Captura dois pontos min e max que o inimigo se localiza, assim ficando em um looping indo de um lado para o outro
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        Andarmax = transform.position.x + walkRange;
        Andarmin = transform.position.x - walkRange;

        Physics2D.queriesStartInColliders = true;

        if (status == null)
        {
            status = GetComponent<EnemyStatus>();
        }

        anim.SetBool("RUN", true);

        xp = 50;

        isFollowing = false;
        isDead = false;
        rebirth = false;
        awakeningAfterRebirth = false;
        canAttack = true;
        lifeBar.SetActive(true);
    }

    void Update()
    {
        //Captura a distancia entre o inimigo e o jogador
        distancePlayer = Vector2.Distance(transform.position, player.position);

        //Se tiver no range do ataque
        if (distancePlayer < rangeAttack)
        {
            if (canAttack && !awakeningAfterRebirth)
            {
                if (!player.gameObject.GetComponent<MovementScript>().isDead)
                {
                    Explode();
                }
                
            }
        }

        //Se tiver no range de seguir
        else if (distancePlayer < rangeFollow && distancePlayer > rangeAttack)
        {
            isFollowing = true;
        }

        else if(distancePlayer > visionRange)
        {
            StopFollow();
        }
        if (isFollowing)
        {
            FollowPlayer();
        }

        if (!isFollowing)
        {
            transform.Translate(direction * Time.deltaTime * moveSpeed * -transform.right);
            //
            bool wall = Physics2D.Linecast(sensorWall1.position, sensorWall2.position, layer);
            bool floor1 = Physics2D.Raycast(sensorFloor1.position, Vector3.forward, Mathf.Infinity, layer);
            bool floor2 = Physics2D.Raycast(sensorFloor2.position, Vector3.forward, Mathf.Infinity, layer);
            if((!floor1&&floor2)||wall){
                direction *= -1;
            }
        }

        // if ((transform.position.x > Andarmax && direction == 1 && !isFollowing || transform.position.x < Andarmin && direction == -1 && !isFollowing))
        // {
        //     direction *= -1;
        // }
        if (direction == 1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }


        if (status.life <= 0)
        {
            isDead = true;
            Die();
        }

        if (status.inDamage)
        {
            TakeDamage(0);
            if (distancePlayer < visionRange)
            {
                FollowPlayer();
            }
        }
    }

    void Explode()
    {
        rebirth = true;
        moveSpeed = 0;
        anim.SetBool("RUN", false);
        anim.SetBool("DAMAGE", true);
        
    }

    public void ExplodeFrame()
    {
        anim.SetBool("DAMAGE", false);
        status.inDamage = false;
        if (distancePlayer < rangeAttack + 1)
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), MovementScript.player.GetComponent<CapsuleCollider2D>(), true);
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            PlayerHealth ph = MovementScript.player.GetComponent<PlayerHealth>();
            lifeBar.SetActive(false);
            ph.TakeDamage(dano, EnemyStatus.damageTypes.physical, transform.position);
            anim.SetBool("EXPLOSION", true);
            Invoke("Rebirth", 4f);
        }
        else
        {
            if (status.life > 1)
            {
                rebirth = false;
                moveSpeed = 2;
            }
        }       
    }

    //Segue o jogador caso ele estiver dentro do raio do inimigo
    void FollowPlayer()
    {
        if (moveSpeed > 0)
        {
            anim.SetBool("RUN", true);
        }
        isFollowing = true;

        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.position.x, transform.position.y), moveSpeed * Time.deltaTime);

        if (transform.position.x < player.position.x && !rebirth)
        {
            direction = -1;
        }
        else if (transform.position.x > player.position.x && !rebirth)
        {
            direction = 1;
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
        rebirth = true;
        status.life = 1;
        if (isDead)
        {
            //player.GetComponent<PlayerStats>().playerXP += xp;
            isDead = false;
        }         
        moveSpeed = 0;
        anim.SetBool("RUN", false);
        anim.SetBool("EXPLOSION", true);
        lifeBar.SetActive(false);
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), MovementScript.player.GetComponent<CapsuleCollider2D>(), true);
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        if(!PodeReviver){ GetComponent<EnemyDrop>().Drop();}
        Invoke("Rebirth", 4f);
    }

    public int TakeDamage(int damageAmount)
    {
        moveSpeed = 0;
        anim.SetBool("DAMAGE", true);
        status.inDamage = false;
        //Transferir logica de dano pra cá
        return -1;
    }

    void Rebirth()
    {
        if (PodeReviver)
        {
            awakeningAfterRebirth = true;
            anim.SetBool("EXPLOSION", false);
            anim.SetBool("REBIRTH", true);
            status.life = 90;
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), MovementScript.player.GetComponent<CapsuleCollider2D>(), false);
            GetComponent<CapsuleCollider2D>().enabled = true;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            PodeReviver = false;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void LastFrameOnRebirth()
    {
        rebirth = false;
        anim.SetBool("RUN", true);
        anim.SetBool("DAMAGE", false);
        anim.SetBool("REBIRTH", false);
        moveSpeed = 2;
        lifeBar.SetActive(true);
        status.CastDamage(0, EnemyStatus.damageTypes.physical);
        Invoke("ReadyForExplosion", 0.5f);
    }

    void ReadyForExplosion()
    {
        awakeningAfterRebirth = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeAttack);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, rangeFollow);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visionRange);

    }
}
