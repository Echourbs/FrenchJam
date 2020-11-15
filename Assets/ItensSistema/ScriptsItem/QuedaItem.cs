using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuedaItem : MonoBehaviour
{
	[Header("Basico")]
	public SpriteRenderer _spriteRenderer;
	public string layerDoChao;
	//
	[Header("Principal")]
    public Item item;
    public int quantos;
    //
    [Header("Animação")]
    public float quantoSobe;
    public float velocidadeAnimacao;
    //
    private Rigidbody2D rb;
    private Collider2D _collider;
    //
    private Vector3 posChao;
    private bool animacaoSubindo = true;
    private float posYAnimacao;
    private bool colidindoComPlayer = false;

    void Start(){
    	rb = GetComponent<Rigidbody2D>();
    	_collider = GetComponent<Collider2D>();
    	//
    	if(item!=null){
    		_spriteRenderer.sprite = item.imagem;
    	}
    }

    void Update(){
        if(Input.GetButton("Interact")&&colidindoComPlayer){
            SistemaItens _sistemaItens = MovementScript.player.GetComponent<SistemaItens>();
            bool adicionouComSucesso = _sistemaItens.AdicionarNovoItem(item, quantos);
            //
            if(adicionouComSucesso){
                Destroy(gameObject);
            }
        }
    }

    void FixedUpdate(){
    	bool noChao = Physics2D.Raycast(transform.position, Vector3.down, 0.1f, 1<<LayerMask.NameToLayer(layerDoChao));
    	//
    	if(noChao){
            rb.velocity = new Vector2(0f, 0f);
            rb.gravityScale = 0;
    		_collider.enabled = false;
    		//
    		Animar();
    	}
    	else{
            rb.gravityScale = 1;
    		_collider.enabled = true;
    	}
        //
    }

    void Animar(){
    	float objetivoValor = 0;
    	if(animacaoSubindo){
    		objetivoValor = quantoSobe;
    	}
    	//
    	posYAnimacao = Mathf.MoveTowards(posYAnimacao, objetivoValor, Time.deltaTime*velocidadeAnimacao);
    	//
    	float distanciaObjetivo = Mathf.Abs(posYAnimacao-objetivoValor);
    	if(distanciaObjetivo<=0.01f){
    		animacaoSubindo = !animacaoSubindo;
    		posYAnimacao = objetivoValor;
    	}
    	//
    	_spriteRenderer.transform.position = new Vector3(transform.position.x, transform.position.y+0.5f+posYAnimacao, transform.position.z);
    }

    void OnTriggerEnter2D (Collider2D col){
    	if(col.gameObject==MovementScript.player){
            colidindoComPlayer = true;
        }
    }

    void OnTriggerExit2D (Collider2D col){
        if(col.gameObject==MovementScript.player){
            colidindoComPlayer = false;
        }
    }
}   
