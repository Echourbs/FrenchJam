using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    #region Player variables
    [Header("Player SFX")]
    public AudioClip[] clips;
    private AudioSource playerAudio;
    #endregion

    #region Enemy Skull variables
    [Header("Enemy Skull SFX")]
    public AudioClip[] skullClips;
    public AudioSource skullAudio;
    #endregion

    #region Enemy Claw variables
    [Header("Enemy Claw SFX")]
    public AudioClip[] clawClips;
    private AudioSource clawAudio;
    #endregion

    #region Enemy Explosive variables
    [Header("Enemy Explosive SFX")]
    public AudioClip[] explosiveClips;
    private AudioSource explosiveAudio;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Get the Audiosource in the GameObject
        playerAudio = GetComponent<AudioSource>();
        //skullAudio = GetComponent<AudioSource>();
        clawAudio = GetComponent<AudioSource>();
        explosiveAudio = GetComponent<AudioSource>();

        if (AudioPref.SFX == 0)
        {
            AudioPref.SFX = 1;
        }

    }
    private void Update()
    {
        
        playerAudio.volume = AudioPref.SFX / 10;
        if (skullAudio != null)
        {
            skullAudio.volume = AudioPref.SFX / 10;
        }
        clawAudio.volume = AudioPref.SFX / 10;
        explosiveAudio.volume = AudioPref.SFX / 10;
    }

    #region Player SFX
    public void StartRunSound()
    {
        playerAudio.clip = clips[0];
        playerAudio.Play();
    }
    
    public void StartJumpSound()
    {
        playerAudio.clip = clips[1];
        playerAudio.Play();
    }
    
    public void StartAttackSound()
    {
        playerAudio.clip = clips[2];
        playerAudio.Play();
    }
    #endregion

    #region Enemy Skull SFX

    public void StartSkullSpawn()
    {
        //skullAudio.volume = 1f;
        skullAudio.clip = skullClips[0];
        skullAudio.Play();
    }

    public void StartSkullAttack()
    {
        //The sound that is in skullClips[1] is the Idle ne 
        skullAudio.volume = 1f;
        skullAudio.clip = skullClips[1];
        skullAudio.Play();
    }

    public void StartSkullDie()
    {
        skullAudio.volume = 0.5f;
        skullAudio.clip = skullClips[2];
        skullAudio.Play();
    }
    #endregion

    #region Enemy Claw SFX
    public void StartClawIdle()
    {
        clawAudio.clip = clawClips[0];
        clawAudio.Play();
    }

    public void StartClawWalk()
    {
        clawAudio.clip = clawClips[1];
        clawAudio.Play();
    }

    public void StartClawWalkHit()
    {
        clawAudio.clip = clawClips[2];
        clawAudio.Play();
    }

    public void StartClawAttack()
    {
        clawAudio.clip = clawClips[3];
        clawAudio.Play();
    }

    public void StartClawHit()
    {
        clawAudio.clip = clawClips[4];
        clawAudio.Play();
    }

    public void StartClawDie()
    {
        clawAudio.clip = clawClips[5];
        clawAudio.Play();
    }

    #endregion

    #region Enemy Explosive SFX
    public void StartExplosiveWalk()
    {
        explosiveAudio.clip = explosiveClips[0];
        explosiveAudio.Play();
    }

    public void StartExplosiveWalkHit()
    {
        explosiveAudio.clip = explosiveClips[1];
        explosiveAudio.Play();
    }

    //this is for the sound with name "Explosive andando HIT ARRASTADO", but i don't know how this sound will work for
    public void StartExplosiveWalkHitLong()
    {
        explosiveAudio.clip = explosiveClips[2];
        explosiveAudio.Play();
    }

    public void StartExplosiveExploding()
    {
        explosiveAudio.clip = explosiveClips[3];
        explosiveAudio.Play();
    }

    public void StartExplosiveReviving()
    {
        explosiveAudio.clip = explosiveClips[4];
        explosiveAudio.Play();
    }

    public void StartExplosiveExplodingLoud()
    {
        explosiveAudio.clip = explosiveClips[5];
        explosiveAudio.Play();
    }

    public void StartExplosiveDie()
    {
        explosiveAudio.clip = explosiveClips[6];
        explosiveAudio.Play();
    }
    #endregion

}
