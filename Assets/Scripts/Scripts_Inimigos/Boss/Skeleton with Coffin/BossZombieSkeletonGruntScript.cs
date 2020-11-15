using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZombieSkeletonGruntScript : MonoBehaviour
{
    //status
    BossHealthScript bossHealthControler;

    public static GameObject bossSkeletonGrunt;
    Rigidbody2D bodyControl;
    EnemyStatus status;

    public string bossName;

    GameObject player;

    Animator anim;

    [Header("sensors")]
    public Transform topSensor;
    public Transform lowerSensor;
    Transform usableSensor;
    float distanceFromPlayer;


    [Header("movimentação")]
    public float moveSpeed;


    [Header("ataque melee")]
    public float meleeDamage;
    public float meleeImpulseForce;
    public float meleeRange;
    public float meleeRangeToStopMove;
    public bool isBossAtacking = false;
    

    [Header("ataque boomerang")]
    public GameObject coffin;
    public Transform boomerangCoffinInstancePoint;
    public Transform boomerangCoffinReturnPoint;
    public float boomeranCoffingSpeed;
    bool executingCoffingAtack = false;
    float distanceToThrowCoffin;
    bool canThrowCoffin;
    public float timeToReloadCoffinThrow;

    [Header("objetos a ignorar")]
    public GameObject[] ignorablesObjects;



    void Awake()
    {
        bossSkeletonGrunt = this.gameObject;
    }

    void Start()
    {
        status = GetComponent<EnemyStatus>();

        player = MovementScript.player.gameObject;
        anim = GetComponent<Animator>();


        bossSkeletonGrunt.SetActive(false);
        distanceToThrowCoffin = boomerangCoffinReturnPoint.localPosition.z - 1.25f;
        canThrowCoffin = true;


        for (int i = 0; i < ignorablesObjects.Length; i++)
        {
            try
            {
                Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), ignorablesObjects[i].GetComponent<Collider2D>(),true);
            }
            finally
            {
                print("cant be done in " + ignorablesObjects[i].name);
            }
            
        }
    }

    void OnEnable()
    {
        Invoke("startDelay", Time.deltaTime);
        isBossAtacking = false;
        executingCoffingAtack = false;
    }

    void startDelay()
    {
        BossHealthScript.bossLifeBar.gameObject.SetActive(true);
        bossHealthControler = BossHealthScript.bossLifeBar.GetComponent<BossHealthScript>();

        bossHealthControler.boss = gameObject;
        bossHealthControler.bossName = bossName;
        bossHealthControler.reloadStatusWithNoDeactivation();
        bossHealthControler.LoadBossLifBar();

        bodyControl = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        checkDistance();

        if (isBossAtacking == false && executingCoffingAtack == false)
        {
             adaptRotation();
        }
        


        if (executingCoffingAtack == false && isBossAtacking == false)
        {
            if (status.life > status.maxLife / 2)
            {
                if (distanceFromPlayer < meleeRange)
                {
                    anim.SetBool("atackMelee", true);
                }
            }
            else
            {
                if (distanceFromPlayer < distanceToThrowCoffin && canThrowCoffin == true)
                {
                    anim.SetBool("ThrowCoffin", true);
                    canThrowCoffin = false;
                }
                else if (distanceFromPlayer < meleeRange)
                {
                    anim.SetBool("atackMelee", true);
                }
                

            }
        }

    }

    void FixedUpdate()
    {
        if(isBossAtacking == false && distanceFromPlayer > meleeRangeToStopMove && executingCoffingAtack == false)
        {
            //calcula direção
            float direction = Mathf.Clamp((transform.position.x - player.transform.position.x), -1f, 1f) * -1f;
            bodyControl.transform.Translate(transform.right * direction * moveSpeed * Time.fixedDeltaTime);
        }
        
    }

    void adaptRotation()
    {
        if(player.transform.position.x > transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void checkDistance()
    {
        if(player.transform.position.y > transform.position.y + 6.72f)
        {
            usableSensor = topSensor;
            Debug.DrawLine(usableSensor.position, player.transform.position, Color.red);
        }
        else
        {
            usableSensor = lowerSensor;
            Debug.DrawLine(usableSensor.position, player.transform.position, Color.blue);
        }

        distanceFromPlayer = Vector2.Distance(usableSensor.transform.position, player.transform.position);
    }

    void BossStartAtackFrame()
    {
        isBossAtacking = true;
    }

    void BossEndAtackFrame()
    {
        isBossAtacking = false;
        anim.SetBool("atackMelee", false);
    }

    public IEnumerator BoomerangCoffinAtack()
    {
        

        if(executingCoffingAtack == false)
        {

            coffin.SetActive(true);

            boomerangCoffinInstancePoint.transform.LookAt(MovementScript.player.transform.position - new Vector3(0, 1.45f, 0));

            //movimenta a lapida para a mao do boss
            coffin.transform.position = boomerangCoffinInstancePoint.transform.position;

            executingCoffingAtack = true;

            //empurra a lapide para o ponto desejado;
            while (coffin.transform.position != boomerangCoffinReturnPoint.position)
            {
                coffin.transform.position = Vector3.MoveTowards(coffin.transform.position, boomerangCoffinReturnPoint.position, boomeranCoffingSpeed * Time.fixedDeltaTime);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            //da um delay no ar ate voltar
            yield return new WaitForSeconds(0.32f);

            //faz o boomerang voltar
            while(coffin.transform.position != boomerangCoffinInstancePoint.position)
            {
                coffin.transform.position = Vector3.MoveTowards(coffin.transform.position, boomerangCoffinInstancePoint.position, boomeranCoffingSpeed * Time.fixedDeltaTime);
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            coffin.SetActive(false);
            anim.SetTrigger("CoffinRecived");
            anim.SetBool("ThrowCoffin",false);
            executingCoffingAtack = false;
        }

        //recarrega a execução do arremesso
        Invoke("reloadCoffinAtack", timeToReloadCoffinThrow);


    }

    void reloadCoffinAtack()
    {
        canThrowCoffin = true;
    }

}
