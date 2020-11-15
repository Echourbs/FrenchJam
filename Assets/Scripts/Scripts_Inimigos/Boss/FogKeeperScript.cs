using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogKeeperScript : MonoBehaviour
{
    //variavel que bloqueia a inicialização do enable antes do start
    bool loadingBoss = true;

    public string bossName;

    //proprio objeto
    public static GameObject fogKeeperBoss;


    //barra de status
    BossHealthScript bossHealthControler;

    //indica se esta morto
    bool bossDead;

    //player
    GameObject player;

    //lista de pontos fracos
    [Header("lista de pontos fracos, podem ser adicionados a mao (e especificar uma sequencia especifica), ou pode deixar que o codigo do ponto fraco se auto-insira na lista")]
    public GameObject[] weakPoint;
    GameObject nextWallToDesactivate;



    [Header("locais de instancia")]
    [SerializeField]
    Transform staffPoint;

    [SerializeField]
    Transform underFogPoint;

    [Header("triggers de dano e controle de padrao do chefe")]
    //varival que controla o quanto de dano o boss deve tomar para tocar a animção de dano
    [SerializeField]
    float damageTriggerByDamage;
    [HideInInspector]
    public bool bossIsStaggered = false;

    //variaveis de controle de padrao de ataques do chefe (trocam o padrao quando passam seus valores)
    [SerializeField]
    float lifeUntilSecondPattern;
    [SerializeField]
    float lifeUntilThirdPattern;


    //teleporte
    //variavel que manipula o teletransporte do boss quando um dos pontos fracos for destruido
    [HideInInspector]
    public Vector3 bossNewPositionOnTeleport;
    [HideInInspector]
    public bool weakPointDestroyed = false;







    //vfx
    [Header("geração de fog ao criar inimigos")]
    public GameObject fogTrail;
    public float fogDelayCreation;
    public int fogAmount;






    //status do boss
    [HideInInspector]
    public EnemyStatus bossStatus;

    //grava a vida e atualiza funçoes ao ser reduzida
    float lifeRecorder;
    //calcula a quantidade de vida perdia
    float deltaLost;
    //padrao de ataque que ele vai seguir
    int atualPattern = 1;

    //permite a chamada da função de invokação dos fantasmas e esqueletos
    bool canInvokeghost = true;

   





    [Header("Geração de inimigos e ataques especificos")]
    [SerializeField]
    float BaseDelay;


    public GameObject flyingSkullPrefab;
    [SerializeField]
    
    float flyingSkullDelayGenerationBetweenSkulls;
    [SerializeField]
    Transform[] flyerGenerationPoints;

    public float atualRangeForCloseCombatGhostSummons;
    [SerializeField]
    Transform[] localsForGroundInstance;

    public GameObject warriorPrefab;
    [SerializeField]
    float warriorDelayGeneration;

    public GameObject grabberPrefab;
    [SerializeField]
    float grabberDelayGeneration;

    public GameObject chargerPrefab;
    [SerializeField]
    float chargerDelayGeneration;








    //a orbe é um ataque do chefe
    [Header("orbe de fog")]
    [SerializeField]
    GameObject localPointCastOrgFog;
    public GameObject orbFogAtack;
    [SerializeField]
    float orbFogAtackDelayGeneration;
    [SerializeField]
    float orbFogAtackDelayGenerationOnThirdPattern;

    //gera um delay na primeira invocação
    bool waitingForGeneration;

    
    
    [Header("controle de dano melee")]
    [SerializeField]
    float distanceFromPlayerToMeleeAtack;
    [SerializeField]
    FogKeeperMeleeAtackScript meleeDamageControl;
    [SerializeField]
    float bossMeleeDamage;
    [SerializeField]
    float bossMeleeImpulsePlayerForce;

    [SerializeField]
    float delayOfMelee;
    bool canDoAtackMelee = true;

    //Controle de animação
    Animator animControl;
    
    //variavel de distancia do boss com o player
    float distance;

    bool canSummonGroundTrops = true;

    //variavel usada para referncia de controle de rotação do boss
    public Transform bossPseudoCenter;

   
    [Header("Variavel que multiplica o dano ao boss enquanto no modo trigger")]
    [Range(1f,10f)]
    public float percentualDefenseDesirableWhenBossIsStaggered;

    [SerializeField]
    float bossStaggerTime;

    //variaveis que gravam as defesas percentuais do boss para serem trocadas durante o stagger
    float recorderedBossPhysical;
    float recorderedBossFire;
    float recorderedBossDark_Void;
    float recorderedBossIce;

    


    //dano de fog
    [Header("dano da fog da saia")]
    public float damageUnderFog;
    public float delayTimeForFogUnderSkirtDamage;

    //configurado no codigo UnderFogDamageScript
    public GameObject fogUnderSkirtControl;







    [Header("ataque de raio")]
    [SerializeField]
    float maxLineDistance;

    LineRenderer rayline;
    Transform lineEndPoint;
    Transform lineStartPoint;
    //posição atual da linha
    float atualLineDistance;
    //tempo de aceleração da barra ate chegar no ponto maximo
    [SerializeField]
    float timeUntilLineReachMaxDistance;

    bool castingRay = false;

    [SerializeField]
    float rayTimeUntilStop;

    [SerializeField]
    GameObject[] raySensors;

    //layer que o raio acerta
    public LayerMask deathRayLayerCheck;

    bool deathRayHitingPlayer = false;

    [SerializeField]
    float rayAtackRate;
    [SerializeField]
    float damageOfRayAtack;

    Vector3 playerCalculatedPosition;

    [SerializeField]
    float raySummonTime;
    bool startedRayCallLock = false;



   

    

    //sensores 




    //inicialização do boss
    private void Awake()
    {
        fogKeeperBoss = this.gameObject;
    }

    void Start()
    {
        loadingBoss = true;
        loadBoss();
    }

    void Reset()
    {
        loadingBoss = true;
        loadBoss();
    }

    void loadBoss()
    {
        
        //reinicia colisao com player
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("player") + 1, false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("inimigo"), LayerMask.NameToLayer("inimigo")+1);

        canSummonGroundTrops = true;
       

        //procura o player
        player = MovementScript.player.gameObject;

        //procura componentes nele mesmo
        bossStatus = GetComponent<EnemyStatus>();
        animControl = GetComponent<Animator>();

        //desliga valor de morte
        animControl.SetBool("BossIsDead", false);
        bossDead = false;

        //configura o valor do gravador de vida
        lifeRecorder = bossStatus.life;

        //coloca o nome do chefe como o nome do gameobject casto esteja vazio
        if (bossName == null || bossName == "")
        {
            bossName = gameObject.name;
        }

        //liga o primeiro ponto fraco e desliga os demais
        weakPoint[0].SetActive(true);
        weakPoint[0].GetComponent<WeakPointFogKeeperScript>().RiseSelfLight();

        weakPoint[1].SetActive(false);
        weakPoint[2].SetActive(false);

        

        //configura o valor do dano e impulso ao player no codigo do ataque melee e o habilita para ser usado
        meleeDamageControl.damageCasted = bossMeleeDamage;
        meleeDamageControl.impulseForce = bossMeleeImpulsePlayerForce;
        canDoAtackMelee = true;

        //configura o ataque de raio 
        startedRayCallLock = false;
        rayline = GetComponent<LineRenderer>();
        lineEndPoint = new GameObject().transform;
        //lineEndPoint.name = "lineEndPoint";
        lineStartPoint = new GameObject().transform;
        //lineStartPoint.name = "lineStartPoint";
        lineEndPoint.transform.position = staffPoint.position;
        castingRay = false;
        rayline.enabled = false;

        bossHealthControler = BossHealthScript.bossLifeBar.GetComponent<BossHealthScript>();
        bossHealthControler.boss = gameObject;
        bossHealthControler.bossName = bossName;

        loadingBoss = false;
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if(loadingBoss == false)
        {
            Invoke("delayedStart", Time.deltaTime * 2);
        }
    }

    //configurção de variaveis que precisam ser feitas depois do start mas antes do update
    void delayedStart()
    {
        //print(bossHealthControler);
        //liga a barra de vida
        bossHealthControler.gameObject.SetActive(true);
        bossHealthControler.LoadBossLifBar();

        //configura pattern
        atualPattern = 1;

        //carrega os ponstos fracos
        //UpdateWeakPoint(0,false);

        //configura estado atual de stagger;
        bossIsStaggered = false;

        //inicia o verificação temporaria da orientação do boss
        DelayCheckRotation();

        weakPointDestroyed = false;

        //inicia o spawn de inimigos
        waitingForGeneration = true;
        //StartCoroutine(GenerateRandomGhost());

        canInvokeghost = true;

        //inicia a invocação dos fantasmas
        Invoke("ReadyToInvokeAnyGhost", 1.25f);

        fogUnderSkirtControl.SetActive(false);

        
    }




    //atualiza os pontos fracos 
    public void UpdateWeakPoint(int selfArrayCount,bool teleport, int pattern,GameObject wallToDesactivate)
    {
        atualPattern = pattern;

        if (selfArrayCount != 2)
        {
            GameObject toActive = weakPoint[selfArrayCount + 1].gameObject;
            toActive.SetActive(true);
            toActive.GetComponent<WeakPointFogKeeperScript>().RiseSelfLight();
        }

        GameObject toDesactive = weakPoint[selfArrayCount];
        toDesactive.SetActive(false);


        if (teleport == true)
        {
            animControl.SetBool("BossTeleportation", true);
        }

        nextWallToDesactivate = wallToDesactivate;
    }

    void Update()
    {

        if (loadingBoss == false)
        {
            // verifica se perdeu vida
            if (lifeRecorder != bossStatus.life)
            {
                LifeCheck();
            }

            //mata o chefe caso esteja sem vida
            if (bossStatus.life <= 0 || bossDead == true)
            {
                //morte
                bossDead = true;

                //para tudo
                CancelInvoke();
                StopAllCoroutines();

                animControl.SetBool("BossIsDead", true);
            }
            else
            {
                //executa ataque melee caso o player esteja muito perto
                distance = Vector2.Distance(transform.position, player.transform.position);

                if (distance < distanceFromPlayerToMeleeAtack && canDoAtackMelee == true)
                {
                    animControl.SetBool("MeleeAtack", true);
                    canDoAtackMelee = false;
                }
                
            }
        }
    }

    void ResetAtackMelee()
    {
        canDoAtackMelee = true;
    }


    public void BossDeathFirstFrame()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("player") + 1);
    }




    //desliga o parametro de ataque melee no fim da animação
    public void BossAtackMeleeEndFrame()
    {
        animControl.SetBool("MeleeAtack", false);
        Invoke("ResetAtackMelee", delayOfMelee);
    }




    //quando o boss perder vida essa funçao sera executada, ela calcula o dano tomado e configura animção de dano apartir disso
    void LifeCheck()
    {
        //limita o do dano para ser positivo e o adiciona a varivel de delta
        deltaLost += Mathf.Abs(lifeRecorder - bossStatus.life);

        //caso seja maior que o trigger de dano, toca a animação de dano;
        if (deltaLost >= damageTriggerByDamage)
        {
            //força animação de dano apos uma certa quantidade x de dano
            //animControl.SetBool("Parametro de animação de dano",true) - exemplo
            CallCheckPattern();
            deltaLost = 0;
        }

        //atualiza o life recorder para parar de executar esta condicional;
        lifeRecorder = bossStatus.life;
    }



    //ATAQUE DE RAIO
    
    // inicial o parametro para animação do raio
    private void RayCall()
    {
        animControl.SetBool("CastingRay", true);
    }

    //chamado via animaçao (inicia a execução do render do raio e verificação de dano)
    public void StartBossCastRay()
    {
        rayline.enabled = true;
        
        //calcula direção do raio
        playerCalculatedPosition = (player.transform.position + new Vector3(0, -1.4f, 0));

        castingRay = true;
        CallRayDamage();
        CheckRayHittingPlayer();
        Invoke("EndBossCastRay", rayTimeUntilStop);
    }

    //chamado via animaçao finaliza a execução do raio
    public void EndBossCastRay()
    {
        
        if (lineEndPoint.position == lineStartPoint.position)
        {
            lineEndPoint.position = Vector3.MoveTowards(lineEndPoint.position, lineStartPoint.position, (maxLineDistance / timeUntilLineReachMaxDistance) * Time.deltaTime);
            Invoke("EndBossCastRay", Time.deltaTime);
        }
        else
        {
            castingRay = false;
            animControl.SetBool("CastingRay", false);
            rayline.enabled = false;
            Invoke("RayCall", raySummonTime);
        }
    }

    //verifica se o raio acertou o player e movimenta o raio (do seu inicio ate uma posição x que cruza o caminho do player)
    void CheckRayHittingPlayer()
    {
        if (castingRay == true)
        {
            //variavel de raycast para descobrir se esta acertando o player;
            RaycastHit2D hit;
            deathRayHitingPlayer = false;

            //calcula aonde o raio deveria estar (fazum delay para atingiar a distancia maxima)
            int dir = Mathf.RoundToInt(Mathf.Clamp(bossPseudoCenter.transform.position.x - player.transform.position.x, -1, 1));

            
            atualLineDistance = Mathf.MoveTowards(atualLineDistance, maxLineDistance, (maxLineDistance / timeUntilLineReachMaxDistance) * Time.deltaTime);

            //gera o render do raio
            lineStartPoint.transform.position = staffPoint.position;
            rayline.SetPosition(1, lineStartPoint.position);
            lineEndPoint.transform.position = (playerCalculatedPosition - lineStartPoint.position) * atualLineDistance;
            rayline.SetPosition(0, lineEndPoint.position);

            for (int I = 0; I < raySensors.Length; I++)
            {
                hit = Physics2D.Linecast(raySensors[I].transform.position, lineEndPoint.position, deathRayLayerCheck);
               
                if (hit)
                {
                    Debug.DrawLine(raySensors[I].transform.position, hit.point, Color.cyan);
                    if (hit.collider.gameObject == player)
                    {
                        deathRayHitingPlayer = true;
                    }
                }
            }

            Invoke("CheckRayHittingPlayer", Time.deltaTime);
        }
    }

    //executa esta funçao para verificar se o player esta sendo atingido e da dano se for sim;
    void CallRayDamage()
    {
        if(castingRay == true)
        {
            if (deathRayHitingPlayer == true)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(Mathf.RoundToInt(damageOfRayAtack),EnemyStatus.damageTypes.void_dark);
            }

            Invoke("CallRayDamage", rayAtackRate);
        }
        else
        {
            print("ended");
        }
    }





    //STAGGER

    //executa esse void quando staggeado (ativado via script do ponto fraco)
    public void StaggerStart()
    {
        //trava os eventos do boss
        //Cancela todos os invokes e co-rotinas da cena da cena

        CancelInvoke();
        StopAllCoroutines();


        //essa linha desliga o controlador de animação, substituir por animação de dano ou trigger
        animControl.SetTrigger("StaggerBegin");

        //grava os estados das defesas do boss e as substitui por defesas mais fracas temporariamente
        recorderedBossPhysical = bossStatus.physicDefensePercent;
        recorderedBossFire = bossStatus.fireDefensePercent;
        recorderedBossIce = bossStatus.iceDefensePercent;
        recorderedBossDark_Void = bossStatus.voidDefensePercent;

        bossStatus.physicDefensePercent = percentualDefenseDesirableWhenBossIsStaggered;
        bossStatus.fireDefensePercent = percentualDefenseDesirableWhenBossIsStaggered;
        bossStatus.iceDefensePercent = percentualDefenseDesirableWhenBossIsStaggered;
        bossStatus.voidDefensePercent = percentualDefenseDesirableWhenBossIsStaggered;
        bossIsStaggered = true;


        //chama a função de recuperação do boss
        Invoke("StaggerRecuperation", bossStaggerTime);
    }


    //reativa o boss apos o stagger;
    void StaggerRecuperation()
    {
        //para a animação de stagger
        animControl.SetTrigger("StaggerEnd");

        //volta as defesas ao normal
        bossStatus.physicDefensePercent = recorderedBossPhysical;
        bossStatus.fireDefensePercent = recorderedBossFire; 
        bossStatus.iceDefensePercent = recorderedBossIce;
        bossStatus.voidDefensePercent = recorderedBossDark_Void;

        //religa os eventos do boss
        CallCheckPattern();
        Invoke("DelayCheckRotation",1.25f);
        //Invoke("GenerateRandomGhost",2.1f);
        waitingForGeneration = true;

        //inicia o ataque de orbe
        Invoke("CallCastOrb", 1f);
       


        canDoAtackMelee = true;




        //aqui o boss teletransporta
        //animControl.SetBool("Parametro de animação de teletransporte",true) - exemplo

        animControl.SetBool("shouldSummon", true);

        bossIsStaggered = false;
    }

    //verifica qual o padrao atual o inimigo deve seguir - chamado via animação ou apos certa quantidade x de dano
    public void CallCheckPattern()
    {
        
        if(atualPattern == 3)
        {
            //inicia o ataque de raio
            if (startedRayCallLock == false)
            {
                animControl.SetBool("CastingRay", true);
            }
        }
        
            
      
    }







    //gera inimgos

    public IEnumerator GenerateRandomGhost()
    {
        float delayUnitilNextGeneration = BaseDelay;

        //seleciona randomicamente a geração de inimigo 

        int selection = -1;
        //nota sobre o range vai de 0 a 4 (5 itens, 1 a mais do que a quantidade) pois quando int, o range max e exclusivo e nao inclusivo
        selection = Random.Range(0, 100);
        
        //verifica distancia com o player para invocar tropas terrestrs
        float range = Vector2.Distance(bossPseudoCenter.position, MovementScript.player.transform.position + new Vector3(0, -1.4f));
        
        if (canSummonGroundTrops == true)
        {
            //25%
            if (selection > 15 && selection <= 40 && range <= atualRangeForCloseCombatGhostSummons)
            {
                //gera warrior - abaixo da saia do boss (remover o // quando tiver o prefab)
                delayUnitilNextGeneration = warriorDelayGeneration;

                //gera 1 caso padrao 1
                if(atualPattern == 1)
                {
                    int local = Random.Range(0, localsForGroundInstance.Length);

                    StartCoroutine(FogGeneration(localsForGroundInstance[local], fogAmount, fogDelayCreation));
                    yield return new WaitForSeconds(fogDelayCreation);
                    Instantiate(warriorPrefab, localsForGroundInstance[local].position, staffPoint.rotation);
                }
                else
                {
                    //gera 2 caso padrao 2 ou 3
                    int local = Random.Range(0, localsForGroundInstance.Length - 2);

                    StartCoroutine(FogGeneration(localsForGroundInstance[local], fogAmount, fogDelayCreation));
                    yield return new WaitForSeconds(fogDelayCreation);
                    Instantiate(warriorPrefab, localsForGroundInstance[local].position, staffPoint.rotation);

                    local += 1;

                    StartCoroutine(FogGeneration(localsForGroundInstance[local+1], fogAmount, fogDelayCreation));
                    yield return new WaitForSeconds(fogDelayCreation);
                    Instantiate(warriorPrefab, localsForGroundInstance[local+1].position, staffPoint.rotation);
                }



                Invoke("UnlockGroundTropsDelayed", warriorDelayGeneration);
                canSummonGroundTrops = false;

            }//25%
            else if (selection > 45 && selection <= 60 && range <= atualRangeForCloseCombatGhostSummons)
            {
                //gera grabber - abaixo da saia do boss (remover o // quando tiver o prefab)
                delayUnitilNextGeneration = grabberDelayGeneration;

                if (atualPattern < 3)
                {
                    //gara 1 caso padrao 1 ou 2
                    int local = Random.Range(0, localsForGroundInstance.Length);

                    StartCoroutine(FogGeneration(localsForGroundInstance[local], fogAmount, fogDelayCreation));
                    yield return new WaitForSeconds(fogDelayCreation);
                    Instantiate(grabberPrefab, localsForGroundInstance[local].position, staffPoint.rotation);
                }
                else
                {
                    //gera 2 caso no 3º padrao
                    int local = Random.Range(0, localsForGroundInstance.Length - 2);

                    StartCoroutine(FogGeneration(localsForGroundInstance[local], fogAmount, fogDelayCreation));
                    yield return new WaitForSeconds(fogDelayCreation * 2f);
                    Instantiate(grabberPrefab, localsForGroundInstance[local].position, staffPoint.rotation);

                    local += 2;

                    StartCoroutine(FogGeneration(localsForGroundInstance[local + 1], fogAmount, fogDelayCreation));
                    yield return new WaitForSeconds(fogDelayCreation);
                    Instantiate(warriorPrefab, localsForGroundInstance[local + 1].position, staffPoint.rotation);
                }

                Invoke("UnlockGroundTropsDelayed", grabberDelayGeneration);
                canSummonGroundTrops = false;


            }//25%
            else if (selection > 65 && selection <= 90 && range <= atualRangeForCloseCombatGhostSummons)
            {
                //gera charger- abaixo da saia do boss (remover o // quando tiver o prefab)
                delayUnitilNextGeneration = chargerDelayGeneration;

                int local = Random.Range(0, localsForGroundInstance.Length);

                StartCoroutine(FogGeneration(localsForGroundInstance[local], fogAmount, fogDelayCreation));
                yield return new WaitForSeconds(fogDelayCreation);
                Instantiate(chargerPrefab, localsForGroundInstance[local].position, staffPoint.rotation);


                Invoke("UnlockGroundTropsDelayed", chargerDelayGeneration);
                canSummonGroundTrops = false;
            }

        }


        StartCoroutine(SummonSkulls());

        //trava invocação
        animControl.SetBool("shouldSummon", false);
    }

    //gera flyer skull
    IEnumerator SummonSkulls()
    {
        //tempo que sera adicionado ao delay- quanto mais caveiras, maior o delay
        float timeAdd = 0;

        //faz o calculo de quantas caveiras serao gerada
        int amount =  Mathf.Clamp(Mathf.RoundToInt((Mathf.Abs(bossStatus.life - bossStatus.maxLife))/600) + 1,1,5);

        for (int i = 0; i < amount; i++)
        {
            //gera fog na staff do boss (se possivel otimizar usando particular)
            StartCoroutine(FogGeneration(flyerGenerationPoints[i], fogAmount, fogDelayCreation));

            yield return new WaitForSeconds(fogDelayCreation);

            Instantiate(flyingSkullPrefab, flyerGenerationPoints[i].position, flyerGenerationPoints[i].rotation);

            yield return new WaitForSeconds(flyingSkullDelayGenerationBetweenSkulls - fogDelayCreation);
            timeAdd += 0.5f;
            
        }

        
        Invoke("ReadyToInvokeAnyGhost", BaseDelay + timeAdd);
    }

    void UnlockGroundTropsDelayed()
    {
        canSummonGroundTrops = true;
    }


    void ReadyToInvokeAnyGhost()
    {
        animControl.SetBool("shouldSummon", true);
    }



    
    //gera fog de vfx
    IEnumerator FogGeneration(Transform local, int amount,float delay)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(fogTrail, local.position, local.rotation);
            yield return new WaitForSeconds(delay);
        }

    }


    public void StartCastFogVFX()
    {
        StartCoroutine(FogGeneration(staffPoint, fogAmount, fogDelayCreation));
    }




    //gera ataque de orb
    void CallCastOrb()
    {
        animControl.SetBool("CastOrb", true);
    }


    public void CastFogOrb()
    {
        Instantiate(orbFogAtack, localPointCastOrgFog.transform.position, orbFogAtack.transform.rotation);
        animControl.SetBool("CastOrb", false);
        if(atualPattern == 2)
        {
            Invoke("CallCastOrb", orbFogAtackDelayGeneration);
        }
        else if (atualPattern == 3)
        {
            Invoke("CallCastOrb", orbFogAtackDelayGenerationOnThirdPattern);
        }
        
    }
    









    //ideal é chamar esta função como evento no fim da animação de teletransporte 
    public void BossTeleport()
    {
        bossPseudoCenter.transform.position = bossNewPositionOnTeleport;

        if (nextWallToDesactivate != null)
        {
            nextWallToDesactivate.SetActive(false);
        }
        else
        {
            print("falta o ponto fraco");
        }


        animControl.SetBool("BossTeleportation", false);
    }

    //verifica a posição do player e rotaciona o boss de acordo com o valor (chamado via )
    public void DelayCheckRotation()
    {
        if (castingRay == false)
        {
            //rotaciona o boss de acordo com a posiçao do player
            if (bossPseudoCenter.transform.position.x > MovementScript.player.transform.position.x)
            {
                bossPseudoCenter.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (bossPseudoCenter.transform.position.x < MovementScript.player.transform.position.x)
            {
                bossPseudoCenter.transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        

        //previne que seja chamada mais de uma vez ja que tambem esta sendo chamada via animação
        if (IsInvoking("DelayCheckRotation") == false)
        {
            Invoke("DelayCheckRotation", 2.25f);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.gameObject == MovementScript.player)
        {
            
        }
    }



    public void OnBossDeath()
    {
        player.GetComponent<PlayerCameraControl>().ResetOnBossFightEnd();
        bossHealthControler.UnableLifeBar();

        gameObject.SetActive(false);
    }


}
