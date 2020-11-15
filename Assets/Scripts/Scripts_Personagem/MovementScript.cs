using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//__________________________________________
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Combat_Player))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(SistemaItens))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
//__________________________________________
public class MovementScript : MonoBehaviour
{
	//__________________________________________________________________________________________________________________________________________________________________
	//Variaveis

	//__________________________________________
	/*- Static -*/
	public static GameObject player;

	//__________________________________________
	/*- Inspector -*/
	[Header("Fenix")]
	public GameObject prefabFenix;
	public Transform ancoraFenix;
	public Transform eggFenix;
	[Header("Movementação Horizontal")]
	public float speed = 10; 
	[Header("Jump")]
	public float jumpForce;
	public float maxPhysicVelocityY;
	public float minPhysicVelocityY;
	[Header("Roll - Dodge")]
    public float velocityMovementRolling;
	[Header("Climb")]
	public float climbVerticalSpeed;
    public LayerMask ladderLayer;
    public LayerMask scalableLayer;
    [Header("Visual")]
    public Transform visualTransform;
    public SpriteRenderer meuSprite;
    public GameObject prefabSpriteVazio;
	[Header("Colliders")]
    public CapsuleCollider2D baseCollider;
    public CapsuleCollider2D colliderOnRoll;
    [Header("Sensores")]
    public LayerMask detectionLayerGround;
    public Transform[] groundSensors;
    public Transform[] topSensors;
    public Transform[] wallSensors;
    public Transform[] sensorBorda;

    //__________________________________________
	/*- Public -*/
	//Geral
	[HideInInspector]
	public bool isPlaying = true;
	[HideInInspector]
	public bool isDead = false;
	[HideInInspector]
	public bool stun = false;
	//Chao
    [HideInInspector]
	public bool onGround;
	[HideInInspector]
	public float raycastMinimumHeight;
	//Estado
	[HideInInspector]
	public float isRoling;
    [HideInInspector]
    public bool usingCrounch;
    [HideInInspector]
    public bool penduradoBorda;
    [HideInInspector]
    public bool climbingLadder;
	[HideInInspector]
	public bool climbing;
    [HideInInspector]
    public bool invuneravel;
    [HideInInspector]
    public float posYMaxima;
    //Efeitos
    [HideInInspector]
    public int numeroMonstersUsado;

    //__________________________________________
	/*- Private -*/
	private string gameScene;
	private GameObject fenix;
	private float move;
    //Jump
	private bool doubleJump;
    private bool jumpBuffer;
	//Roll
    private bool RolingClick;
    private float tempoRoll;
    //Pendurado
    private Vector3 posBorda;
    private bool v_inputCima;
    //
    private bool v_onGround;
	private float originalGravity;

	//__________________________________________
	/*- Components -*/
	private Rigidbody2D rb;
	private AudioSource _audio;
    private Animator animator;
	private Combat_Player _combatPlayer;
    private PlayerHealth _playerHealth;
	private SistemaItens _sistemaItens;

	//__________________________________________________________________________________________________________________________________________________________________
	//Funcoes Base

	//essa linha diz que o objeto global Player é este objeto - para reduzir o uso de FIND e Tags para este objeto
    void Awake()
    {
        player = this.gameObject;
    }

	void Start(){
		//Get Components
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		_audio = GetComponent<AudioSource>();
		_combatPlayer = GetComponent<Combat_Player>();
		_sistemaItens = GetComponent<SistemaItens>();

		//variaveis para guardar
		gameScene = SceneManager.GetActiveScene().name;
		originalGravity = rb.gravityScale;

		//Fenix
		fenix = Instantiate(prefabFenix, new Vector3(eggFenix.position.x, eggFenix.position.y + 2.67f, eggFenix.position.z), ancoraFenix.transform.rotation);
        fenix.SetActive(false);
	}

	void Update(){
		/* - Deteccao Input - */
		float axisHorizontal = 0;
		float axisVertical = 0;
		bool inputJump = false;
		bool inputRoll = false;
		bool inputCima = false;
		bool inputAgaichar = false;
		//
		if(!isDead&&isPlaying&&!stun&&!Menu_Item.ativo&&!PauseGame.GameIsPaused){
			axisHorizontal = Input.GetAxis("Horizontal");
			axisVertical = Input.GetAxis("Vertical");
			inputJump = Input.GetButtonDown("Jump");
			inputRoll = Input.GetButtonDown("Fire3");
			//Detectar inputCima como GetButtonDown
			if(v_inputCima != Input.GetAxis("Vertical")>0.9f ){
                v_inputCima = Input.GetAxis("Vertical")>0.9f;
                //
                if(v_inputCima){
                    inputCima = v_inputCima;
                }
            }
			inputAgaichar = Input.GetAxis("Vertical")<-0.9f||Input.GetButton("Crounch");
		}
		
		/* - Verificadores - */
		PhysicsAndHeightDetection();

		/* - Execucao - */
		//__________________________________________________________________________________________________________________________________________________________________
		//Movement
		if(!stun&&!usingCrounch&&isRoling<=0&&!penduradoBorda&&!climbingLadder&&!climbing&&!_combatPlayer.usingArrow&&(!onGround&&_combatPlayer.atacando||!_combatPlayer.atacando)){
			//empede o jogador de se mover caso mais de uma axis esteja sendo pressionada no mesmo instante
            if (axisHorizontal != 0)
            {
                if ((Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
                {
                    axisHorizontal = 0;
                }
            }
            //muda a orientacao do player
            checkOrientation(axisHorizontal);
            //verifica se tem uma parede na frente do player
            bool wall = Physics2D.Linecast(wallSensors[0].transform.position, wallSensors[1].transform.position, detectionLayerGround);
            //move o player caso nao tenha parede na frente
            if(!wall){
            	animator.SetBool("Run", axisHorizontal!=0);

                move = axisHorizontal * speed;

                //movimenta via matematica com o rigidbody
                if(!_combatPlayer.usingShield){
                    rb.velocity = new Vector2(move, rb.velocity.y);
		        }
                else{
                    rb.velocity = new Vector2(move/2, rb.velocity.y);
                }
            }
		    else{
		    	animator.SetBool("Run", false);
		    }
        }

        //__________________________________________________________________________________________________________________________________________________________________
		//Agaichar
		usingCrounch = inputAgaichar&&onGround&&!climbing&&!climbingLadder;
        if(usingCrounch){
            rb.velocity = new Vector2(0f, rb.velocity.y);
            checkOrientation(axisHorizontal);
        }

		//__________________________________________________________________________________________________________________________________________________________________
		//PULO
        if ((inputJump || jumpBuffer == true)&&!penduradoBorda&&!climbingLadder&&!climbing&&!_combatPlayer.usingArrow)
        {
            //verifica se o player esta caindo apos o segundo pulo e se esta a uma distancia curta do chao para, caso contario apenas faz o pulo normal
            if (doubleJump == false && onGround == false && raycastMinimumHeight < 1.8f)
            {
                jumpBuffer = true;
            }
            else
            {
                CastJump_CastDoubleJump();
            }
        }

        //__________________________________________________________________________________________________________________________________________________________________
		//Roll

		//Clicar para rolar
		if(inputRoll&&onGround&&isRoling<=0&&tempoRoll<=0){
			isRoling = 0.33f;
			tempoRoll = 0.5f;
			_combatPlayer.atacando = false;
            checkOrientation(axisHorizontal);
		}
		//Clicar para rolar no ar, guardando o input caso esteja proximo do chao
        else if(inputRoll&&isRoling<=0&&raycastMinimumHeight<=1.8f&&rb.velocity.y<0){
            RolingClick = true;
            _combatPlayer.atacando = false;
        }
        //durante o rolamento
		if (isRoling>0)
		{
			//diminuir o tempo de rolamento
            isRoling-=Time.deltaTime;

            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("inimigo"), true);

            //caso bata em uma parede, mudar a direcao
            bool wall = Physics2D.Raycast(wallSensors[1].transform.position, Vector3.forward, Mathf.Infinity, detectionLayerGround);
            if(wall){
                checkOrientation(transform.right.x*-1f);
            }

            //realizar o rolamento
	        if(onGround == true)
	        {
                rb.velocity = new Vector2(transform.right.x*velocityMovementRolling, rb.velocity.y);
	        }       
            GameObject spriteVazioGerado = Instantiate(prefabSpriteVazio, meuSprite.transform.position, transform.rotation);
            spriteVazioGerado.GetComponent<SpriteRenderer>().sprite = meuSprite.sprite;
            spriteVazioGerado.transform.rotation = transform.rotation;
            spriteVazioGerado.transform.Rotate(0f, 180f, 0f);
            Destroy(spriteVazioGerado, 0.11f);
            //
            //quando o rolamento estiver no fim, verificar se tem algo em cima do player, para ele continuar rolando
            if(isRoling<Time.deltaTime*1.5f){
                bool haveSomethingAtopPlayer = CheckAtop();
                if(haveSomethingAtopPlayer){
                    isRoling = Time.deltaTime*2f;
                } 
            }
        }
        else if(!invuneravel){
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("player"), LayerMask.NameToLayer("inimigo"), false);
        }

        //diminuir o tempo para rolar novamente (valor que, caso nao esteja em zero, impede do jogador rolar)
        if(isRoling<=0&&tempoRoll>0){
        	tempoRoll-=Time.deltaTime;
        }

        //__________________________________________________________________________________________________________________________________________________________________
        //Pendurado

        //detectar borda e se pendurar
        if(!onGround&&!penduradoBorda&&rb.velocity.y<0f&&!_combatPlayer.atackDown&&raycastMinimumHeight>1.8f&&!inputAgaichar&&!climbing&&!climbingLadder&&!stun&&
            (_combatPlayer.ganchoGerado==null||_combatPlayer.ganchoGerado!=null&&!_combatPlayer.ganchoGerado.enganchou)){
            //
            //verifica se esta em uma borda
            RaycastHit2D borda = Physics2D.Linecast(sensorBorda[0].transform.position, sensorBorda[1].transform.position, detectionLayerGround);
            //se estivar em uma borda, se pendurar
            if(borda.collider!=null&&borda.point.y!=sensorBorda[0].transform.position.y){
                RaycastHit2D detectarBorda = Physics2D.Raycast(borda.point-new Vector2(transform.right.x, 0.1f), new Vector3(transform.right.x, 0f, 0f), 2f, detectionLayerGround);
                transform.position = new Vector3(detectarBorda.point.x+transform.right.x*-1, borda.point.y, transform.position.z);
                //
                penduradoBorda = true;
                _combatPlayer.atacando = false;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        //se estiver pendurado
        if(penduradoBorda){
            posYMaxima = transform.position.y;
        	//subir na plataforma clicando para Cima
            if(inputCima){
                RaycastHit2D borda = Physics2D.Linecast(sensorBorda[0].transform.position+new Vector3(transform.right.x*0.2f, 1f, 0), sensorBorda[1].transform.position, detectionLayerGround);
                RaycastHit2D detectarPlataforma = Physics2D.Raycast(borda.point+new Vector2(0f, 1f), new Vector3(transform.right.x, 0f, 0f), 0.8f, detectionLayerGround);
                if(detectarPlataforma.collider==null){
                	animator.SetTrigger("PenduradoSubir");
                	posBorda = borda.point+new Vector2(transform.right.x, 3f);
                }
            }
            //pular, saindo da plataforma
            else if(inputJump){    
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                doubleJump = false;
                penduradoBorda = false;
                onGround = true;
                CastJump_CastDoubleJump();
            }
            //soltar da plataforma clicando para baixo
            else if(inputAgaichar){
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                penduradoBorda = false;
                transform.position = transform.position - new Vector3(0f, 1f, 0f);
            }
        }

        //__________________________________________________________________________________________________________________________________________________________________
		//Climb

		//Iniciar Escalada
        if(inputCima&&!climbing&&!climbingLadder&&!penduradoBorda&&!_combatPlayer.atackDown){
        	//verifica se tem local de escalada
        	RaycastHit2D hitLadder = Physics2D.Raycast(transform.position-Vector3.up*1.25f, Vector3.forward, Mathf.Infinity, ladderLayer);
        	RaycastHit2D hitScalable = Physics2D.Raycast(transform.position-Vector3.up*1.25f, Vector3.forward, Mathf.Infinity, scalableLayer);
        	//se tocou em uma escada, ativa a escalada
        	if(hitLadder.collider!=null){
        		climbingLadder = true;
        		transform.position = new Vector3(hitLadder.collider.transform.position.x, transform.position.y, transform.position.z);
        	}
        	//se tocou em uma local de escalada, ativa a escalada
        	else if(hitScalable.collider!=null){
        		UsarPiolet();
        	}
        }
        else if (climbingLadder == true|| climbing == true)
        {
        	//desliga a gravidade a gravidade e trava a velocidade
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            posYMaxima = transform.position.y;

            //verifica se clicou pra sair da escalada
            if(inputJump&&!(axisVertical>0)){
            	SairDaEscalada();
            	doubleJump = false;
	            onGround = true;
                CastJump_CastDoubleJump();
            }

        	//Climbing Ladder
        	if(climbingLadder){
        		//verifica se ainda esta em uma escada
        		RaycastHit2D hitLadder = Physics2D.Raycast(transform.position-Vector3.up*1.25f, Vector3.forward, Mathf.Infinity, ladderLayer);

	        	//movimenta na corda
                rb.velocity = transform.up * (axisVertical * climbVerticalSpeed);

	            //caso nao esteja em uma escada, sair da escada. Do contrario, manter jogador alinhado
	            if(hitLadder.collider==null){
	            	SairDaEscalada();
	            	doubleJump = false;
	            	onGround = true;
                	CastJump_CastDoubleJump();
            	}
            	else{
		            //alinha jogador com a corda
		            rb.transform.position = new Vector2(hitLadder.transform.position.x, transform.position.y);
				}
			}
			//Climbing
			else if(climbing){
				//verifica se ainda esta em um local de escalada
				bool hitScalable = Physics2D.Raycast(transform.position-Vector3.up*1.25f, Vector3.forward, Mathf.Infinity, scalableLayer);

				//corrigir orientacao do player
				checkOrientation(axisHorizontal);
				rb.velocity = Vector3.right*axisHorizontal*climbVerticalSpeed+transform.up*axisVertical*climbVerticalSpeed;

				//caso nao esteja em um local de escalada, sair da escalada
				if(!hitScalable){
					SairDaEscalada();
					doubleJump = false;
					onGround = true;
                	CastJump_CastDoubleJump();
            	}
				//
				animator.SetBool("Run", axisHorizontal!=0);
			}
        	//
            animator.SetFloat("InputVertical", axisVertical);
        }

        //__________________________________________________________________________________________________________________________________________________________________
        //Ao Cair no Chao

        //Ao sair ou tocar no chao
        if(v_onGround!=onGround){
        	//ao sair do chao
            if(!onGround){
                posYMaxima = transform.position.y;
                isRoling = 0;
            }
            //ao tocar no chao
            if(onGround&&Mathf.Abs(posYMaxima-transform.position.y)>15f&&!_combatPlayer.atackDown){
                StartCoroutine(StunPlayer(1f));
            }
            //ao tocar no chao
            else if(onGround&&Mathf.Abs(posYMaxima-transform.position.y)>4.5f&&!_combatPlayer.atackDown){
                //efeito visual
                visualTransform.localScale = new Vector3(1.075f, 0.8f, 1f);
            }
            if(RolingClick){
                RolingClick = false;
                isRoling = 0.33f;
                tempoRoll = 1f;
            }
            //
            _combatPlayer.AoTocarNoChao();
            //
            v_onGround = onGround;
        }
        if(!onGround){
        	//guarda a informacao de altura maxima para depois verificar se deve stunar
            if(transform.position.y>posYMaxima){
                posYMaxima = transform.position.y;
            }
        }

        //__________________________________________________________________________________________________________________________________________________________________
        //Invuneravel

        if(invuneravel){
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("inimigo"), true);
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("damage"), true);
        }
        else{
            if(isRoling<0){ Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("inimigo"), false);}
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("damage"), false);
        }

        //__________________________________________________________________________________________________________________________________________________________________
        //Collider
        if((usingCrounch||isRoling>0)&&onGround){
        	baseCollider.enabled = false;
			colliderOnRoll.enabled = true;
        }
        else{
        	baseCollider.enabled = true;
			colliderOnRoll.enabled = false;
        }

        //Animation
        animator.SetBool("OnGround", onGround);
        animator.SetFloat("Gravity", rb.velocity.y);
        animator.SetBool("Crounch", usingCrounch);
        animator.SetBool("Roll", isRoling>0);
        animator.SetBool("Pendurado", penduradoBorda);
        animator.SetBool("ClimbingLadder", climbingLadder);
        animator.SetBool("Climbing", climbing);
	}

    void FixedUpdate(){
        // visualTransform.localScale = Vector3.Lerp(visualTransform.localScale, new Vector3(1+Mathf.Abs(rb.velocity.y/(maxPhysicVelocityY*6)), 1f+Mathf.Abs(rb.velocity.y/(maxPhysicVelocityY*3)), 1f), Time.deltaTime*5f);
        visualTransform.localScale = Vector3.Lerp(visualTransform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime*5f);
    
        // if(rb.velocity.x!=0){
        //     rb.velocity = new Vector2(rb.velocity.x*0.98f, rb.velocity.y);
        // }

        //posição da fenix
        if (PlayerStats.hasFenix)
        {
            fenix.transform.position = Vector2.Lerp(fenix.transform.position, ancoraFenix.transform.position, 4f * Time.fixedDeltaTime);
        }
    }

	//__________________________________________________________________________________________________________________________________________________________________
	//Acoes Player
	
	//executa o pulo / pulo duplo
    private void CastJump_CastDoubleJump()
    {

        //verifica se tem algo acima do player caso esteja rolando
        bool haveSomethingAtopPlayer = false;
        if (isRoling>0)
        {
            haveSomethingAtopPlayer = CheckAtop();
        }

        if (haveSomethingAtopPlayer == false)
        {
            if (onGround == true || climbingLadder == true || climbing)
            {
                visualTransform.localScale = new Vector3(0.8f, 1.175f, 1f);
                climbingLadder = false;
                climbing = false;
                //troca os triggers de animação para normal
                usingCrounch = false;

                if (jumpBuffer == true)
                {
                    jumpBuffer = false;
                }

                isRoling = 0f;
                rb.velocity = new Vector2(rb.velocity.x, 0);

                rb.AddForce(Vector2.up * jumpForce);

                doubleJump = true;
                onGround = false;
            }
            else
            {
                if (doubleJump&&!_combatPlayer.atackDown)
                {
                    visualTransform.localScale = new Vector3(0.8f, 1.175f, 1f);
                    animator.SetTrigger("DoubleJump");
                    climbingLadder = false;
                    climbing = false;

                    doubleJump = false;

                    isRoling = 0f;
                    rb.velocity = new Vector2(rb.velocity.x, 0);

                    rb.AddForce(Vector2.up * jumpForce);
                }
            }
        }
    }

    void PenduradoSubir(){
    	rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.position = posBorda;
        penduradoBorda = false;
        animator.ResetTrigger("PenduradoSubir");
    }

    void UsarPiolet(){
        bool temItemEscalada = false;
        for(int i=0; i<_sistemaItens.itensBag.Length; i++){
            if(_sistemaItens.itensBag[i].item!=null&&
                _sistemaItens.itensBag[i].item.tipoItem==Item.tiposDeItem.outro&&
                _sistemaItens.itensBag[i].item.itemID=="piolet"){ 
                temItemEscalada = true;
                break;
            }
        }
        //
        if(temItemEscalada){
            climbing = true;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }

    public void SairDaEscalada(){
        climbing = false;
        climbingLadder = false;
        rb.gravityScale = originalGravity;
    }

    public IEnumerator AtivarInvuneravel(float tempo){
        invuneravel = true;
        //
        for (int i = 0; i < tempo*(1/(Time.deltaTime*5f)); i++)
        {
            meuSprite.enabled = false;
            yield return new WaitForSeconds(Time.deltaTime*2.5f);
            meuSprite.enabled = true;
            yield return new WaitForSeconds(Time.deltaTime*2.5f);
        }
        //
        invuneravel = false;
    }

    public IEnumerator SimpleStunPlayer(float tempo){
        isRoling = 0f;
        rb.velocity = Vector2.zero;
        //
        stun = true;
        yield return new WaitForSeconds(tempo);
        stun = false;
    }

    public IEnumerator StunPlayer(float tempo){
        RolingClick = false;
        isRoling = 0f;
        rb.velocity = Vector2.zero;
        //
        SairDaEscalada();
        penduradoBorda = false;
        //
        animator.SetBool("Queda", true);
        stun = true;
        yield return new WaitForSeconds(tempo);
        animator.SetBool("Queda", false);
        yield return new WaitForSeconds(0.35f);
        stun = false;
    }

    //executa animação de morte e desliga a movimentação do player
    public void DiePlayer()
    {
        animator.SetBool("Die", true);
        baseCollider.enabled = false;
        colliderOnRoll.enabled = true;
        isDead = true;
        //
        StartCoroutine(Rebirth());
    }

    public IEnumerator Rebirth()
    {
    	yield return new WaitForSeconds(1f);
    	//
        SceneManager.LoadScene(gameScene);
    }

	//__________________________________________________________________________________________________________________________________________________________________
	//Verificadores
	void PhysicsAndHeightDetection()
    {
        //limita a velocidade fisica no eixo y (queda e subida)
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, minPhysicVelocityY, maxPhysicVelocityY));

        //descobre a altura do personagem referente ao chao, usando 3 raycasts apontados para baixo
        raycastMinimumHeight = 2;
        for (int I = 0; I < groundSensors.Length; I++)
        {
            //armazena o raio
            RaycastHit2D rayForGroundDistance = Physics2D.Raycast(groundSensors[I].transform.position, Vector2.down, Mathf.Infinity, detectionLayerGround);

            //armazena a distancia do raio
            float CheckedSensorDistance = rayForGroundDistance.distance;

            // define valor minimo como raio mais proximo do chao
            if (raycastMinimumHeight > CheckedSensorDistance&&rayForGroundDistance.collider!=null)
            {
                raycastMinimumHeight = CheckedSensorDistance;
            }
        }

        //VERIFICA A DISTANCIA PARA SABER O ESTADO DO JOGADOR (PULANDO, CAINDO) E TROCAR AS DEVIDAS ANIMAÇÔES
        onGround = (raycastMinimumHeight == 0 || raycastMinimumHeight < 0.01f);
    }

    //direção do player e fenix
    public void checkOrientation(float direction)
    {
        //ORIENTAÇÃO da fenix de acordo com os inputs de movimentação
        if (direction > 0.0f)
        {
            fenix.transform.eulerAngles = transform.eulerAngles = new Vector2(0, 0);
        }
        else if (direction < 0.0f)
        {
            fenix.transform.eulerAngles = transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public bool CheckAtop()
    {
        bool have = false;
        //verficia se os sensores detectam algo em cima do jogador
        RaycastHit2D rhc;

        float CastedDistance = 1.76f;

        for (int i = 0; i < topSensors.Length; i++)
        {
            rhc = Physics2D.Raycast(topSensors[i].transform.position, topSensors[i].transform.up, CastedDistance, detectionLayerGround);
            if (rhc.collider != null)
            {
                Debug.DrawLine(topSensors[i].transform.position, rhc.point, Color.cyan);
                if (rhc.distance < CastedDistance)
                {
                    have = true;
                    break;
                }
            }
        }        
        return have;
    }

	//__________________________________________________________________________________________________________________________________________________________________
	//Fenix
	public void EnableFenix()
    {
        if(fenix!=null){
            fenix.GetComponent<FenixScript>().hadCapture = PlayerStats.hasFenix;
            fenix.SetActive(true);
            fenix.GetComponent<Animator>().SetTrigger("BIRTH");
        }
    }

	//__________________________________________________________________________________________________________________________________________________________________
	//Colisao
	private void OnCollisionEnter2D(Collision2D c){

	}

	private void OnTriggerEnter2D(Collider2D c)
    {
        
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        
    }
}