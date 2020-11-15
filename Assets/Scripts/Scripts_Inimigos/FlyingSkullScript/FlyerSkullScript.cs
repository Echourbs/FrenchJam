using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerSkullScript : MonoBehaviour
{
    public float damageDealt;

    public float speed;

    public float heightCorrection;

    EnemyStatus myStatus;

    Transform player;

    Animator animControl;

    SpriteRenderer spR;

    [SerializeField]
    float lifeTime = 9.4f;

    bool startedHunt = false;

    [SerializeField]
    float mainPersuitRange;

    [Header("delay de movimentação quando muito proximo")]
    public float pseudoPersuitSpeed;
    Transform pseudoLocation;
    Vector3 direction;

    [Header("ataque")]
    public GameObject atackFlyerPrefab;
    [SerializeField]
    float rangeToGenerateAtack;

    bool canAtack = true;



    MovementScript playerControl;


    void Start()
    {
        myStatus = GetComponent<EnemyStatus>();
        animControl = GetComponent<Animator>();
        spR = GetComponent<SpriteRenderer>();

        //player
        player = MovementScript.player.transform;
        playerControl = player.GetComponent<MovementScript>();

        //gera um ponto vazio e o parenteia 
        pseudoLocation = new GameObject().transform;

        //habilita ataque
        canAtack = true;

        if (player.position.x - transform.position.x < 0)
        {
            spR.flipX = true;
        }
        else
        {
            spR.flipX = false;
        }


        //boleana que indica a morte do objeto
        startedHunt = false;
    }

    //procura o player e o caça
    public void Hunt()
    {
        //movimenta uma posição pseudonica na localização do player
        pseudoLocation.position = player.transform.position;
        transform.eulerAngles = Vector3.zero;
        startedHunt = true;

        //chama a funçao de fade e morte com delay
        Invoke("FadeAndKill", lifeTime);
    }

    void Update()
    {
        //apaga o inimigo se estiver sem vida
        if (myStatus.life <= 0)
        {
            animControl.SetTrigger("Kill");
            startedHunt = false;
        }

        if (startedHunt)
        {


            player = MovementScript.player.transform;
            Vector3 playerPosition = player.transform.position + new Vector3(0, heightCorrection, 0);

            if (playerControl.usingCrounch == false)
            {
                playerPosition = player.transform.position + new Vector3(0, heightCorrection * 2 / 3, 0);
            }

            float dist = Vector2.Distance(transform.position, playerPosition);

            if(dist > mainPersuitRange)
            {
                pseudoLocation.position = Vector2.Lerp(pseudoLocation.position, playerPosition, 0.95f * Time.deltaTime);

                if(transform.parent != null)
                {
                    pseudoLocation.SetParent(null);
                }
            }
            else
            {
                pseudoLocation.SetParent(transform);
            }
            

            if (playerPosition.x - transform.position.x < 0)
            {
                spR.flipX = true;
            }
            else
            {
                spR.flipX = false;
            }

            direction = pseudoLocation.position - transform.position;

            if (dist < rangeToGenerateAtack)
            {
                direction.y = Mathf.Clamp(direction.y, -0.2f, 0.2f);
            }

            transform.Translate(direction.normalized * speed * Time.deltaTime);

            
        }
    }

    void atackReload()
    {
        canAtack = true;
    }

    void FadeAndKill()
    {
        //inicia o fade e apaga o objeto
        if (gameObject.active == true)
        {
            animControl.SetTrigger("Kill");
            startedHunt = false;
        }
        else
        {
            Destroy(pseudoLocation.gameObject);
            Destroy(gameObject);
        }
        
    }


    //apaga o objeto no fim da animação hit
    public void DieAfterHit()
    {
        Destroy(pseudoLocation.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == player || c.gameObject.GetComponent<PlayerSensor>())
        {
            //da dano ao player se puder bater
            if (canAtack == true)
            {
                AttackInstance atack = Instantiate(atackFlyerPrefab, transform.position, transform.rotation).GetComponent<AttackInstance>();
                atack.AtribuirValores(AttackInstance.CreateDamageDictionaryType("void", Mathf.RoundToInt(damageDealt)), gameObject);
                canAtack = false;
            }

            //realinha a caveira para se movimentar mais proximo aos eixos horizontais do jogador
            if(pseudoLocation.parent == transform)
            {
                float height = player.transform.position.y + heightCorrection;

                float heightAlignToPlayer = 0.7f;

                if(transform.position.y > height)
                {
                    heightAlignToPlayer = -0.7f;
                }

                //verifica se o player esta a esqueda ou a direita
                if (transform.position.x > player.position.x)
                {
                    //caso player a esquerda 
                    pseudoLocation.transform.localPosition = new Vector2(-10f, heightAlignToPlayer);

                }
                else
                {
                    //caso player a direita
                    pseudoLocation.transform.localPosition = new Vector2(10f, heightAlignToPlayer);
                }


            }
        }



    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if(c.gameObject == player || c.gameObject.GetComponent<PlayerSensor>())
        {
            canAtack = true;
        }
    }



}
