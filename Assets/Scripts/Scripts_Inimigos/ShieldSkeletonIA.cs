using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class ShieldSkeletonIA : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private Animator anim;
    private Transform player;
    private EnemyStatus status;
    [SerializeField]
    Transform centro;
    [Header("Variables for Combat")]
    [SerializeField]
    int damage;
    [SerializeField]
    float EsperaAtaqueMin, EsperaAtaqueMax;
    [SerializeField]
    GameObject damegeColider;
    bool podeAtacar, trava;
    [SerializeField]
    float sttagerMax;
    float sttager;


    [SerializeField]
    float rangeGard, rangeAttack, rangeVision, rangeIdentifyPlayerIsAttackingIt;

    [Header("Variable for moviments")]
    [SerializeField]
    bool StoodGard;
    [SerializeField]
    bool SmartPatrol;
    bool travaParaVirar;
    [SerializeField]
    float EsperaMinVirar, EsperaMaxVirar;
    [SerializeField]
    SensorSmartPatrol sensorChao, sensorParede;
    [SerializeField]
    float moveSpeed, walkRange;
    float Andarmin, Andarmax, PontoGuarda;
    int direction = 1;
    private bool isFollowing;


    float distance;

    bool Garding, alive, move, hit;

    [Header("Timer to revive")]
    [SerializeField]
    bool PodeReviver;
    [SerializeField]
    float MinReviveTime;
    [SerializeField]
    float MaxReviveTime;

    [Header("Variables for attack animation speed")]
    [SerializeField]
    AnimationClip ataqueClip;
    [SerializeField]
    float tempAtaque;

    void Start()
    {
        startBaseVariables();
        startMovimentVariables();
        startDamegeVariables();
        StartAnimationVariables();
    }

    #region Stating of variables
    void startBaseVariables()
    {
        status = gameObject.GetComponent<EnemyStatus>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        cc = gameObject.GetComponent<CapsuleCollider2D>();

        //find the player
        player = MovementScript.player.transform;


    }
    void startMovimentVariables()
    {
        travaParaVirar = true;
        alive = true;
        trava = true;
        if (rangeAttack <= 0.1f)
        {
            rangeAttack = 5;
        }
        if (rangeAttack > rangeGard)
        {
            rangeGard = rangeAttack + 5;
        }
        if (rangeGard > rangeVision)
        {
            rangeVision = rangeGard + 5;
        }
        if (rangeIdentifyPlayerIsAttackingIt < rangeVision)
        {
            rangeIdentifyPlayerIsAttackingIt = rangeVision + 5;
        }

        //set the point were the enemy will stood gard or the area where it will patrulate
        if (StoodGard)
            PontoGuarda = transform.position.x;
        else if (!SmartPatrol)
        {
            Andarmax = transform.position.x + walkRange;
            Andarmin = transform.position.x - walkRange;
        }
    }
    void startDamegeVariables()
    {
        //inicialize damege variables and deactivate the collider
        ShieldedSkeletonDamageCast damegeCast = damegeColider.GetComponent<ShieldedSkeletonDamageCast>();
        damegeCast.master = gameObject;
        damegeCast.damage = damage;
        damegeColider.SetActive(false);
    }
    void StartAnimationVariables()
    {
        //set the speed of the attack animation
        float ataqueFullTime = 0;
        ataqueFullTime = (ataqueClip.length * 1) / tempAtaque;
        anim.SetFloat("MultiplicadorDeVelocidadeAtaque", ataqueFullTime);
    }
    #endregion

    void Update()
    {
         /*while (true)
         {
            
         }*/
        if (alive)
        {
            if (!hit)
            {
                distance = Vector3.Distance(centro.position, player.transform.position);

                if (distance > rangeIdentifyPlayerIsAttackingIt)
                    isFollowing = false;

                if (distance < rangeVision)
                    if (player.position.y < centro.position.y + 6 && player.position.y > centro.position.y - 2)
                        isFollowing = true;

                if (isFollowing)
                {
                    FindDirectionOfPlayer();

                    if (distance < rangeGard)
                    {
                        //defend
                        anim.SetBool("Defender", true);
                        if (distance < rangeAttack)
                        {
                            anim.SetBool("Andar", false);
                            if (podeAtacar)
                            {
                                if (!player.gameObject.GetComponent<MovementScript>().isDead)
                                {
                                    //attack
                                    anim.SetBool("Ataque", true);
                                    trava = true;
                                    podeAtacar = false;
                                }
                            }
                            else if (trava)
                            {
                                trava = false;
                                StartCoroutine("PrepararParaAtaque");
                            }
                        }
                    }
                    else
                        anim.SetBool("Defender", false);

                    if (distance > rangeAttack)
                    {
                        transform.Translate(direction * Time.deltaTime * moveSpeed * transform.right);
                        anim.SetBool("Andar", true);
                    }
                }
                else
                {
                    if (!StoodGard)
                        patroling();
                    else
                        GardingAria();
                }

                //set if he can take damege or not
                status.canTakeDamage = !Garding;

                setOrientation();

                if (move)
                    transform.Translate(direction * Time.deltaTime * moveSpeed * transform.right);
            }

            cc.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            cc.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        anim.SetBool("Morto", IsDead());
    }



    #region Attack
    public void StartAtaque()
    {
        damegeColider.SetActive(true);
        anim.SetBool("Ataque", false);
        anim.SetBool("ContraAtaque", false);
    }

    public void EndAtaque()
    {
        damegeColider.SetActive(false);
    }

    public void StopGard()
    {
        Garding = false;
    }

    public void StartGard()
    {
        Garding = true;
    }
    IEnumerator PrepararParaAtaque()
    {
        yield return new WaitForSeconds(Random.Range(EsperaAtaqueMin, EsperaAtaqueMax));
        podeAtacar = true;
    }
    public void StartMove()
    {
        move = true;
    }
    public void StoptMove()
    {
        move = false;
    }
    #endregion

    #region rection to attack
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<AttackInstance>() || col.gameObject.GetComponent<Flecha>())
            if (distance < rangeIdentifyPlayerIsAttackingIt)
                isFollowing = true;
        if (col.gameObject.GetComponent<AttackInstance>())
        {
            if (col.gameObject.GetComponent<AttackInstance>().master == MovementScript.player)
            {
                Reaction();
            }
        }
        else if (col.gameObject.GetComponent<Flecha>())
        {
            if (col.gameObject.GetComponent<Flecha>().souDoPlayer)
            {
                Reaction();


            }
        }

    }
    void Reaction()
    {
        if (Garding)
        {
            if (distance < rangeAttack)
            {
                anim.SetBool("ContraAtaque", true);
            }

        }
        else
        {

            sttager++;

            if (sttagerMax <= sttager)
            {
                sttager = 0;
                anim.SetTrigger("RecebeuDano");
            }
        }
    }

    public void startHit()
    {
        hit = true;
    }
    public void endHit()
    {
        hit = false;
    }

    #endregion

    #region orientation
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
    #endregion

    #region inactive Behavure

    #region patrol
    void patroling()
    {
        if (SmartPatrol)
        {
            smartPatroling();
        }
        else
        {
            dumbPatrol();
        }
    }

    #region turn
    void virar()
    {
        direction *= -1;
        Invoke("delayvirar", 0.1f);
    }
    void delayvirar()
    {
        travaParaVirar = true;
    }
    #endregion

    #region Smart patrol behavure
    void smartPatroling()
    {
        if (travaParaVirar)
        {
            transform.Translate(direction * Time.deltaTime * moveSpeed * transform.right);
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
                Invoke("virar", Random.Range(EsperaMinVirar, EsperaMaxVirar));

            }
        }
    }
    #endregion

    #region dumb patrol behavure
    void dumbPatrol()
    {
        if (travaParaVirar)
        {
            transform.Translate(direction * Time.deltaTime * moveSpeed * transform.right);
            anim.SetBool("Andar", true);
        }
        else
        {
            anim.SetBool("Andar", false);
        }
        if ((transform.position.x > Andarmax && direction == 1 || transform.position.x < Andarmin && direction == -1))
        {
            if (travaParaVirar)
            {
                travaParaVirar = false;
                Invoke("virar", Random.Range(EsperaMinVirar, EsperaMaxVirar));

            }
        }
    }
    #endregion
    #endregion

    #region gard behavure
    void GardingAria()
    {
        if (CalculoDistanciaDoPontoDeGuarda() > 1.5)
        {
            if (transform.position.x < PontoGuarda)
            {
                direction = 1;
            }
            else if (transform.position.x > PontoGuarda)
            {
                direction = -1;
            }
            transform.Translate(direction * Time.deltaTime * moveSpeed * transform.right);
            anim.SetBool("Andar", true);
        }
        else
        {
            anim.SetBool("Andar", false);
        }
    }
    float CalculoDistanciaDoPontoDeGuarda()
    {
        return Vector2.Distance(transform.position, new Vector2(PontoGuarda, transform.position.y));
    }
    #endregion

    #endregion

    #region death
    public void morre()
    {
        alive = false;
        anim.SetBool("Morto", false);
        StartCoroutine("reviveCoolDown");
    }
    IEnumerator reviveCoolDown()
    {
        if (!PodeReviver) { GetComponent<EnemyDrop>().Drop(); }
        yield return new WaitForSeconds(Random.Range(MinReviveTime, MaxReviveTime));
        ReviveVerify();
        anim.SetBool("Revive", true);
    }
    public void reviver()
    {
        alive = true;
        anim.SetBool("Revive", false);
        status.life = status.maxLife;
        status.StatusBarUpdate();
    }

    void ReviveVerify()
    {
        if (PodeReviver)
        {
            PodeReviver = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    bool IsDead()
    {
        return status.life <= 0;
    }

    bool canTurn = true;
    void FindDirectionOfPlayer()
    {
        if ((transform.position.x < player.position.x && direction == -1) || (transform.position.x > player.position.x && direction == 1))
            if (canTurn)
            {
                canTurn = false;
                Invoke("setDirectionToplayerWithDelay", .75f);
            }

    }
    void setDirectionToplayerWithDelay()
    {
        canTurn = true;
        if (transform.position.x < player.position.x)
            direction = 1;
        else if (transform.position.x > player.position.x)
            direction = -1;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centro.position, rangeAttack);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centro.position, rangeGard);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centro.position, rangeVision);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(centro.position, rangeIdentifyPlayerIsAttackingIt);

    }
}
