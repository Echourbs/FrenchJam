using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FenixGenerator : MonoBehaviour
{
    [HideInInspector]
    public CinemachineVirtualCamera cine;
    private MovementScript player;
    public GameObject eggFenix;

    private void Awake()
    {

    }

    private void Start()
    {
        cine = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        print("carregou: " + PlayerStats.isLoad);
        if (PlayerStats.hasFenix && PlayerStats.isLoad)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            PlayerStats.hasFenix = true;
            player = MovementScript.player.GetComponent<MovementScript>();
            player.isPlaying = false;
            player.GetComponent<Animator>().SetBool("RUN", false);
            player.GetComponent<Animator>().SetBool("ISGROUNDED", true);
            player.GetComponent<Animator>().SetBool("IsOnFallinLoopState", false);
            player.GetComponent<Animator>().SetBool("IsFallin", false);
            eggFenix.GetComponent<EggFenix>().EggAnimTrigger();
            //c.GetComponent<MovementScript>().EnableFenix();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        cine.LookAt = MovementScript.player.transform;
        cine.Follow = MovementScript.player.transform;
    }


}
