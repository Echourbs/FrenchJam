using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTrap : MonoBehaviour
{
    public AudioSource source;
    private PlayerHealth health;
    private bool canDamage = false;

    [Header("Trap Settings")]
    public int trapDamage = 5;

    public AudioClip sound1;
    public AudioClip sound2;

    void Awake()
    {
        health = FindObjectOfType<PlayerHealth>();
        source = GetComponent<AudioSource>();
    }

    // evento definido na animação somente quando for true
    private void CanApplyDamageEvent(int state = 1) 
    {
        canDamage = state == 1 ? true : false;
    }

    private void PlaySound1() 
    {
        source.volume = AudioPref.SFX;
        // source.clip = sound1;
        // source.Play();
        source.PlayOneShot(sound2);
        // AudioSource.PlayClipAtPoint(sound1, transform.position, AudioPref.SFX);
    }

    private void PlaySound2() 
    {
        source.volume = AudioPref.SFX;
        // source.clip = sound2;
        // source.Play();
        source.PlayOneShot(sound2);
        // AudioSource.PlayClipAtPoint(sound2, transform.position, AudioPref.SFX);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && canDamage) 
        {
            health.TakeDamage(trapDamage);
            canDamage = false;
        }
    }
}
