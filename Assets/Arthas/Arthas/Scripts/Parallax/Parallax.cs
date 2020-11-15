using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public float maX;
    public float minX;
    //public float origPos;

    void Start()
    {

        minX = cam.transform.position.x;
        maX = minX;
        startpos = transform.position.x;
     
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        

    }

    // Update is called once per frame
    void Update()
    {
        minX = cam.transform.position.x;
        float delta = 0;

        if (maX != minX)
        {
            delta = (minX - maX) * parallaxEffect;
            maX = minX;
        }

        transform.position = new Vector3(transform.position.x + (delta * Time.deltaTime), transform.position.y, 0);
        
        

    }
}
