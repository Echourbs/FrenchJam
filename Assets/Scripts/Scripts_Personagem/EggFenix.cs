using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EggFenix : MonoBehaviour
{
    private FenixScript fs;
    private MovementScript player;
    public static bool finish;
    public CinemachineVirtualCamera cine;
    bool alreadyHasFenix;

    void Start()
    {
        cine = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        if (PlayerStats.hasFenix)
        {
            alreadyHasFenix = true;
        }
        else
        {
            alreadyHasFenix = false;
        }
    }

    public void EggAnimTrigger()
    {
        player = MovementScript.player.GetComponent<MovementScript>();
        player.GetComponent<Animator>().SetBool("RUN", false);
        player.GetComponent<Animator>().SetBool("ISGROUNDED", true);
        player.GetComponent<Animator>().SetBool("IsOnFallinLoopState", false);
        GetComponent<Animator>().SetTrigger("GLOW");
    }

    public void EnterGlow()
    {
        cine.LookAt = transform;
        cine.Follow = transform;
    }

    public void FinishGlow()
    {
        MovementScript.player.GetComponent<MovementScript>().EnableFenix();
        finish = true;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Invoke("EnterDialogue", 0.3f);
        Destroy(gameObject, 0.5f);
    }

    void EnterDialogue()
    {
        GetComponent<DialogueTrigger>().TriggerDialogue();
    }
}
