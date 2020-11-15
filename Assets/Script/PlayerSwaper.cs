using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using Jam;

public class PlayerSwaper : MonoBehaviour
{
    public GameObject shinko_class;
    private GameObject _kibonoki;
    private GameObject _shinko;
    private Camera2DFollow _camera;
    private GameObject _focus;
    private Vector3 _tpLocation;


    // Start is called before the first frame update
    void Start()
    {
        _kibonoki = GameObject.Find("Kibo");
        _focus = _kibonoki;
        _kibonoki.transform.Find("Sprite").GetComponent<KiboAnim>().fadeOutEnd.AddListener(OnFadeOutEnd);

        _camera = GameObject.Find("Camera").GetComponent<Camera2DFollow>();
    }

    void OnFadeOutEnd()
    {
        _kibonoki.transform.position = _tpLocation;
        _kibonoki.GetComponent<Kibo>().endTp();
    }

    void swap()
    {
        if (_shinko)
        {
            _shinko.GetComponent<ShinkoUserControl>().enabled = !_shinko.GetComponent<ShinkoUserControl>().enabled;
            _kibonoki.GetComponent<KiboUserControl>().enabled = !_kibonoki.GetComponent<KiboUserControl>().enabled;

            (_focus.GetComponent(_focus == _kibonoki ? typeof(Kibo) : typeof(Shinko)) as ICharacter).stop();
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
                bool direction = _kibonoki.GetComponent<Kibo>().facingRight();

                _shinko = Instantiate(shinko_class, _kibonoki.transform.position + new Vector3(direction ? 1 : -1, 0), _kibonoki.transform.rotation);
                if (!direction)
                {
                    _shinko.GetComponent<Shinko>().Flip();
                }
                swap();
            }
        }
    }

    public void tpKibo(Vector3 position)
    {
        if (_shinko)
        {
            swap();
            spawn();
            _tpLocation = position;
            _kibonoki.GetComponent<Kibo>().startTp();
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
