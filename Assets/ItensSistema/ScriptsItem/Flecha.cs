using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{

	public float velocidade;
    public float tempoCair;
	public bool souDoPlayer;
	//
	[HideInInspector]
    public Dictionary<string, int> damage = new Dictionary<string, int>(){
    	{"physic", 0},
    	{"thunder", 0},
    	{"void", 0},
    	{"fire", 0},
    	{"ice", 0}
    };

    [HideInInspector]
    public Effect damageEffect = new Effect();

    //
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
        //
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right*velocidade;
        //
        StartCoroutine(Cair());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // transform.Translate(Vector3.right*velocidade*Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        transform.Rotate(Vector3.up*-90f);
    }

    IEnumerator Cair(){
        yield return new WaitForSeconds(tempoCair);
        //
        rb.gravityScale = 1f;
        rb.drag = 0.5f;
    }

    void OnTriggerEnter2D(Collider2D col){
    	if(col.gameObject.layer==LayerMask.NameToLayer("inimigo")&&souDoPlayer){
    		EnemyStatus _enemyStatus = col.gameObject.GetComponent<EnemyStatus>();
    		_enemyStatus.CastMultipleTypeDamage(damage);
    		//
    		damageEffect.Invoke(col.gameObject);
    		//
    		Destroy(gameObject);
    	}
    	else if(col.gameObject==MovementScript.player&&!souDoPlayer){
    		PlayerHealth _playerHealth = col.gameObject.GetComponent<PlayerHealth>();
    		_playerHealth.TakeMultipleTypeDamage(damage, transform.position);
    		//
    		damageEffect.Invoke(col.gameObject);
    		//
    		Destroy(gameObject);
    	}
    	else if(col.gameObject.layer==LayerMask.NameToLayer("floor")){
    		Destroy(gameObject);
    	}
    }

    public void AtribuirValores( Dictionary<string, int> damage, bool souDoPlayer = false){
    	this.damage = damage;
    	this.souDoPlayer = souDoPlayer;
    }
}
