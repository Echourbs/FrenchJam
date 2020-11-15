using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenixScript : MonoBehaviour
{
    private GameObject ancoraFenix;

    public bool hadCapture;
    private MovementScript player;
    public static GameObject fenix;
    bool captureBeforeLoad;

    public bool isEgg;

    private void Awake()
    {
        captureBeforeLoad = PlayerStats.hasFenix;
    }

    void Start()
    {
        print("Captured: " + captureBeforeLoad);

        fenix = this.gameObject;
        if (!hadCapture)
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (!hadCapture)
        {
            gameObject.SetActive(false);
        }
        else
        {
            player = MovementScript.player.GetComponent<MovementScript>();
            
        }
    }

    public void FenixOnLoad()
    {
        if (captureBeforeLoad)
        {
            transform.position = MovementScript.player.GetComponent<MovementScript>().ancoraFenix.transform.position;
        }
    }

    public void IsPlaying()
    {
        player = MovementScript.player.GetComponent<MovementScript>();
        player.isPlaying = true;
    }


}
