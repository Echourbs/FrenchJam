using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Marcelino Borges.
/// 
/// Bats particles that can get scared and fly away.
/// Place more sprites as children of this gameobject if desired.
/// </summary>
public class BatsScaredParticles : MonoBehaviour {
    #region PROPERTIES
    public ParticleSystem batsParticlesSystem;
    public string playerTag = "Player";
    [Tooltip("Bats idle in the scene (children of this prefab)")]
    public GameObject[] batsIdle;
    [Range(0f,1f)]
    [Tooltip("Chance to bats get scared (0 to 1, meaning 0% to 100%)")]
    public float chanceToGetScared = .5f; 
    #endregion

    #region METHODS
    void Start() {
        if (batsParticlesSystem == null)
            throw new MissingReferenceException("Referenciar o sistema de partículas dos morcegos");
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(playerTag)) {
            PlayBats();
        }
    }

    /// <summary>
    /// Play bats particle system, hide the idle bats sprites and schedule to destroy this object after 5s
    /// </summary>
    void PlayBats() {
        if (Random.Range(0f, 1f) < chanceToGetScared) {
            HideBatsIdle();
            batsParticlesSystem.Play();
            Destroy(gameObject, 5f);
        }
    }

    /// <summary>
    /// Hides all idle bats gameobjects
    /// </summary>
    void HideBatsIdle() {
        //Checks if the array is valid
        if (batsIdle != null && batsIdle.Length > 0) {
            foreach (GameObject bat in batsIdle)
                //Deactivate each bat
                bat.SetActive(false);
        }
    }
    #endregion
}
