using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour
{
    public GameObject popUpDamage;

    public bool canKnockBack;

    public float maxLife;
    public bool inDamage;
    public Image lifeBar;
    [HideInInspector]
    public HealthBarScript gradientHealthBar;

    public float popUpHeightPlus = 1;

    [Header("Piscar ao receber Dano")]
    public SpriteRenderer visualInimigo;
    public Material materialHit;
    public Material materialPadrao;

    [Header("Variavel que diz se o pop up sera gerada")]
    public bool willCastPopUp = true;

    [Header("Variavel que atualiza barra de vida")]
    public bool willCastBarChange = true;


    //tipos de dano
    public enum damageTypes
    {
        physical, thunder, fire, void_dark, ice
    }

    [Header("Defesas")]
    public float physicDefense;
    [Range(0f, 5f)]
    public float physicDefensePercent = 1;

    public float thunderDefense;
    [Range(0f, 5f)]
    public float thunderDefensePercent = 1;

    public float voidDefense;
    [Range(0f, 5f)]
    public float voidDefensePercent = 1;

    public float fireDefense;
    [Range(0f, 5f)]
    public float fireDefensePercent = 1;

    public float iceDefense;
    [Range(0f, 5f)]
    public float iceDefensePercent = 1;

    float calculatedDamage = -1;


    //[HideInInspector]
    public float life;

    [HideInInspector]
    public bool canTakeDamage = true;

    void Start()
    {
        canTakeDamage = true;
        life = maxLife;

        if (willCastBarChange)
        {
            if(lifeBar != null)
            {
                lifeBar.fillAmount = life / maxLife;
                gradientHealthBar = lifeBar.GetComponent<HealthBarScript>();
                StatusBarUpdate();
            }
        }
    }

    //reload
    public void ReloadSBarStatus()
    {
        life = maxLife;

        if (willCastBarChange)
        {
            lifeBar.fillAmount = life / maxLife;
            gradientHealthBar = lifeBar.GetComponent<HealthBarScript>();
            StatusBarUpdate();
        }
    }

   
    //damage types "phy" fisico ou natural, "fir" fogo, "ice" gelo ou gelado, "voi" void ou dark
    public void CastDamage(int damage, damageTypes damageType)
    {
        if(visualInimigo!=null){
            StartCoroutine(piscarSprite());
        }

        //parametro de animaçao
        inDamage = true;


        float damageToReduce = DamageCalculation(damage, damageType);


        //reduz vida atraves do calculo de dano
        life -= damageToReduce;

        if (willCastBarChange)
        {
            StatusBarUpdate();
        }
        
        //
        if (willCastPopUp && damageToReduce > 0)
        {
            CastPopUp(damageToReduce);
        }
    }

    public void CastMultipleTypeDamage(Dictionary<string, int> damage){
        if(visualInimigo!=null){
            StartCoroutine(piscarSprite());
        }

        inDamage = true;

        float damageToReduce = 0;
        damageToReduce += DamageCalculation(damage["physic"], damageTypes.physical);
        damageToReduce += DamageCalculation(damage["thunder"], damageTypes.thunder);
        damageToReduce += DamageCalculation(damage["void"], damageTypes.void_dark);
        damageToReduce += DamageCalculation(damage["fire"], damageTypes.fire);
        damageToReduce += DamageCalculation(damage["ice"], damageTypes.ice);

        //reduz vida atraves do calculo de dano
        life -= damageToReduce;

        if (willCastBarChange)
        {
            StatusBarUpdate();
        }

        //
        if (willCastPopUp && damageToReduce > 0)
        {
            CastPopUp(damageToReduce);
        }
    }

    IEnumerator piscarSprite(){
        visualInimigo.material = materialHit;
        yield return new WaitForSeconds(0.1f);
        visualInimigo.material = materialPadrao;
    }

    //calculo de dano
    float DamageCalculation(float baseDamage, damageTypes damageTypeBase)
    {
        if (canTakeDamage)
        {
            //verifica o tipo de dano e o reduz com a defesa respectiva e porcentagem extra
            if (damageTypeBase == damageTypes.physical)
            {
                calculatedDamage = (baseDamage * physicDefensePercent) - physicDefense;
            }
            else if (damageTypeBase == damageTypes.fire)
            {
                calculatedDamage = (baseDamage * fireDefensePercent) - fireDefense;
            }
            else if (damageTypeBase == damageTypes.ice)
            {
                calculatedDamage = (baseDamage * iceDefensePercent) - iceDefense;
            }
            else if (damageTypeBase == damageTypes.void_dark)
            {
                calculatedDamage = (baseDamage * voidDefensePercent) - voidDefense;
            }
            else if (damageTypeBase == damageTypes.thunder)
            {
                calculatedDamage = (baseDamage * thunderDefensePercent) - thunderDefense;
            }
            else
            {
                //printa que o tipo de dano foi escrito errado
                print("Somenthing is not right");
            }
        }
        else
        {
            calculatedDamage = 0;
        }


        //clampa o valor para que seja zero ao inves de negativo e aumentar a vida ao inves de a reduzir
        calculatedDamage = Mathf.Clamp(calculatedDamage, 0, Mathf.Infinity);

        //print(calculatedDamage + " before pass");
        return calculatedDamage;
    }



    //instancia popup de dano com o dano dado
    public void CastPopUp(float popUpDamageValue)
    {
        if (willCastPopUp)
        {
            GameObject PopUpDanoVar = Instantiate(popUpDamage, lifeBar.transform.position + new Vector3(0, popUpHeightPlus, 0), popUpDamage.transform.rotation);
            PopUpDanoVar.GetComponent<DamagePopUpScritp>().SetDamageValue(Mathf.RoundToInt(popUpDamageValue));
        }
        
    }

    //atualiza barra de vida
    public void StatusBarUpdate()
    {
        if (willCastBarChange && lifeBar != null)
        {
            gradientHealthBar.gameObject.SetActive(true);
            lifeBar.fillAmount = life / maxLife;
            gradientHealthBar.SetHealthByImage(life, maxLife);
            calculatedDamage = 0;
        }
    }
}
