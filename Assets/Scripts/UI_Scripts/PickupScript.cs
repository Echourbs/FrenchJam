using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    private PlayerInventoryScript inventory;
    public GameObject itemButton;

    private void Start()
    {
        inventory = MovementScript.player.GetComponent<PlayerInventoryScript>();
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject == MovementScript.player)
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    //ITEM PODE SER ADICIONADO AO INVENTARIO
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }

            }
        }
    }
}
