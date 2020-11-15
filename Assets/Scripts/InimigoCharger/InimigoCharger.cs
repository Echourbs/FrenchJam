using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoCharger : MonoBehaviour
{
	[Header("InfoBasico")]
	public string tagDoPlayer = "Player";
	public string layerChao = "Default";
	public string layerParede = "Default";
	public Animator anim;
	//
	[Header("Detectores")]
	public Transform DetectarChao;
	public Transform DetectarChaoNaFrente;
	public Transform DetectarParede;
	[Header("AreaDetectarPlayer")]
	public Transform AreaDetectarPlayerMax;
	public Transform AreaDetectarPlayerMin;
	//
	[Header("Valores")]
	public float dano = 5f;
	public float velocidadePadrao = 2.5f;
	public float velocidadeCorrendo = 10f;
	public float tempoOlhandoParaParede = 1f;
	public float distanciaParaAtacar = 3.5f;
	public float tempoAtacando = 1f;

	//
	private Transform posPlayer;
	private float velocidade;
	private bool modoAgressivo;
	private float tempoDeAtaque;

	//Iniciar
    void Start()
    {
    	posPlayer = GameObject.FindWithTag(tagDoPlayer).transform;
    	//
    	velocidade = velocidadePadrao;
    }

    //Fazer por frame
    void FixedUpdate()
    {
    	//Mover Para Frente
        transform.Translate(Vector3.right*velocidade*Time.deltaTime);
    	
    	//Verificar Detectores
    	bool noChao = Physics2D.Linecast(transform.position, DetectarChao.position, 1<<LayerMask.NameToLayer(layerChao));
    	bool haChaoNaFrente = Physics2D.Raycast(DetectarChaoNaFrente.position, Vector3.forward, 1<<LayerMask.NameToLayer(layerChao));
        bool haParedeNaFrente = Physics2D.Raycast(DetectarParede.position, Vector3.forward, 1<<LayerMask.NameToLayer(layerParede));
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
    	else if(!estouVendoPlayer&&modoAgressivo){
    		modoAgressivo = false;
    		velocidade = velocidadePadrao;
    	}

    	//Virar para o Player quando estiver no Modo Agressivo
    	else if(modoAgressivo){
    		bool playerNaMinhaFrente = false;
    		playerNaMinhaFrente = transform.position.x<posPlayer.position.x&&AreaDetectarPlayerMax.position.x>AreaDetectarPlayerMin.position.x;
    		playerNaMinhaFrente = playerNaMinhaFrente||(transform.position.x>posPlayer.position.x&&AreaDetectarPlayerMax.position.x<AreaDetectarPlayerMin.position.x);
    		//
    		if(!playerNaMinhaFrente){
    			transform.Rotate(0f, 180f, 0f);
    		}
    		//
    		float distanciaDoPlayer = Vector2.Distance(transform.position, posPlayer.position);
    		if(distanciaDoPlayer<distanciaParaAtacar){
    			if(tempoDeAtaque<=0){
    				tempoDeAtaque = tempoAtacando;
    				StartCoroutine(Atacar());
    			}
    			velocidade = 0f;
    		}
    		else if(tempoDeAtaque<=0){
    			velocidade = velocidadeCorrendo;
    		}
    	}

    	//Quanto tempo falta para poder Atacar Novamente
    	if(tempoDeAtaque>0){ tempoDeAtaque-=Time.deltaTime;}

    	//Animacoes
    	anim.SetBool("Walk", velocidade==velocidadePadrao);
    	anim.SetBool("Run", velocidade==velocidadeCorrendo);
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
    	bool vendoEmY = posPlayer.position.y<AreaDetectarPlayerMax.position.y&&posPlayer.position.y>AreaDetectarPlayerMin.position.y;
    	bool vendoEmX = (posPlayer.position.x<AreaDetectarPlayerMax.position.x&&posPlayer.position.x>AreaDetectarPlayerMin.position.x)&&AreaDetectarPlayerMax.position.x>AreaDetectarPlayerMin.position.x;
    	vendoEmX = vendoEmX||((posPlayer.position.x>AreaDetectarPlayerMax.position.x&&posPlayer.position.x<AreaDetectarPlayerMin.position.x)&&AreaDetectarPlayerMax.position.x<AreaDetectarPlayerMin.position.x);
    	//
    	estouVendo = vendoEmX&&vendoEmY;
    	//
    	return estouVendo;
    }

    //Coloca a animação de Atacar
    IEnumerator Atacar(){
    	anim.SetBool("Atack", true);
    	yield return new WaitForSeconds(0.5f);
    	anim.SetBool("Atack", false);
    }
}
