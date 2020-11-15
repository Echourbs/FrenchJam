using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject canvasControle, canvasTeclado;
    IndividualButtonScript btns;
    BaseEventData eventData;

    private void Start()
    {
        string[] temp = Input.GetJoystickNames();

        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    //Not empty, controller temp[i] is connected
                    //Debug.Log("Controller " + i + " is connected using: " + temp[i]);
                    canvasControle.SetActive(true);
                    canvasTeclado.SetActive(false);
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    //Debug.Log("Controller: " + i + " is disconnected.");
                    canvasControle.SetActive(false);
                    canvasTeclado.SetActive(true);
                }
            }
        }
    }

    private void Update()
    {
        string[] temp = Input.GetJoystickNames();

        //Check whether array contains anything
        if (temp.Length > 0)
        {
            //Iterate over every element
            for (int i = 0; i < temp.Length; ++i)
            {
                //Check if the string is empty or not
                if (!string.IsNullOrEmpty(temp[i]))
                {
                    //Not empty, controller temp[i] is connected
                    if (Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") > 0)
                    {
                        canvasControle.SetActive(false);
                        canvasTeclado.SetActive(true);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;                   
                    }
                    else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
                    {
                        canvasControle.SetActive(false);
                        canvasTeclado.SetActive(true);
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                    else if (Input.GetAxisRaw("ArrowPadVertical") != 0 || Input.GetAxisRaw("ArrowPadHorizontal") != 0)
                    {
                        canvasControle.SetActive(true);
                        canvasTeclado.SetActive(false);
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }                 
                }
                else
                {
                    //If it is empty, controller i is disconnected
                    //where i indicates the controller number
                    canvasControle.SetActive(false);
                    canvasTeclado.SetActive(true);
                }
            }
        }
    }

    public void Control()
    {
        
    }

    public void Keyboard()
    {
        
    }
}
