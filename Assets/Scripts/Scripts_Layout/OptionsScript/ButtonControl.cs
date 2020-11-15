using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public GameObject canvasControle, canvasTeclado;

    public void TelaControle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ControlScene");
    }


    private void Update()
    {
        //Get Joystick Names
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
}
