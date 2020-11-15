using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public bool areOverlaping = false;
    public GameObject player;

    public void activate()
    {
        areOverlaping = false;
        player = MovementScript.player;
        transform.parent = null;
    }

    void FixedUpdate()
    {
        transform.position = player.transform.position + new Vector3(0, -1.625f,0);
    }


    public void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            areOverlaping = true;
        }
    }

    public void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            areOverlaping = false;
        }
    }


}
