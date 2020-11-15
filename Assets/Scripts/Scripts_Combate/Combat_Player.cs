using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//__________________________________________
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(MovementScript))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(SistemaItens))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
//__________________________________________
public class Combat_Player : MonoBehaviour
{
    //__________________________________________________________________________________________________________________________________________________________________
    //Variaveis

    //__________________________________________
    /*- Inspector -*/
    [HideInInspector]
    public Animator animator;
    bool playingAttack = false;
    bool pressingAttackForCombo = false;
    public GameObject prefabDano;
    public Transform damagePoint;
    public int dano;
    bool checkingFrame = false;
    [Header("Arco")]
    public GameObject flechaPrefab;
    public float distanciaMaximaDetectarInimigo;
    public float maxRotacionar=15f;
    [Header("Gancho")]
    public GameObject ganchoPrefab;
    [Header("Piolet")]
    public float velocidadeEscalada;
    
    //__________________________________________
    /*- Public -*/
    [HideInInspector]
    public bool atackDown;
    [HideInInspector]
    public Gancho ganchoGerado;
    [HideInInspector]
    public bool atacando;
    [HideInInspector]
    public int slotItemUsando;
    [HideInInspector]
    public bool usingArrow;
    [HideInInspector]
    public bool usingShield;
    [HideInInspector]
    public bool shield;

    //__________________________________________
    /*- Private -*/
    private int localFlechasBag;
    private float tempoAtirarFlecha;
    //
    private bool v_inputHorizontalDown;
    private bool v_inputAttackDown;
    //
    private float tempoDoubleClick = 0;
    private float posYMaxima;

    //__________________________________________
    /*- Components -*/
    private MovementScript _movementScript;
    private SistemaItens _sistemaItens;
    private Rigidbody2D rb;

    //__________________________________________________________________________________________________________________________________________________________________
    //Funcoes Base

    void Start()
    {
        //pega os componentes de animaçao e o codigo do player
        animator = GetComponent<Animator>();
        _movementScript = GetComponent<MovementScript>();
        _sistemaItens = GetComponent<SistemaItens>();
        rb = GetComponent<Rigidbody2D>();
        posYMaxima = transform.position.y;
    }

    void Update()
    {
        /* - Deteccao Input - */
        float axisHorizontal = 0f;
        bool inputHorizontalDown = false;
        bool inputArma1Down = false;
        bool inputArma2Down = false;
        bool inputArma1 = false;
        bool inputArma2 = false;
        //
        bool inputArmaUsando = false;
        //
        bool inputAttackDown = false;
        //
        if(!_movementScript.isDead&&_movementScript.isPlaying&&!Menu_Item.ativo&&!_movementScript.stun&&!PauseGame.GameIsPaused){
            axisHorizontal = Input.GetAxis("Horizontal");
            //
            if(v_inputHorizontalDown != Mathf.Abs(Input.GetAxis("Horizontal"))>0.1f){
                v_inputHorizontalDown = Mathf.Abs(Input.GetAxis("Horizontal"))>0.1f;
                //
                if(v_inputHorizontalDown){
                    inputHorizontalDown = v_inputHorizontalDown;
                }
            }
            //
            inputArma1Down = Input.GetButtonDown("Fire1");
            inputArma2Down = Input.GetButtonDown("Fire2");
            inputArma1 = Input.GetButton("Fire1");
            inputArma2 = Input.GetButton("Fire2");
            //
            inputArmaUsando = (inputArma1&&slotItemUsando==0)||(inputArma2&&slotItemUsando==1);
            //
            if(v_inputAttackDown != (Input.GetAxis("Vertical")<-0.9f||Input.GetButton("Crounch")) ){
                v_inputAttackDown = Input.GetAxis("Vertical")<-0.9f||Input.GetButton("Crounch");
                //
                if(v_inputAttackDown){
                    inputAttackDown = v_inputAttackDown;
                }
            }
        }

        //__________________________________________________________________________________________________________________________________________________________________
        /*- Usar Arma -*/
        //Clique para atacar
        Item arma1 = _sistemaItens.itensMao[0].item;
        Item arma2 = _sistemaItens.itensMao[1].item;
        Item armaUsando = _sistemaItens.itensMao[slotItemUsando].item;
        //
        if (inputArma1Down&&arma1!=null&&!_movementScript.penduradoBorda&&!atackDown){
            UsarArma(0);
        }
        else if (inputArma2Down&&arma2!=null&&!_movementScript.penduradoBorda&&!atackDown){
            UsarArma(1);
        }
        else{
            usingArrow = inputArmaUsando&&armaUsando!=null&&armaUsando.tipoArma==Item.tiposDeArma.arco&&(_sistemaItens.itensBag[localFlechasBag].item!=null&&_sistemaItens.itensBag[localFlechasBag].item.itemID=="flecha")&&_movementScript.onGround;
            animator.SetBool("Arrow", usingArrow);
            if(usingArrow){
                rb.velocity = new Vector2(0f, rb.velocity.y);
                _movementScript.checkOrientation(axisHorizontal);
            }
            //
            usingShield = inputArmaUsando&&armaUsando!=null&&armaUsando.tipoArma==Item.tiposDeArma.escudo&&_movementScript.onGround&&_movementScript.isRoling<=0;
            animator.SetBool("Shield", usingShield);
            //
            if(usingShield){
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }

        AnimatorStateInfo atualAnimacao = animator.GetCurrentAnimatorStateInfo(0); 
        atacando = atualAnimacao.IsName("Attack Sword Combo1")||atualAnimacao.IsName("Attack Sword Combo2")||atualAnimacao.IsName("Attack Sword Combo3")||atualAnimacao.IsName("Crounch Attack");
        shield = atualAnimacao.IsName("OnShield")||atualAnimacao.IsName("Crounch OnShield");

        if(atacando){
            _movementScript.checkOrientation(axisHorizontal);
            rb.velocity = new Vector2(0f, rb.velocity.y);
            //
            GameObject spriteVazioGerado = Instantiate(_movementScript.prefabSpriteVazio, _movementScript.meuSprite.transform.position, transform.rotation);
            spriteVazioGerado.GetComponent<SpriteRenderer>().sprite = _movementScript.meuSprite.sprite;
            spriteVazioGerado.transform.Rotate(0f, 180f, 0f);
            Destroy(spriteVazioGerado, 0.11f);
        }

        //Flecha
        if(tempoAtirarFlecha>0){ tempoAtirarFlecha-=Time.deltaTime;}
        else if(tempoAtirarFlecha<0){
            tempoAtirarFlecha = 0;
        }
        //Gancho
        if(ganchoGerado!=null){
            if(_sistemaItens.itensMao[slotItemUsando].item==null||_sistemaItens.itensMao[slotItemUsando].item.tipoArma!=Item.tiposDeArma.gancho){
                ganchoGerado.SoltarGancho();
            }
            //
            if(Input.GetAxis("Vertical")!=0){
                ganchoGerado.AumentarTamanhoCorda(Input.GetAxis("Vertical")*-5*Time.deltaTime);
                if(rb.velocity==Vector2.zero){ rb.velocity = new Vector3(1f, 0f, 0f);}
            }
        }

        //__________________________________________________________________________________________________________________________________________________________________
        //AtackDown
        if(inputAttackDown&&!_movementScript.onGround&&!atackDown&&!_movementScript.penduradoBorda&&!_movementScript.climbingLadder&&!_movementScript.climbing){
            //
            if((arma1!=null&&arma1.tipoArma!=Item.tiposDeArma.arco&&arma1.tipoArma!=Item.tiposDeArma.escudo&&arma1.tipoArma!=Item.tiposDeArma.gancho)){
            	if(tempoDoubleClick>0){
                    slotItemUsando = 0;
            		atackDown = true;
                    _movementScript.invuneravel = true;
            		rb.velocity = new Vector2(rb.velocity.x, -20f);
            		//
            		if(ganchoGerado!=null){
            			ganchoGerado.SoltarGancho();
            		}
            	}
            	tempoDoubleClick = 0.25f;
            }
            else if((arma2!=null&&arma2.tipoArma!=Item.tiposDeArma.arco&&arma2.tipoArma!=Item.tiposDeArma.escudo&&arma2.tipoArma!=Item.tiposDeArma.gancho)){
                if(tempoDoubleClick>0){
                    slotItemUsando = 1;
                    atackDown = true;
                    _movementScript.invuneravel = true;
                    rb.velocity = new Vector2(rb.velocity.x, -20f);
                    //
                    if(ganchoGerado!=null){
                        ganchoGerado.SoltarGancho();
                    }
                }
                tempoDoubleClick = 0.25f;
            }
        }
        if(tempoDoubleClick>0){
        	tempoDoubleClick-=Time.deltaTime;
        }
        animator.SetBool("AttackDown", atackDown);
    }

    public void AoTocarNoChao(){
        if(_movementScript.onGround&&atackDown){
            StartCoroutine(AtackDown());
        }
    }
    //__________________________________________________________________________________________________________________________________________________________________
    //Usar Arma

    //__________________________________________________________________________________________________________________________________________________________________
    //Attack

    void UsarArma(int numeroSlot){
        bool haveSomethingAtopPlayer = false;
        if(_movementScript.isRoling>0){
            haveSomethingAtopPlayer = _movementScript.CheckAtop();
        }
        if(!haveSomethingAtopPlayer){
            rb.velocity = new Vector2(0f, rb.velocity.y);
            _movementScript.isRoling = 0;
            //
            if(slotItemUsando!=numeroSlot){
                animator.SetTrigger("ResetArma");
                animator.SetInteger("ComboAttack", 0);
                animator.ResetTrigger("Attack");
                usingArrow = false;
                usingShield = false;
                atacando = false;
            }
            //
            slotItemUsando = numeroSlot;
            //
            Item arma = _sistemaItens.itensMao[numeroSlot].item;
            int tipoArma = arma.tipoArmaToInteger();
            if(tipoArma==0){
                atacando = true;
                //
                _movementScript.isRoling = 0;
                //verifica se o player clicou o ataque para o combo
                if (checkingFrame == true)
                {
                    pressingAttackForCombo = true;
                }
                //inicia parametro para ataque
                animator.SetTrigger("Attack");
            }
            else if(tipoArma==2){
                if(_sistemaItens.itensBag[localFlechasBag].item==null||_sistemaItens.itensBag[localFlechasBag].item.itemID!="flecha"){
                    for(int i=0; i<_sistemaItens.itensBag.Length; i++){
                        if(_sistemaItens.itensBag[i].item!=null&&_sistemaItens.itensBag[i].item.itemID=="flecha"){
                            localFlechasBag = i;
                            //
                            break;
                        }
                    }
                }
            }
            else if(tipoArma==3&&ganchoGerado==null){ UsarGancho();}
            else if(tipoArma==3&&ganchoGerado!=null){ganchoGerado.SoltarGancho();}
        }
    }

    public void ResetSpeed(){
        animator.speed = 1f;
    }

    //inicia a verificação de click para o combo no inicio do frame de ataque
    public void StartFrame()
    {
        // checkingFrame = true;
        animator.speed = 1/_sistemaItens.itensMao[slotItemUsando].item.velocidade;
    }

    //desliga a verificação e inicia o proximo combo caso o player tenha clicado no meio da verficiação
    public void EndFrame()
    {
        checkingFrame = false;

        if(pressingAttackForCombo == true)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("STOPATTACK");
        }

        pressingAttackForCombo = false;
        
        animator.speed = 1;
    }

    public void FimTotalAtack(){
        animator.SetTrigger("STOPATTACK");
    }

    //adiciona 1 para o contador de combo e o joga nos parametros de ataque
    public void SetComboFrameCount(int ComboNumberOfThisFrame)
    {
        animator.SetInteger("ComboAttack", ComboNumberOfThisFrame);
    }

    //reinicia os combos
    public void LastFrameCount()
    {
        animator.SetInteger("ComboAttack", 0);
    }

    //void parar dar dano, executado via evento de animação
    public void DamegeOn()
    {
        Item arma = _sistemaItens.itensMao[slotItemUsando].item;
        if(arma!=null){
            //instancia o prefab de dano e diz o que que invocou o prefab foi este objeto (player) e envia o valor do dano para ele aplicar no inimigo
            AttackInstance isDamage = Instantiate(prefabDano, damagePoint).GetComponent<AttackInstance>();
            isDamage.transform.localScale += new Vector3(0.5f, 0f, 0f);
            isDamage.master = this.gameObject;
            isDamage.gameObject.transform.parent = null;
            isDamage.damage = AttackInstance.CreateWeaponDamageDictionary(arma);
            isDamage.damageEffect.AddListener(arma.efeito.Call);  
            isDamage.damageEffect.AddListener(Hit);
        }
    }

    public void CrounchDamageOn(){
        damagePoint.position-=new Vector3(0f, 0.5f, 0f);
        //
        DamegeOn();
        //
        damagePoint.position+=new Vector3(0f, 0.5f, 0f);
    }

    IEnumerator AtackDown(){
        rb.velocity = Vector3.zero;
        //
    	Item arma = _sistemaItens.itensMao[slotItemUsando].item;
    	//
    	AttackInstance isDamage = Instantiate(prefabDano, transform.position-Vector3.up*2.75f, Quaternion.Euler(Vector3.zero)).GetComponent<AttackInstance>();
        isDamage.transform.localScale = new Vector3(isDamage.transform.localScale.x*3f, isDamage.transform.localScale.y, isDamage.transform.localScale.z);
        isDamage.master = this.gameObject;
        isDamage.gameObject.transform.parent = null;
        isDamage.damage = AttackInstance.CreateWeaponDamageDictionary(arma);
        isDamage.damageEffect.AddListener(arma.efeito.Call);
        isDamage.damageEffect.AddListener(StunAtackDown);
        isDamage.damageEffect.AddListener(EmpurrarInimigosAtackDown);
        // 
        _movementScript.stun = true;
        //
        atackDown = false;
        //
        yield return new WaitForSeconds(1f/3f);
        _movementScript.invuneravel = false;
        _movementScript.stun = false;
    }

    void EmpurrarInimigosAtackDown(GameObject obj){
        float distancia = Mathf.Abs(obj.transform.position.x-transform.position.x);
        float direcaoInimigo = 1;
        if(transform.position.x<obj.transform.position.x){
            direcaoInimigo = -1;
        }
        distancia = Mathf.Clamp(distancia, 0.5f, Mathf.Infinity);
        direcaoInimigo = direcaoInimigo*(1/distancia);
        obj.GetComponent<Rigidbody2D>().velocity = new Vector3(direcaoInimigo, 0f, 0f) * -50f / obj.GetComponent<Rigidbody2D>().mass;
    }

    void StunAtackDown(GameObject obj){
        StartCoroutine(StunExecucao(obj));
    }       

    IEnumerator StunExecucao(GameObject obj){
        MonoBehaviour[] componentes = obj.GetComponents<MonoBehaviour>();
        //
        for(int i=0; i<componentes.Length; i++){
            componentes[i].enabled = false;
        }
        obj.GetComponent<Collider2D>().enabled = true;
        obj.GetComponent<EnemyStatus>().enabled = true;
        if (obj.GetComponent<SpriteRenderer>() == true)
        { 
            obj.GetComponent<SpriteRenderer>().enabled = true;
        }
        //
        yield return new WaitForSeconds(0.5f);
        //
        for(int i=0; i<componentes.Length; i++){
            if(componentes[i] != null)
            {
                componentes[i].enabled = true;
            }
        }
    }

    void Hit(GameObject obj){
        if (obj.GetComponent<EnemyStatus>().canKnockBack)
        {
            StartCoroutine(EmpurraoInimigo(obj));
            //obj.GetComponent<Rigidbody2D>().velocity = new Vector3(direcaoInimigo, 0f, 0f) * -50f / obj.GetComponent<Rigidbody2D>().mass;
        }
    }

    IEnumerator EmpurraoInimigo(GameObject obj){
        int direcaoInimigo = 1;
        if(transform.position.x<obj.transform.position.x){
            direcaoInimigo = -1;
        }
        //
        Rigidbody2D rb_Objeto = obj.GetComponent<Rigidbody2D>(); 
        //
        rb_Objeto.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.05f);
        rb_Objeto.velocity = new Vector3(direcaoInimigo, 0f, 0f) * -100f / obj.GetComponent<Rigidbody2D>().mass;
    }

    //__________________________________________________________________________________________________________________________________________________________________
    //Arco
    public void ArcoStartFrame(){
        Item arco = _sistemaItens.itensMao[slotItemUsando].item;
        //
        animator.speed = 1/arco.velocidade;
    }

    public void ArcoIniciarPreparacao(){
        Item arco = _sistemaItens.itensMao[slotItemUsando].item;
        //
        tempoAtirarFlecha = arco.velocidade*0.166f;
    }

    public void ArcoFrame(){
        EnemyStatus[] inimigo = FindObjectsOfType<EnemyStatus>();
        float inimigoMaisProximo = distanciaMaximaDetectarInimigo;
        int inimigoEscolhido = -1;
        for(int i=0; i<inimigo.Length; i++){
            float distancia = Vector3.Distance(inimigo[i].transform.position, transform.position);
            if(distancia<inimigoMaisProximo&&
                inimigo[i].life>0&&
                ((transform.right.x<0&&transform.position.x>inimigo[i].transform.position.x)||
                (transform.right.x>0&&transform.position.x<inimigo[i].transform.position.x))){
                inimigoMaisProximo = distancia;
                inimigoEscolhido = i;
            }
        }
        //
        Item arco = _sistemaItens.itensMao[slotItemUsando].item;
        //
        Flecha flechaGerada = Instantiate(flechaPrefab, damagePoint.position, transform.rotation).GetComponent<Flecha>();
        //
        if(inimigoEscolhido!=-1){
            Vector3 posCentroInimigo = inimigo[inimigoEscolhido].transform.position+(Vector3)inimigo[inimigoEscolhido].GetComponent<Collider2D>().offset;
            Quaternion rotacaoOriginal = flechaGerada.transform.rotation;
            //
            flechaGerada.transform.rotation = Quaternion.LookRotation(posCentroInimigo-flechaGerada.transform.position);
            flechaGerada.transform.Rotate(Vector3.up*-90f);
            //
            if(Quaternion.Angle(flechaGerada.transform.rotation, rotacaoOriginal)>maxRotacionar){
                flechaGerada.transform.rotation = rotacaoOriginal;
            }
        }
        //
        float tempoTotalArco = arco.velocidade*0.166f;
        float porcentagemDano = 1f-(tempoAtirarFlecha/tempoTotalArco);
        porcentagemDano = (porcentagemDano*0.75f)+0.25f;
        //
        Dictionary<string, int> valoresDano = new Dictionary<string, int>(){ 
            {"physic", (int)(arco.physicDamage*porcentagemDano)},
            {"thunder", (int)(arco.thunderDamage*porcentagemDano)},
            {"void", (int)(arco.voidDamage*porcentagemDano)},
            {"fire", (int)(arco.fireDamage*porcentagemDano)},
            {"ice", (int)(arco.iceDamage*porcentagemDano)}
        };
        //
        flechaGerada.velocidade = 30f*porcentagemDano;
        flechaGerada.tempoCair = 0.25f*porcentagemDano;
        //
        flechaGerada.AtribuirValores(valoresDano, true);
        flechaGerada.damageEffect = arco.efeito;
        //
        if(_sistemaItens.itensBag[localFlechasBag].quantos>0){
            _sistemaItens.itensBag[localFlechasBag].quantos--;
        }
        else{
            _sistemaItens.itensBag[localFlechasBag].item=null;
        }
    }

    public void ArcoEndFrame(){
        animator.speed = 1f;
    }

    public void CrounchArcoFrame(){
        damagePoint.position-=new Vector3(0f, 0.5f, 0f);
        //
        ArcoFrame();
        //
        damagePoint.position+=new Vector3(0f, 0.5f, 0f);
    }

    //__________________________________________________________________________________________________________________________________________________________________
    //Gancho
    void UsarGancho(){
        ganchoGerado = Instantiate(ganchoPrefab, transform.position, Quaternion.Euler(0f, 0f, 0f)).GetComponent<Gancho>();
    }
}
