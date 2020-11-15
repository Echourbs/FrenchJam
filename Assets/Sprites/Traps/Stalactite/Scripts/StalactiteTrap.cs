using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteTrap : MonoBehaviour
{
    private AudioSource source;

    private bool canMove = false;
    private bool canDamage = false; 
    
    [Header("Trap Settings")]
    public int trapDamage = 5;
    public float downVelocity = 0.2f;
    public bool initState = false;

    [Header("Trap Audios")]
    public AudioClip[] clips;
    
    void Awake()
    {
        gameObject.SetActive(initState);
        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            transform.position -= new Vector3(0, downVelocity, 0);
            if (!source.isPlaying) source.Play(); // toca o som de loop
        }
    }

    public IEnumerator InitStatactile() 
    {
        source.PlayOneShot(clips[0]);
        gameObject.SetActive(true);
        canDamage = true;
        canMove = true;
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8) // floor layer
        {
            canMove = false;
            canDamage = false;
            source.PlayOneShot(clips[1]);
        }

        if (other.tag == "Player" && canDamage)
        {
            if (other.TryGetComponent(out PlayerHealth health))
            {
                health.TakeDamage(trapDamage);
                canDamage = false;
            }
        }        
    }
}
