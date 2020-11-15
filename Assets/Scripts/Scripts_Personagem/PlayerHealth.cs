using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //pop up de dano
    public GameObject popUpDamagePrefab;

    public int maxHealth = 100;

    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public bool inDamage;

    public HealthBarScript healthBar;

    public Image gradientDamage;

    [HideInInspector]
    public Animator anim;

    private bool locked = false;

    private MovementScript _movementScript;
    private Combat_Player _combatPlayer;
    private SistemaItens _sistemaItens;
    private Rigidbody2D rb;


    //bool diecurrentHealth;

    void Start()
    {
        if (healthBar != null)
        {
            anim = GetComponent<Animator>();

            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            print("Falta colocar a health bar");
        }
        

        _movementScript = GetComponent<MovementScript>();
        _combatPlayer = GetComponent<Combat_Player>();
        _sistemaItens = GetComponent<SistemaItens>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Neutralized" && _movementScript.isDead == false)
        {
            TakeDamage(5);
            anim.SetBool("Damage", true);
            //
            StartCoroutine(_movementScript.SimpleStunPlayer(0.1f));
            StartCoroutine(_movementScript.AtivarInvuneravel(2f));
            //
            Vector3 direcao = transform.position-c.transform.position;
            direcao = new Vector3(Mathf.Clamp(direcao.x, -1f, 1f), Mathf.Clamp(direcao.y, -1f, 1f), 0f);
            rb.velocity = direcao*20f;
            //
            _movementScript.posYMaxima = transform.position.y;
        }
        
    }

    public void TakeDamage(int damage, EnemyStatus.damageTypes damageType = EnemyStatus.damageTypes.physical, Vector2 direcaoDano = new Vector2())
    {
        float damageToReduce = DamageCalculation(damage, damageType, direcaoDano);
        if (damageToReduce > 0)
        {
            StartCoroutine(_movementScript.AtivarInvuneravel(2f));
            //
            anim.SetBool("Damage", true);
            //instancia popup quando acertar o inimigo
            GameObject popUp = Instantiate(popUpDamagePrefab, transform.position, popUpDamagePrefab.transform.rotation);
            popUp.GetComponent<DamagePopUpScritp>().SetDamageValue((int)damageToReduce);
            //desparenteia
            popUp.transform.parent = null;
            //reseta a posição
            popUp.transform.eulerAngles = Vector3.zero;
            //
            StartCoroutine(Gradiente());
            //
            CameraController.Damage();
        }


        currentHealth -= (int)damageToReduce;

        healthBar.SetHealth(currentHealth);

        if (currentHealth  <= 0 && locked == false)
        {
            _movementScript.DiePlayer();
            locked = true;
            
        }       
    }

    public void TakeMultipleTypeDamage(Dictionary<string, int> damage, Vector2 direcaoDano = new Vector2()){
        float damageToReduce = 0;
        damageToReduce += DamageCalculation(damage["physic"], EnemyStatus.damageTypes.physical, direcaoDano);
        damageToReduce += DamageCalculation(damage["thunder"], EnemyStatus.damageTypes.thunder, direcaoDano);
        damageToReduce += DamageCalculation(damage["void"], EnemyStatus.damageTypes.void_dark, direcaoDano);
        damageToReduce += DamageCalculation(damage["fire"], EnemyStatus.damageTypes.fire, direcaoDano);
        damageToReduce += DamageCalculation(damage["ice"], EnemyStatus.damageTypes.ice, direcaoDano);
        //
        if (damageToReduce > 0)
        {
            StartCoroutine(_movementScript.AtivarInvuneravel(2f));
            //
            anim.SetBool("Damage", true);
            //instancia popup quando acertar o inimigo
            GameObject popUp = Instantiate(popUpDamagePrefab, transform.position, popUpDamagePrefab.transform.rotation);
            popUp.GetComponent<DamagePopUpScritp>().SetDamageValue((int)damageToReduce);
            //desparenteia
            popUp.transform.parent = null;
            //reseta a posição
            popUp.transform.eulerAngles = Vector3.zero;
            // StartCoroutine(GetComponent<CameraController>().AmpliarPorTempo(0.01f, -12f));
            //
            StartCoroutine(Gradiente());
            //
            CameraController.Damage();
        }


        currentHealth -= (int)damageToReduce;

        healthBar.SetHealth(currentHealth);

        if (currentHealth  <= 0 && locked == false)
        {
            _movementScript.DiePlayer();
            locked = true;
            
        } 
    }

    //calculo de dano
    float DamageCalculation(float baseDamage, EnemyStatus.damageTypes damageTypeBase, Vector2 direcaoDano)
    {
        Item arma1 = _sistemaItens.itensMao[0].item;
        int tipoArma1 = 0;
        if(arma1!=null){ tipoArma1 = arma1.tipoArmaToInteger();}
        Item arma2 = _sistemaItens.itensMao[1].item;
        int tipoArma2 = 0;
        if(arma2!=null){ tipoArma2 = arma2.tipoArmaToInteger();}
        //
        Item escudo = _sistemaItens.itensMao[_combatPlayer.slotItemUsando].item;
        //
        Item arma = new Item();
        if((arma1!=null&&tipoArma1!=1&&tipoArma1!=2&&tipoArma1!=3)&&
            (arma2!=null&&tipoArma2!=1&&tipoArma2!=2&&tipoArma2!=3)){
            //
            arma.physicDefense = (arma1.physicDefense+arma2.physicDefense)/2f;
            arma.thunderDefense = (arma1.thunderDefense+arma2.thunderDefense)/2f;
            arma.fireDefense = (arma1.fireDefense+arma2.fireDefense)/2f;
            arma.voidDefense = (arma1.voidDefense+arma2.voidDefense)/2f;
            arma.iceDefense = (arma1.iceDefense+arma2.iceDefense)/2f;
        }
        else if((arma1!=null&&tipoArma1!=1&&tipoArma1!=2&&tipoArma1!=3)&&
            !(arma2!=null&&tipoArma2!=1&&tipoArma2!=2&&tipoArma2!=3)){
            //
            arma = arma1;
        }
        else if(!(arma1!=null&&tipoArma1!=1&&tipoArma1!=2&&tipoArma1!=3)&&
            (arma2!=null&&tipoArma2!=1&&tipoArma2!=2&&tipoArma2!=3)){
            //
            arma = arma1;
        }
        //
        float calculatedDamage = -1;
        bool escudoApontadoInimigo = _combatPlayer.shield&&direcaoDano!=Vector2.zero&&
            ((transform.position.x<=direcaoDano.x&&transform.right.x==1)||
            (direcaoDano.x<=transform.position.x&&transform.right.x==-1));
        //

        switch(damageTypeBase){
            case EnemyStatus.damageTypes.physical:
                if(arma!=null){ calculatedDamage = baseDamage-arma.physicDefense;}
                else{ calculatedDamage = baseDamage;}
                if(escudoApontadoInimigo){
                    calculatedDamage = calculatedDamage-escudo.physicDefense;
                }
                break;
            case EnemyStatus.damageTypes.thunder:
                if(arma!=null){ calculatedDamage = baseDamage-arma.thunderDefense;}
                else{ calculatedDamage = baseDamage;}
                if(escudoApontadoInimigo){
                    calculatedDamage = calculatedDamage-escudo.thunderDefense;
                }
                break;
            case EnemyStatus.damageTypes.fire:
                if(arma!=null){ calculatedDamage = baseDamage-arma.fireDefense;}
                else{ calculatedDamage = baseDamage;}
                if(escudoApontadoInimigo){
                    calculatedDamage = calculatedDamage-escudo.fireDefense;
                }
                break;
            case EnemyStatus.damageTypes.void_dark:
                if(arma!=null){ calculatedDamage = baseDamage-arma.voidDefense;}
                else{ calculatedDamage = baseDamage;}
                if(escudoApontadoInimigo){
                    calculatedDamage = calculatedDamage-escudo.voidDefense;
                }
                break;
            case EnemyStatus.damageTypes.ice:
                if(arma!=null){ calculatedDamage = baseDamage-arma.iceDefense;}
                else{ calculatedDamage = baseDamage;}
                if(escudoApontadoInimigo){
                    calculatedDamage = calculatedDamage-escudo.iceDefense;
                }
                break;
            default:
                print("Somenthing is not right");
                break;
        }

        //
        if(escudoApontadoInimigo){
            CameraController.Hit();
        }
        //
        //clampa o valor para que seja zero ao inves de negativo e aumentar a vida ao inves de a reduzir
        calculatedDamage = Mathf.Clamp(calculatedDamage, 0, Mathf.Infinity);

        //print(calculatedDamage + " before pass");
        return calculatedDamage;
    }

    IEnumerator Gradiente(){
        gradientDamage.enabled = true;
        yield return new WaitForSeconds(0.1f);
        gradientDamage.enabled = false;
    }

}
