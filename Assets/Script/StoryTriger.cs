using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriger : MonoBehaviour
{
    public DialogueTrigger toTrigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Kibo")
        {
            toTrigger.help();
            Destroy(gameObject);
        }
    }
}
