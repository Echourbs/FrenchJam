using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    public int damage;

    [SerializeField]
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

 
    void Update()
    {
        
    }
}
