using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public abstract class Enemy : MonoBehaviour 
{
    [Header("ENEMY CLASS PROPERTIES")]
    public GameObject deathParticlesPrefab;
    public GameObject takeDamageParticlesPrefab;
    public AudioClip deathSfx, takeDamageSfx, disappearSfx;

    public bool dropCrystalOnDeath = false, canBeDamaged = true, dead, canInflictDamage = true;
    public float deathParticlesHeight, damageCooldownTime = 1f, timeToDisappearWhenDead = .25f;
    public int xpPoints;
    
    //public int currentHealth, maxHealth, xpPoints;
    //OnKillPlayerEvent: Everytime an enemy hurts the player, player will check damage and if he dies from that damage, player calls this event
    public UnityEvent OnKillPlayerEvent, OnDieEvent;

    [SerializeField]
    private int damage;
    public int Damage {
        set => damage = value;
        get => canInflictDamage ? damage : 0;
    }

    protected AudioSource audioSource;
    protected Animator animator;
    protected Rigidbody2D rb;

    //Implement in children
    public abstract void OnKillPlayer();
    protected virtual void ResetEnemy() { }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start() {

        if (OnDieEvent == null) 
            OnDieEvent = new UnityEvent();

        if (OnKillPlayerEvent == null)
            OnKillPlayerEvent = new UnityEvent();

        OnKillPlayerEvent.AddListener(OnKillPlayer);
    }
    
    public virtual void TakeDamage(int damageAmount) 
    {
        if (canBeDamaged) 
        {
            PlayDamageSfx();
            if (takeDamageParticlesPrefab != null) {
                GameObject hit = Instantiate(takeDamageParticlesPrefab, transform.position, Quaternion.identity);
                Destroy(hit, 2f);
            }
            SetCanNotBeDamaged();
            Invoke("SetCanBeDamaged", damageCooldownTime);
        }
    }

    public virtual void Die() 
    {
        dead = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        var cols = GetComponents<Collider2D>();
        if (cols != null) {
            foreach (Collider2D collider in cols) {
                collider.enabled = false;
            }
        }

        OnDieEvent.Invoke();
        StartCoroutine(DestroyEnemy());

        if (deathParticlesPrefab != null) {
            Vector3 effectPosition = new Vector3(transform.position.x, transform.position.y, 0);
            GameObject pickupEffect = Instantiate(deathParticlesPrefab, effectPosition, Quaternion.identity);
        }
        if (deathSfx != null) AudioSource.PlayClipAtPoint(deathSfx, gameObject.transform.position);
    }

    public virtual void Flip() => transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    public virtual void SetCanNotBeDamaged() => canBeDamaged = false;
    public virtual void SetCanBeDamaged() => canBeDamaged = true;
    
    protected virtual void PlayDamageSfx() 
    {
        if (takeDamageSfx != null)
            AudioSource.PlayClipAtPoint(takeDamageSfx, transform.position);
    }

    public virtual void PlaySound(AudioClip audioClip) 
    {
        if (!SoundManager.instance.isMuted)
            if (audioSource != null && audioClip != null)
                audioSource.PlayOneShot(audioClip, SoundManager.instance.maxVolume);
    }

    protected virtual IEnumerator DestroyEnemy() 
    {
        yield return new WaitForSeconds(timeToDisappearWhenDead);
        Destroy(gameObject);
    }
}
