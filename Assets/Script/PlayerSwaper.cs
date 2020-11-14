using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerSwaper : MonoBehaviour
{
    private GameObject kibonoki;
    private GameObject shinko;
    private Camera2DFollow camera;
    private Platformer2DUserControl[] controls;
    private GameObject focus;


    // Start is called before the first frame update
    void Start()
    {
        kibonoki = GameObject.Find("Kibonoki");
        shinko = GameObject.Find("Shinko");
        focus = kibonoki;

        camera = GameObject.Find("Camera").GetComponent<UnityStandardAssets._2D.Camera2DFollow>();

        controls = new Platformer2DUserControl[] { kibonoki.GetComponent<Platformer2DUserControl>(), shinko.GetComponent<Platformer2DUserControl>() };
    }

    void swap()
    {
        foreach (Platformer2DUserControl i in controls)
        {
            i.enabled = !i.enabled;
        }

        focus.GetComponent<Animator>().SetFloat("Speed", 0);

        focus = focus == kibonoki ? shinko : kibonoki;

        camera.target = focus.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            swap();
        }
    }
}
