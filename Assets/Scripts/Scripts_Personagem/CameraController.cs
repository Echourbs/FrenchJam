using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	//Static
    public static CameraController _cameraController;
	//Inspector
    public Transform target;
    public Transform segundoTarget;
    [Header("Básico")]
    public Camera cam;
	[Header("Padrão")]
	public float velocidade;
	public float distanciaPadrao;
	public float rotacao;
	[Header("Player")]
	public float quantoFrente;
	[Header("Identificadores")]
	public LayerMask layerGround;
	public string areaVisaoTag;
	public string areasDeVisaoTag;
	//Public
	[HideInInspector]
	public float distancia;
	//Private
	private float proporcaoDistanciaX;
	private float proporcaoDistanciaY;
	private Vector2 dimensao;
	private Vector3 posObjetivo;
	//Components
	private Rigidbody2D rb;
	private MovementScript _movementScript;
	private Combat_Player _combatPlayer;

    void Awake(){
        _cameraController = this;
    }

    //
    void Start()
    {
        _movementScript = GetComponent<MovementScript>();
        _combatPlayer = GetComponent<Combat_Player>();
    	rb = GetComponent<Rigidbody2D>();
    	//
    	float distanciaInicial = cam.transform.position.z;
    	dimensao = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
    	dimensao = cam.transform.position - new Vector3(dimensao.x, dimensao.y, 0f);
    	dimensao = new Vector2(dimensao.x*2f, dimensao.y*2f);
    	//
    	proporcaoDistanciaX = distanciaInicial/dimensao.x;
    	proporcaoDistanciaY	= distanciaInicial/dimensao.y;
    	//
    	distancia = distanciaPadrao;
    }

    //
    void Update()
    {
    	float velocidadeAtual = velocidade;

    	//Se tiver dois targets
    	if(segundoTarget!=null){
    		float distanciaEntreTargetsX = segundoTarget.position.x-target.position.x;
    		float distanciaEntreTargetsY = segundoTarget.position.y-target.position.y;
    		//
    		posObjetivo = new Vector3(target.position.x+distanciaEntreTargetsX/2, target.position.y+distanciaEntreTargetsY/2, cam.transform.position.z);
    		//
    		distanciaEntreTargetsX = Mathf.Abs(distanciaEntreTargetsX);
    		distanciaEntreTargetsY = Mathf.Abs(distanciaEntreTargetsY);
    		if((distanciaEntreTargetsX*1.15f)>dimensao.x||(distanciaEntreTargetsY*1.15f)>dimensao.y){
    			if(distanciaEntreTargetsY*dimensao.x>distanciaEntreTargetsX*dimensao.y){
    				distancia = (distanciaEntreTargetsY*1.15f)*proporcaoDistanciaY;
    			}
    			else{
    				distancia = (distanciaEntreTargetsX*1.15f)*proporcaoDistanciaX;
    			}
    		}
    		else{
    			distancia = distanciaPadrao;
    		}
    	}
    	//Se for o Player
    	else if(target.gameObject==gameObject){
	    	//
	    	posObjetivo = target.position + new Vector3(transform.right.x*quantoFrente+(Input.GetAxis("HorizontalAxis2")*5f), Input.GetAxis("VerticalAxis2")*5f, 0f);
	    	
	    	//MovementScript
	    	if(!_movementScript.onGround&&!_movementScript.penduradoBorda&&!_movementScript.climbingLadder&&!_movementScript.climbing){
	    		bool hitGround = Physics2D.Raycast(transform.position, Vector3.down, 15f, layerGround);
	    		//
	    		if(hitGround&&rb.velocity.y<0){
	    			posObjetivo = posObjetivo + new Vector3(0f,  Mathf.Clamp(rb.velocity.y/3.33f, -15f, 15f), 0f);
	    			// velocidadeAtual = velocidade*1.5f;
	    		}
	    		else if(rb.velocity.y>=0){
	    			posObjetivo = posObjetivo + new Vector3(0f, Mathf.Clamp(rb.velocity.y/3.33f, -15f, 15f), 0f);
	    			// velocidadeAtual = velocidade*1.5f;
	    		}
	    	}
	    	else if(_movementScript.usingCrounch||_movementScript.penduradoBorda){
	    		posObjetivo = posObjetivo + new Vector3(0f, -3, 0f);
	    	}
	    	else{
	    		posObjetivo = posObjetivo + new Vector3(0f, 3f, 0f);
	    	}
    	}
    	//Um target e não é o Player
    	else{
    		posObjetivo = target.position;
    	}

    	//
    	posObjetivo = new Vector3(posObjetivo.x, posObjetivo.y, distancia);
    	//
    	cam.transform.position = Vector3.Lerp(cam.transform.position, posObjetivo, Time.deltaTime*velocidade);
    	cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, rotacao)), Time.deltaTime*velocidadeAtual);
    }

    public static IEnumerator AmpliarPorTempo(float tempo, float distanciaAmplia){
        _cameraController.distancia = distanciaAmplia;
        yield return new WaitForSeconds(tempo);
        _cameraController.distancia = _cameraController.distanciaPadrao;
    }

    public static void Hit(){
        _cameraController.cam.transform.position = _cameraController.cam.transform.position+new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0f);
        _cameraController.StartCoroutine(AmpliarPorTempo(0.01f, _cameraController.distanciaPadrao + 1f));
    }

    public static void Damage(){
        // _cameraController.cam.transform.position = _cameraController.cam.transform.position+new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0f);
        _cameraController.StartCoroutine(AmpliarPorTempo(0.01f, _cameraController.distanciaPadrao + 10f));
    }

    IEnumerator Mostrar(Transform[] arrayAreasDeVisao, float t){
    	_movementScript.isPlaying = false;
    	//
    	for(int i=0; i<arrayAreasDeVisao.Length; i++){
    		target = arrayAreasDeVisao[i];
    		//
    		if(arrayAreasDeVisao[i].lossyScale.x>=arrayAreasDeVisao[i].lossyScale.y){
    			distancia = (arrayAreasDeVisao[i].lossyScale.x)*proporcaoDistanciaX;
    		}
    		else{
    			distancia = (arrayAreasDeVisao[i].lossyScale.y)*proporcaoDistanciaY;
    		}
    		//
    		yield return new WaitForSeconds(t);
    	}
    	//
    	_movementScript.isPlaying = true;
    	//
    	target = transform;
    	distancia = distanciaPadrao;
    }

    Transform[] GetChilds(Transform transformDoObj){
    	Transform[] retorno = new Transform[transformDoObj.childCount];
    	//
    	for(int i=0; i<retorno.Length; i++){
    		retorno[i] = transformDoObj.GetChild(i);
    	}
    	//
    	return retorno;
    }

    void OnTriggerEnter2D(Collider2D col){
    	if(col.gameObject.tag == areaVisaoTag){
    		target = col.gameObject.transform;
    		if(col.transform.localScale.x>=col.transform.localScale.y){
    			distancia = col.transform.localScale.x*proporcaoDistanciaX;
    		}
    		else{
    			distancia = col.transform.localScale.y*proporcaoDistanciaY;
    		}
    	}
    	else if(col.gameObject.tag == areasDeVisaoTag){
    		Transform[] arrayTransform = GetChilds(col.gameObject.transform);
    		StartCoroutine(Mostrar(arrayTransform, 2f+(1f/velocidade)));
    		col.gameObject.SetActive(false);
    	}
    }

    void OnTriggerExit2D(Collider2D col){
    	if(col.gameObject.tag == areaVisaoTag){
    		target = transform;
    		distancia = distanciaPadrao;
    	}
    }
}
