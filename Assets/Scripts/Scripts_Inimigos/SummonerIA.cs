using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerIA : MonoBehaviour
{
	//Public
	public GameObject PrefabFlyer;
	public Transform ondeGerar;
	public float distanciaPlayer;
	public float tempoInvocar;
	public float tempoParadoPosDano;
	//Private
	private float _tempoPosDano;
	private float _tempoInvocar;
	private bool morto;
	//Components
	private EnemyStatus _enemyStatus;
	private Collider2D _collider;
	private Animator _animator;
	private Rigidbody2D rb;
	private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        _enemyStatus = GetComponent<EnemyStatus>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
       	//
       	player = MovementScript.player.transform; 
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemyStatus.inDamage){
        	_enemyStatus.inDamage = false;
        	_tempoPosDano = tempoParadoPosDano;
        	_tempoInvocar = 0f;
        	_animator.SetTrigger("Damage");
        	if(_enemyStatus.life<=0){
        		_enemyStatus.life = 0;
        		morto = true;
        		_animator.SetBool("Die", true);
        		//
        		rb.simulated = false;
        		_collider.enabled = false;
        		//
                GetComponent<EnemyDrop>().Drop();
                //
        		this.enabled = false;
        	}
        }
        //
        float distancia = Vector2.Distance(transform.position, player.position);
        if(_tempoPosDano<=0&&!morto&&distancia<distanciaPlayer){
        	if(_tempoInvocar<=0){
        		_tempoInvocar = tempoInvocar;
        		_animator.SetTrigger("Invoke");
        	}
        	else{
        		_tempoInvocar-=Time.deltaTime;
        	}
        }
        else if(_tempoPosDano>0&&!morto){
        	_tempoPosDano -= Time.deltaTime;
        }
    }

    public void Invocar(){
    	Instantiate(PrefabFlyer, ondeGerar.position, Quaternion.Euler(0f, 0f, 0f));
    }
}
