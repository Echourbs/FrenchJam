using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotScript : MonoBehaviour
{

    private PlayerInventoryScript inventory;
    public int i;

    private void Start()
    {
        inventory = MovementScript.player.GetComponent<PlayerInventoryScript>();
    }

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[i] = false;
        }
    }

    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<SpawnScript>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }

}
