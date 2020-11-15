using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue hello;
	public Dialogue thx;
	public Dialogue fu;
	public Sprite after;
	private Dialogue _cur;

	void Start()
    {
		_cur = hello;
    }

	public void TriggerDialogue ()
	{
		if (_cur == thx)
        {
			GetComponent<SpriteRenderer>().sprite = after;
        }
		FindObjectOfType<DialogueManager>().StartDialogue(_cur);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Kibo")
		{
			TriggerDialogue();
		}
	}

	public void help()
    {
		_cur = thx;
    }

	public void fail()
    {
		_cur = fu;
	}

	public Dialogue getCur()
    {
		return _cur;
    }

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Kibo")
		{
			FindObjectOfType<DialogueManager>().EndDialogue();
		}
	}

}
