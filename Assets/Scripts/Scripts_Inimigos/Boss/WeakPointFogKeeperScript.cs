using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class WeakPointFogKeeperScript : MonoBehaviour
{
    FogKeeperScript bossScript;
    EnemyStatus myStatus;

    //varivel de intensidade desejada para quando a luz reiniciar
    float lightIntesityTarget;

    [SerializeField]
    float lightRiseSpeed;

    Light2D selfLight;

    // 1/4 de vida
    float percentOfValueDamageToBossWhenDestroy = 0.25f;

    [Header("proximo padrao do chefe")]
    public int patternOfBossOnDestroyThis;

    [Header("coordenadas de teleporte (o eixo z pode ser 0)")]
    public Vector3 bossTeleportPosition;

    [Header("nao modificar - posição de array dos pontos fracos do boss")]
    public int selfArrayCount;

    [Header("objeto de parede invisivel apra ser desligada quando o player destruir este ponto fraco")]
    [SerializeField]
    GameObject areaOfLiberationWhenThisWeakPointIsDestroyed;


    void OnEnable()
    {
        SelfLoad();
    }


    //carrega variaveis
    void SelfLoad()
    {
        //pega componentes de status e iluminação
        myStatus = GetComponent<EnemyStatus>();
        selfLight = GetComponent<Light2D>();

        //procura script do chefe no objeto em que esta parenteado -- o pai do objeto e o adiciona na lista de pontos fracos
        bossScript = GetComponentInParent<FogKeeperScript>();


        //define valor para mudar intensiade da luz para o valor colocado
        lightIntesityTarget = selfLight.intensity;

        //reseta a luz para tocar animação
        selfLight.intensity = 0;

        //liga parede invisivel
        if(areaOfLiberationWhenThisWeakPointIsDestroyed!= null)
        {
            areaOfLiberationWhenThisWeakPointIsDestroyed.SetActive(true);
        }
        else
        {
            print("falta colocar parede invisivel no ponto fraco " + selfArrayCount);
        }
        

        RiseSelfLight();
    }



    //void ascyncrono para executar movimentação de luz ate chegar no valor original
    public void RiseSelfLight()
    {
        if (selfLight.intensity != lightIntesityTarget)
        {
            selfLight.intensity = Mathf.MoveTowards(selfLight.intensity, lightIntesityTarget, lightRiseSpeed * Time.deltaTime);
            Invoke("RiseSelfLight", Time.deltaTime);
        }

    }


    void Update()
    {

        if (myStatus.life <= 0)
        {
            //remove 10% da vida do boss (referente a vida maxima e nao a atual) quando destruido
            //float deathOnBossDamage = ((bossScript.bossStatus.maxLife * 30) / 100);
            float deathOnBossDamage = (bossScript.bossStatus.maxLife * percentOfValueDamageToBossWhenDestroy);
            bossScript.bossStatus.life -= deathOnBossDamage;

            //instancia um popup com o dano calculado
            bossScript.bossStatus.CastPopUp(deathOnBossDamage);

            //atualiza vida do boss
            bossScript.bossStatus.StatusBarUpdate();

            //reduz a defesa do boss;
            bossScript.bossStatus.physicDefensePercent += 1f / 3f;


            //configura teleporte
            bossScript.bossNewPositionOnTeleport = bossTeleportPosition;
            bossScript.StaggerStart();

            if (selfArrayCount == 2)
            {
                bossScript.UpdateWeakPoint(selfArrayCount, false, patternOfBossOnDestroyThis, areaOfLiberationWhenThisWeakPointIsDestroyed);
            }
            else
            {
                bossScript.UpdateWeakPoint(selfArrayCount, true, patternOfBossOnDestroyThis,areaOfLiberationWhenThisWeakPointIsDestroyed);
            }

            gameObject.SetActive(false);
        }

        
       
       
    }

}
