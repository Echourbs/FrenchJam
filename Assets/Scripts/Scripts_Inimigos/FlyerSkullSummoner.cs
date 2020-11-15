using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerSkullSummoner : MonoBehaviour
{

	public float velocidade;
	public int dano;
	//
	private Transform player;
	private EnemyStatus _enemyStatus;
	private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        player = MovementScript.player.transform;
    	_enemyStatus = GetComponent<EnemyStatus>();
    	_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     	if(_enemyStatus.inDamage){
     		_animator.SetBool("Die", true);
     		Destroy(gameObject);
     	}   
     	//
     	transform.position = Vector3.MoveTowards(transform.position, player.position-Vector3.up*2f, Time.deltaTime*velocidade);
    	transform.LookAt(player);
    	transform.Rotate(Vector3.up*-90f);
    }

    void OnTriggerEnter2D(Collider2D col){
    	if(col.gameObject==player.gameObject){
    		player.GetComponent<PlayerHealth>().TakeDamage(dano);
    		_animator.SetBool("Die", true);
    		Destroy(gameObject);
    	}
    }
}
