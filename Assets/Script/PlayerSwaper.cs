using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerSwaper : MonoBehaviour
{
    public GameObject shinko_class;
    private GameObject kibonoki;
    private GameObject shinko;
    private Camera2DFollow camera;
    private GameObject focus;


    // Start is called before the first frame update
    void Start()
    {
        kibonoki = GameObject.Find("Kibonoki");
        focus = kibonoki;

        camera = GameObject.Find("Camera").GetComponent<Camera2DFollow>();
    }

    void swap()
    {
        if (shinko)
        {
            shinko.GetComponent<Platformer2DUserControl>().enabled = !shinko.GetComponent<Platformer2DUserControl>().enabled;
            kibonoki.GetComponent<Platformer2DUserControl>().enabled = !kibonoki.GetComponent<Platformer2DUserControl>().enabled;

            focus.GetComponent<Animator>().SetFloat("Speed", 0);
            focus = focus == kibonoki ? shinko : kibonoki;
            camera.target = focus.transform;
        }
    }

    void spawn()
    {
        if (focus == kibonoki)
        {
            if (shinko)
            {
                Destroy(shinko);
                shinko = null;
            }
            else
            {
                bool direction = kibonoki.GetComponent<PlatformerCharacter2D>().facingRight();

                shinko = Instantiate(shinko_class, kibonoki.transform.position + new Vector3(direction ? 1 : -1, 0), kibonoki.transform.rotation);
                if (!direction)
                {
                    shinko.GetComponent<PlatformerCharacter2D>().Flip();
                }
                swap();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            swap();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            spawn();
        }
    }
}
