using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    bool inventoryPressed;
    public GameObject uiInventory;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPressed)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }

    }

    void CloseInventory()
    {
        uiInventory.SetActive(false);
        inventoryPressed = false;
        Time.timeScale = 1f;
        
    }

    public void OpenInventory()
    {
        uiInventory.SetActive(true);
        inventoryPressed = true;
        Time.timeScale = 0f;
        
    }
}

