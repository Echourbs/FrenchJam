using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsTrap : MonoBehaviour
{
    private AudioSource source_;
    private PlayerHealth health_;
    private Animator anim_;
    private bool isApplying = false;
    private bool canApplyDamage = false;

    [Header("Trap Settings")]
    public int trapDamage = 5;
    public float coolDownTime = 0.5f;

    [Header("Trap Audios")]
    public AudioClip audioIn;
    public AudioClip audioOut;
    public AudioClip audioReady;

    public bool activeForTest;
    private bool asActiveForTest;


    void Awake()
    {
        health_ = FindObjectOfType<PlayerHealth>();
        anim_ = GetComponent<Animator>();
        source_ = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (activeForTest && !asActiveForTest)
        {
            asActiveForTest = true;
            StartCoroutine(ApplyDamage());
        }
    }

    // evento definido na animação somente quando for true
    private void CanApplyDamageEvent(int state = 1) 
    {
        canApplyDamage = state == 1 ? true : false;
    }

    private void PlaySound1()
    {
        source_.PlayOneShot(audioIn);
    }

    private void PlaySound2() 
    {
        source_.PlayOneShot(audioOut);
    }

    private void PlaySound3() 
    {
        source_.PlayOneShot(audioReady);
    }

    // Ativa a animação e recebimento de dano
    // Aplica o dano
    private void OnTriggerStay2D(Collider2D other)
    {        
        if (other.tag == "Player" && canApplyDamage && isApplying)
        {
            canApplyDamage = false;
            health_.TakeDamage(trapDamage);
        }
        else if (other.tag == "Player" && !isApplying)
        {
            isApplying = true;
            StartCoroutine(ApplyDamage());
        }        
    }

    private IEnumerator ApplyDamage() 
    {
        // Espera a animação
        anim_.SetTrigger("ActiveThorns");
        while (!anim_.GetCurrentAnimatorStateInfo(0).IsName("ThornsAnimation")) yield return new WaitForSeconds(Time.deltaTime);

        // Enquanto a animação estiver tocando pode receber dano
        // canApplyDamage definido por evento na animação
        yield return new WaitUntil( () => !anim_.GetCurrentAnimatorStateInfo(0).IsName("ThornsAnimation"));
        canApplyDamage = false;

        // Termino da animação
        yield return new WaitForSeconds(coolDownTime);
        isApplying = false;
        asActiveForTest = false;
    }
}
