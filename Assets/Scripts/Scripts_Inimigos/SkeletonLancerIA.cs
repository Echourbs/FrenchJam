using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class SkeletonLancerIA : MonoBehaviour
{
    enum BehaviourType { Gard, SmartPatrol, DumbPatrol }

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private Animator anim;
    private Transform player;
    private EnemyStatus status;
    bool alive;
    float distancia;
    [SerializeField]
    Transform centro;
    
    [Header("Variables for Combat")]
    [SerializeField]
    int damage;
    [SerializeField]
    float EsperaAtaqueMin, EsperaAtaqueMax, rangeSee, rangeFollow, rangeAttack;
    [SerializeField]
    GameObject damegeColider;
    bool podeAtacar, willSlide;
    [SerializeField]
    float sttagerMax;
    float sttager;

    [Header("Variable for moviments")]
    [SerializeField]
    BehaviourType patrolType;
    [SerializeField]
    float walkSpeed, runSpeed, slideSpeed, walkRange, delayTurnMin, delayTurnMax;
    [SerializeField]
    SensorSmartPatrol sensorChao, sensorParede;
    float Andarmin, Andarmax, PontoGuarda;
    int direction = 1, slideDirection;
    private bool isFollowing, sliding, travaParaVirar, Anda;

    [Header("revive variables")]
    [SerializeField]
    int ReviveAmount;
    [SerializeField]
    float MinReviveTime, MaxReviveTime;

    private void Start()
    {
        Anda = true;
        travaParaVirar = true;
        willSlide = true;
        iniciarVariaveisBase();
        iniciarVariaveisCombate();
        iniciarVariaveisMovimento();

    }
#region Iniciar Variaveis
    void iniciarVariaveisBase()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        status = gameObject.GetComponent<EnemyStatus>();
        player = MovementScript.player.transform;
    }
    void iniciarVariaveisCombate()
    {
        ShieldedSkeletonDamageCast damegeCast = damegeColider.GetComponent<ShieldedSkeletonDamageCast>();
        damegeCast.master = gameObject;
        damegeCast.damage = damage;
        damegeColider.SetActive(false);
    }
    void iniciarVariaveisMovimento()
    {
        switch (patrolType)
        {
            case BehaviourType.Gard:
                PontoGuarda = transform.position.x;
                break;
            case BehaviourType.DumbPatrol:
                Andarmin = transform.position.x - walkRange;
                Andarmax = transform.position.x + walkRange;
                break;
            default:
                PontoGuarda = transform.position.x;
                break;
        }
    }
    #endregion

    bool IsAlive()
    {
        return alive = status.life >= 0;
    }

    private void Update()
    {
        alive = IsAlive();
        anim.SetBool("Morto", !alive);

        if (alive)
        {
            distancia = Vector2.Distance(centro.position, player.position);
            if (distancia <= rangeFollow)
                isFollowing = true;

            if (distancia >= rangeSee)
            {
                isFollowing = false;
                willSlide = true;
            }
                
            
            if (isFollowing)
            {
                findPlayer();
                if (distancia > rangeAttack)
                {
                    movivento(runSpeed);
                    anim.SetBool("Correr", true);
                    anim.SetBool("Andar", false);
                }
                else
                {
                    anim.SetBool("Correr", false);
                    if (willSlide)
                    {
                        willSlide = false;
                        anim.SetTrigger("Deslizar");
                    }
                    else if(podeAtacar)
                    {
                        podeAtacar = false;
                        anim.SetTrigger("Attack");
                    }
                }
            }
            else
            {
                anim.SetBool("Correr", false);
                inative();
            }

            if (sliding)
            {
                movivento(slideSpeed);
            }
            else
            {
                setOrientation();
            }

            status.canTakeDamage = !sliding;
            cc.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rb.velocity = Vector2.zero;
            cc.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void movivento(float vel)
    {
        if(Anda)
        transform.Translate(direction * Time.deltaTime * vel * transform.right);
    }
    void setOrientation()
    {
        if (direction == 1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

    }

    #region Ativo
    void seguir()
    {
        movivento(runSpeed);
    }

    void findPlayer()
    {
        if (!sliding)
        {
            if (centro.position.x < player.position.x)
            {
                direction = 1;
            }
            else if (centro.position.x > player.position.x)
            {
                direction = -1;
            }
        }
    }

    void startSlide()
    {
        sliding = true;
        Physics2D.IgnoreCollision(cc, player.GetComponent<CapsuleCollider2D>(), true);
    }
    void endSlide()
    {
        sliding = false;
        Physics2D.IgnoreCollision(cc, player.GetComponent<CapsuleCollider2D>(), false);
    }
    void endSlideAnin()
    {
        podeAtacar = true;
    }

    void StartAttack()
    {
        damegeColider.SetActive(true);
    }

    void EndAttack()
    {
        damegeColider.SetActive(false);
    }

    void pararDeAndarComOAtaque()
    {
        Anda = false;
    }
    void VoltarAAndarComOAtaque()
    {
        Anda = true;
    }

    IEnumerator attackCoolDown()
    {
        yield return new WaitForSeconds(Random.Range(EsperaAtaqueMin, EsperaAtaqueMax));
        willSlide = true;
    }
#endregion

    #region inative
    void inative()
    {
        switch (patrolType)
        {
            case BehaviourType.Gard:
                Garding();
                break;
            case BehaviourType.SmartPatrol:
                smart();
                break;
            case BehaviourType.DumbPatrol:
                break;
                Dumb();
            default:
                Garding();
                break;
        }
    }

    void Garding()
    {
        if (Vector2.Distance(centro.position,new Vector2(PontoGuarda, centro.position.y)) > 1.5)
        {
            if (centro.position.x < PontoGuarda)
            {
                direction = 1;
            }
            else if (centro.position.x > PontoGuarda)
            {
                direction = -1;
            }
            movivento(walkSpeed);
            anim.SetBool("Andar", true);
        }
        else
        {
            anim.SetBool("Andar", false);
        }
    }
#region patrol
    void Dumb()
    {
        if (travaParaVirar)
        {
            movivento(walkSpeed);
            anim.SetBool("Andar", true);
        }
        else
        {
            anim.SetBool("Andar", false);
        }
        if ((centro.position.x > Andarmax && direction == 1 || centro.position.x < Andarmin && direction == -1))
        {
            if (travaParaVirar)
            {
                travaParaVirar = false;
                Invoke("ChangeOrientation", Random.Range(delayTurnMin, delayTurnMax));
            }
        }
    }
    void smart()
    {
        if (travaParaVirar)
        {
            movivento(walkSpeed);
            anim.SetBool("Andar", true);
        }
        else
        {
            anim.SetBool("Andar", false);
        }
        if (sensorChao.AcabouChao || sensorParede.EncontrouParede)
        {
            if (travaParaVirar)
            {
                travaParaVirar = false;
                Invoke("virar", Random.Range(delayTurnMin, delayTurnMax));
            }
        }
    }
    void ChangeOrientation()
    {
        direction *= -1;
        Invoke("delayvirar", 0.1f);
    }
    void delayvirar()
    {
        travaParaVirar = true;
    }
    #endregion
    #endregion

    #region reaction
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<AttackInstance>() || col.gameObject.GetComponent<Flecha>())
            if (distancia < rangeSee)
                isFollowing = true;

        if (!sliding)
        {
            if (col.gameObject.GetComponent<AttackInstance>())
            {
                if (col.gameObject.GetComponent<AttackInstance>().master == MovementScript.player)
                {
                    StartCoroutine(Reaction());
                    
                }
            }
            else if (col.gameObject.GetComponent<Flecha>())
            {
                if (col.gameObject.GetComponent<Flecha>().souDoPlayer)
                {
                    StartCoroutine(Reaction());
                }
            }
        }

    }
    IEnumerator Reaction()
    {
        sttager++;
        if (sttagerMax <= sttager)
        {
            sttager = 0;
            willSlide = true;
        }

        yield return new WaitForSeconds(0);
        anim.SetTrigger("takedamege");

    }
    #endregion

    #region morte
    public void morre()
    {
        //anim.SetBool("Morto", false);
        StartCoroutine("reviveCoolDown");
    }
    IEnumerator reviveCoolDown()
    {
        if(ReviveAmount < 0){ GetComponent<EnemyDrop>().Drop();}
        yield return new WaitForSeconds(Random.Range(MinReviveTime, MaxReviveTime));
        ReviveVerify();
        anim.SetBool("Revive", true);
    }
    public void reviver()
    {
        anim.SetBool("Revive", false);
        status.life = status.maxLife;
        status.StatusBarUpdate();
    }

    void ReviveVerify()
    {
        if (ReviveAmount >= 0)
        {
            ReviveAmount--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
#endregion

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centro.position, rangeAttack);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centro.position, rangeFollow);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centro.position, rangeSee);
    }
}
