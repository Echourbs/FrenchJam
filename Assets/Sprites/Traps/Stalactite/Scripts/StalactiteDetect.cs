using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteDetect : MonoBehaviour
{
    private bool isStarted = false;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && !isStarted)
        {
            isStarted = true;
            StartCoroutine(transform.parent.GetChild(0).GetComponent<StalactiteTrap>().InitStatactile());
        }
    }
}
