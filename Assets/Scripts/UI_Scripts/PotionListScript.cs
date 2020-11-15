using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionListScript : MonoBehaviour
{
    GameObject player;

    [SerializeField]
    Image slotPotion;

    void Start()
    {
        player = MovementScript.player;
    }

    void Update()
    {
        //Slot 1 da poção
        if (player.GetComponent<PlayerInventoryScript>().isFull[0])
        {
            slotPotion.enabled = true;
        }
        else
        {
            slotPotion.enabled = false;
        }
    }
}
