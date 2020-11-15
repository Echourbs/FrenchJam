using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gancho : MonoBehaviour
{

	// public float tamanhoMaximoCorda;
	[Header("Basico")]
	public string layer = "Default";
	public Transform cordaGancho;
	public Transform outraPonta;
	//
	[Header("Principal")]
	public float velocidadeCorda;
	public float tamanhoMaximoCorda = 5f;
	public float tamanhoMinimoCorda = 1f;
	public float multiplicadorForca = 2f;
	//
	[HideInInspector]
	public float tamanhoCorda;
	//
	private GameObject player;
	private DistanceJoint2D _distanceJoint;
	//
    [HideInInspector]
	public bool enganchou = false;
	private bool indo = true;
	private Vector2 posAnteriorPlayer = new Vector2(0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        player = MovementScript.player;
    	_distanceJoint = outraPonta.GetComponent<DistanceJoint2D>();
    	//
    	posAnteriorPlayer = player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cordaGancho.position = player.transform.position;
        //
        float distancia = Vector3.Distance(cordaGancho.position, outraPonta.position);
        //
        if(indo&&!enganchou){
        	outraPonta.Translate(Vector3.up*velocidadeCorda*Time.deltaTime);
            outraPonta.transform.position = new Vector3(player.transform.position.x, outraPonta.transform.position.y, outraPonta.transform.position.z);
        	//
        	if(distancia>tamanhoMaximoCorda){
        		indo = false;
        	}
        	//
        	VerificarPonta();
        }
        else if(enganchou){
        	_distanceJoint.distance = tamanhoCorda;
        }
        else{
        	outraPonta.transform.position = Vector3.MoveTowards(outraPonta.transform.position, cordaGancho.position, Time.deltaTime*velocidadeCorda*2);
        	//
        	if(distancia<0.25f){
        		Destroy(gameObject);
        	}
        }
        //
        SeguirPonta();
        //
        posAnteriorPlayer = player.transform.position;
    }

    void SeguirPonta(){
    	float distancia = Vector3.Distance(cordaGancho.position, outraPonta.position);
        cordaGancho.localScale = new Vector3(cordaGancho.localScale.x, cordaGancho.localScale.y, distancia);
        //
        cordaGancho.LookAt(outraPonta);
    }

    void VerificarPonta(){
    	RaycastHit2D hit = Physics2D.Linecast(outraPonta.position+Vector3.up*0.5f, outraPonta.position+Vector3.down*0.5f, 1<<LayerMask.NameToLayer(layer));
    	//
    	if(hit.collider!=null){
    		outraPonta.position = hit.point;
    		//
    		float distancia = Vector3.Distance(cordaGancho.position, outraPonta.position);
    		//
    		Rigidbody2D player_rb = player.GetComponent<Rigidbody2D>();
    		enganchou = true;
    		tamanhoCorda = distancia;
    		_distanceJoint.enabled = true;
    		_distanceJoint.connectedBody = player_rb;
    		//
	    	float forca = Mathf.Abs(Input.GetAxis("Horizontal"))*Mathf.Abs(player_rb.velocity.y);
    		player_rb.velocity = cordaGancho.transform.up*forca;
    	}
    }

    public void AumentarTamanhoCorda(float valorAumentar){
        tamanhoCorda = Vector3.Distance(cordaGancho.position, outraPonta.position);
    	tamanhoCorda+=valorAumentar;
    	//
    	if(tamanhoCorda>tamanhoMaximoCorda){
    		tamanhoCorda = tamanhoMaximoCorda;
    	}
    	else if(tamanhoCorda<tamanhoMinimoCorda){
    		tamanhoCorda = tamanhoMinimoCorda;
    	}
    }

    public void SoltarGancho(){
    	if(posAnteriorPlayer!=new Vector2(0f, 0f)&&enganchou){
	    	float distancia = Vector3.Distance(player.transform.position, posAnteriorPlayer);
	    	float forca = distancia/Time.deltaTime;
	    	//
	    	player.GetComponent<Rigidbody2D>().velocity = cordaGancho.transform.up*forca*multiplicadorForca;
	    	//
	    	// player.GetComponent<MovementScript>().doubleJump = true;
	    }
    	//
    	enganchou = false;
    	indo = false;
    	_distanceJoint.enabled = false;
    	//
    }
}
