using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject item;
    private Transform player;

    private void Start()
    {
        player = MovementScript.player.transform;
    }

    public void SpawnDroppedItem()
    {
        Vector2 playerPos = new Vector2(player.position.x + 3, player.position.y);
        Instantiate(item, playerPos, Quaternion.identity);
    }
}
