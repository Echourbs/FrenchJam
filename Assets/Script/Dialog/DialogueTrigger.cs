using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue hello;
	public Dialogue thx;
	public Dialogue fu;

	public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(hello);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Kibo")
		{
			FindObjectOfType<DialogueManager>().StartDialogue(hello);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Kibo")
		{
			FindObjectOfType<DialogueManager>().EndDialogue();
		}
	}

}
