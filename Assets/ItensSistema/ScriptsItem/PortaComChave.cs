using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaComChave : AbrirComChave
{

	public float distanciaPlayer;
  [Header("Animação Porta")]
	public Animator animator;
  //
	// private AbrirComChave _abrirComChave;
	private GameObject player;
	private Collider2D _collider;
	private bool jaVerificado;

    // Start is called before the first frame update
    void Start()
    {
    	base.Start();
    	//
        _collider = GetComponent<Collider2D>();
        animator.speed = 0f;
        player = MovementScript.player;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
   		float distancia = Vector2.Distance(player.transform.position, transform.position);
   		if(distancia<distanciaPlayer){
   			if(!jaVerificado){
   				jaVerificado = true;
   				Abrir();
   			}
   		}
   		else{
   			jaVerificado = false;
   		}
    }

    public void Abrir(){
    	base.Abrir();
    	//
    	_collider.enabled = !aberto;
      if(aberto){
        animator.speed = 1f;
      }
    }
}
