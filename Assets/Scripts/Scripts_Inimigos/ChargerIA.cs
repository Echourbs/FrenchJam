using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;

public class ChargerIA : MonoBehaviour
{

    public GameObject BarraDeVida;
	[Header("Nome das Tags e Layers")]
	public string tagDoPlayer = "Player";
	public string layerChao = "Default";
	public string layerParede = "Default";
	//
	[Header("Detectores")]
	public Transform DetectarChao;
	public Transform DetectarChaoNaFrente;
	public Transform DetectarParede1;
	public Transform DetectarParede2;
	[Header("Area Detectar Player")]
	public Transform AreaDetectarPlayerMax;
	public Transform AreaDetectarPlayerMin;
	//
	[Header("Valores")]
	public int dano = 5;
	public float velocidadePadrao = 2.5f;
	public float velocidadeCorrendo = 10f;
	public float tempoOlhandoParaParede = 1f;
	public float distanciaParaAtacar = 3.5f;
	public float tempoAtacando = 1f;
    public float quantoTempoFicaEmDano = 1f;
	//
	[Header("Ataque")]
	public GameObject prefabDano;	
	public Transform ancoraAtaque;
    public float forcaEmpurrao = 600f;

	//
	private Transform player;
	private PlayerHealth vidaDoPlayer;
	private float velocidade;
	private bool modoAgressivo;
	private float tempoDeAtaque;
    private float emDano;
    private bool morto;
    [SerializeField]
    Transform centro;
    [SerializeField]
    float rangeStop,rangeMinMovivento;
    //
    private Animator anim;
    private EnemyStatus _enemyStatus;
    private Rigidbody2D rb;
    private Collider2D collider;

    
    [SerializeField]
    bool PodeReviver;

	//Iniciar
    void Start()
    {
    	player = MovementScript.player.transform;
    	vidaDoPlayer = player.gameObject.GetComponent<PlayerHealth>();
        //
        anim = GetComponent<Animator>();
        _enemyStatus = GetComponent<EnemyStatus>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    	//
    	velocidade = velocidadePadrao;
    }

    //Fazer por frame
    void Update()
    {
        if(!morto){
            if (Vector2.Distance(centro.position,player.position) > rangeMinMovivento)
            {
                //Mover Para Frente
                transform.Translate(Vector3.right * velocidade * Time.deltaTime);
            }
            else
            {
                //Mover Para Frente
                transform.Translate(Vector3.right * velocidade * Time.deltaTime / 4);
            }
    	
    	
    	//Verificar Detectores
    	bool noChao = Physics2D.Linecast(transform.position, DetectarChao.position, 1<<LayerMask.NameToLayer(layerChao));
    	bool haChaoNaFrente = Physics2D.Raycast(DetectarChaoNaFrente.position, Vector3.forward, 1<<LayerMask.NameToLayer(layerChao));
        bool haParedeNaFrente = Physics2D.Linecast(DetectarParede1.position, DetectarParede2.position, 1<<LayerMask.NameToLayer(layerParede));
        bool estouVendoPlayer = EstouVendoPlayer();

        //Caso Haja uma Parede ou Não haja Chao Na Frente, Parar e Virar Inimigo
    	if((!haChaoNaFrente||haParedeNaFrente)&&velocidade!=0&&!estouVendoPlayer){
    		StartCoroutine(PararEVirar());
    	}

    	//Entrar no Modo Agressivo (persegue o player e o ataca)
    	else if(estouVendoPlayer&&!modoAgressivo){
    		modoAgressivo = true;
    	}

    	//Sair do Modo Agressivo
    	else if(Vector2.Distance(transform.position,player.position) > rangeStop&&modoAgressivo){
    		modoAgressivo = false;
    		velocidade = velocidadePadrao;
    	}

    	//Virar para o Player quando estiver no Modo Agressivo
    	else if(modoAgressivo){
    		bool playerNaMinhaFrente = false;
    		playerNaMinhaFrente = transform.position.x<player.position.x&&AreaDetectarPlayerMax.position.x>AreaDetectarPlayerMin.position.x;
    		playerNaMinhaFrente = playerNaMinhaFrente||(transform.position.x>player.position.x&&AreaDetectarPlayerMax.position.x<AreaDetectarPlayerMin.position.x);
    		//
    		if(!playerNaMinhaFrente&&tempoDeAtaque<=0f&&emDano<=0){
    			transform.Rotate(0f, 180f, 0f);
    		}
    		//
    		float distanciaDoPlayer = Vector2.Distance(transform.position, player.position);
    		if(distanciaDoPlayer<distanciaParaAtacar){
    			if(tempoDeAtaque<=0&&emDano<=0){
                        if (!player.gameObject.GetComponent<MovementScript>().isDead)
                        {
                            tempoDeAtaque = tempoAtacando;
                            StartCoroutine(Atacar());
                        }
    				
    			}
    			velocidade = 0f;
    		}
    		else if(tempoDeAtaque<=0){
    			velocidade = velocidadeCorrendo;
    		}
    	}

    	//Quanto tempo falta para poder Atacar Novamente
    	if(tempoDeAtaque>0){ tempoDeAtaque-=Time.deltaTime;}

        //Quanto tempo falta para sair do Dano
        if(emDano>0){ emDano-=Time.deltaTime;}

        //Receber Dano
        if(_enemyStatus.inDamage){
            StartCoroutine(AoReceberDano());
        }

        //
        if(_enemyStatus.life<=0){
            emDano = 0f;
            //
            StartCoroutine(MorrerERenascer());
        }

    	//Animacoes
    	anim.SetBool("Walk", velocidade==velocidadePadrao);
    	anim.SetBool("Run", velocidade==velocidadeCorrendo);
        //
        }
    }

    //Inimigo Para, Espera, e Vira, voltando a Andar;
    IEnumerator PararEVirar(){
    	velocidade = 0f;
    	//
    	yield return new WaitForSeconds(tempoOlhandoParaParede);
    	//
    	transform.Rotate(0f, 180f, 0f);
    	velocidade = velocidadePadrao;
    }

    //Se o Player esta na Area de Deteccao
    bool EstouVendoPlayer(){
    	bool estouVendo = false;
    	//
    	bool vendoEmY = player.position.y<AreaDetectarPlayerMax.position.y&&player.position.y>AreaDetectarPlayerMin.position.y;
    	bool vendoEmX = (player.position.x<AreaDetectarPlayerMax.position.x&&player.position.x>AreaDetectarPlayerMin.position.x)&&AreaDetectarPlayerMax.position.x>AreaDetectarPlayerMin.position.x;
    	vendoEmX = vendoEmX||((player.position.x>AreaDetectarPlayerMax.position.x&&player.position.x<AreaDetectarPlayerMin.position.x)&&AreaDetectarPlayerMax.position.x<AreaDetectarPlayerMin.position.x);
    	//
    	estouVendo = vendoEmX&&vendoEmY;
    	//
    	return estouVendo;
    }

    //Coloca a animação de Atacar
    IEnumerator Atacar(){
    	//
    	anim.SetBool("Atack", true);
        //
        yield return new WaitForSeconds(0.25f);
        //
        if(emDano<=0){
            AttackInstance objDano = Instantiate(prefabDano, ancoraAtaque.position, Quaternion.Euler(0f, 0f, 0f)).GetComponent<AttackInstance>();
            objDano.master = gameObject;
            objDano.damage = AttackInstance.CreateDamageDictionary(dano);
            objDano.damageEffect.AddListener(Empurrao);
        }
        //
    	yield return new WaitForSeconds(0.25f);
        //
    	anim.SetBool("Atack", false);
    	//
    }

    //Quando o Inimigo Receber Dano, caso tenha morrido, tocar animação de morte e desativar tudo; caso tenha sobrevivido ao dano, animação e stun no inimigo
    IEnumerator AoReceberDano(){
        _enemyStatus.inDamage = false;
        emDano = quantoTempoFicaEmDano;
        anim.SetBool("Hit", true);
       	yield return new WaitForSeconds(0.5f);
        anim.SetBool("Hit", false);
        
    }

    IEnumerator MorrerERenascer(){
        morto = true;
        anim.SetBool("Die", true);
        BarraDeVida.SetActive(false);
        collider.enabled = false;
        rb.simulated = false;
        //
        if(!PodeReviver){ GetComponent<EnemyDrop>().Drop();}
        yield return new WaitForSeconds(5f);
        if (PodeReviver)
        {
            //
            _enemyStatus.life = _enemyStatus.maxLife;
            //
            rb.simulated = true;
            collider.enabled = true;
            anim.SetBool("Die", false);
            //
            yield return new WaitForSeconds(1f);
            BarraDeVida.SetActive(true);
            _enemyStatus.ReloadSBarStatus();
            //
            morto = false;
            PodeReviver = false;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    //Efeito do Ataque
    public void Empurrao(GameObject obj){
        MovementScript _movementScript = obj.GetComponent<MovementScript>();
        if(_movementScript!=null){ _movementScript.StartCoroutine(_movementScript.StunPlayer(0.5f));}
        //
        obj.GetComponent<Rigidbody2D>().AddForce(forcaEmpurrao*transform.right);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeStop);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centro.position, rangeMinMovivento);

    }
}
