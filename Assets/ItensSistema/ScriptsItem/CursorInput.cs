using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorInput : MonoBehaviour
{
	//Static
	public static bool ativo = true;
	public static Vector3 posMouse;
	public static bool mexendoJoystick;
	//Public
	public bool ativar = true;
	public float sensibilidade;
	public float quantoParaMexerMouse;
	//Private
	private Vector3 posParouMouse;
	private Image visual;

	void Start(){
		//Ativar Mouse Input
		ativo = ativar;

		//Cursor
		// Cursor.visible = false;

		//Component
		visual = GetComponent<Image>();

		//posInicial
		transform.position = posMouse;
	}

	void Update(){
		visual.enabled = ativo;
		//
		if(ativo){

			//variavel para facilitar
			bool temInputMouse = Input.GetAxisRaw("Mouse X")!=0||Input.GetAxisRaw("Mouse Y")!=0;
			bool temInputJoystick = Input.GetAxisRaw("HorizontalJoystick")!=0||Input.GetAxisRaw("VerticalJoystick")!=0;

			if(!Input.GetKeyDown("mouse 0")&&Input.GetButtonDown("Click 0")){
				Clicar();
			}

			//Resetar Mouse
			if(mexendoJoystick&&temInputMouse){
				float distancia = Vector3.Distance(posParouMouse, Input.mousePosition);
				if(distancia>quantoParaMexerMouse){
					mexendoJoystick = false;
					transform.position = Input.mousePosition;
				}
			}

			//Mexer com Mouse
			if(!mexendoJoystick){
				transform.position = Input.mousePosition;
			}

			//Resetar Joystick
			if(temInputJoystick&&!mexendoJoystick&&!temInputMouse){
				mexendoJoystick = true;
				posParouMouse = Input.mousePosition;
			}

			//Mexer Com Joystick
			if(temInputJoystick&&mexendoJoystick){
				Vector3 mover = new Vector3(Input.GetAxis("HorizontalJoystick"), Input.GetAxis("VerticalJoystick"), 0f);
				transform.Translate(mover*sensibilidade*Time.deltaTime);
				
				//Nao Sair da Tela em X
				if(transform.position.x>Screen.width){
					transform.position = new Vector3(Screen.width, transform.position.y, transform.position.z);
				}
				else if(transform.position.x<0f){
					transform.position = new Vector3(0f, transform.position.y, transform.position.z);
				}
				
				//Nao Sair da Tela em Y
				if(transform.position.y>Screen.height){
					transform.position = new Vector3(transform.position.x, Screen.height, transform.position.z);
				}
				else if(transform.position.y<0f){
					transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
				}
			}

			//Atualizar variavel
			posMouse = transform.position;
		}
	}

	void Clicar(){
		PointerEventData point = new PointerEventData(EventSystem.current);
		point.position = posMouse;
		//
		List<RaycastResult> results = new List<RaycastResult>();

		EventSystem.current.RaycastAll(point, results);
		foreach(RaycastResult result in results){
			Image image = result.gameObject.GetComponent<Image>();
			if(result.gameObject.activeInHierarchy&&image!=null&&image.enabled){
				Button botao = result.gameObject.GetComponent<Button>();
				if(botao!=null){
					botao.onClick.Invoke();
					break;
				}
			}
		}
	}
}