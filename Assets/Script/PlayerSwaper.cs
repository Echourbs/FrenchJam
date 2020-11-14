using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class PlayerSwaper : MonoBehaviour
{
    public GameObject shinko_class;
    private GameObject _kibonoki;
    private GameObject _shinko;
    private Camera2DFollow _camera;
    private GameObject _focus;


    // Start is called before the first frame update
    void Start()
    {
        _kibonoki = GameObject.Find("Kibonoki");
        _focus = _kibonoki;

        _camera = GameObject.Find("Camera").GetComponent<Camera2DFollow>();
    }

    void swap()
    {
        if (_shinko)
        {
            _shinko.GetComponent<Platformer2DUserControl>().enabled = !_shinko.GetComponent<Platformer2DUserControl>().enabled;
            _kibonoki.GetComponent<Platformer2DUserControl>().enabled = !_kibonoki.GetComponent<Platformer2DUserControl>().enabled;

            _focus.GetComponent<Animator>().SetFloat("Speed", 0);
            _focus = _focus == _kibonoki ? _shinko : _kibonoki;
            _camera.target = _focus.transform;
        }
    }

    void spawn()
    {
        if (_focus == _kibonoki)
        {
            if (_shinko)
            {
                Destroy(_shinko);
                _shinko = null;
            }
            else
            {
                bool direction = _kibonoki.GetComponent<PlatformerCharacter2D>().facingRight();

                _shinko = Instantiate(shinko_class, _kibonoki.transform.position + new Vector3(direction ? 1 : -1, 0), _kibonoki.transform.rotation);
                if (!direction)
                {
                    _shinko.GetComponent<PlatformerCharacter2D>().Flip();
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
