using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsScared : MonoBehaviour {
    #region PROPERTIES
    public string playerTag = "Player";
    [Range(0f, 1f)]
    [Tooltip("Chance to birds get scared (0 to 1, meaning 0% to 100%)")]
    public float chanceToGetScared = .5f;
    [MinMaxSlider(0.5f,1.5f)]
    public Vector2 timeIdleFrozen;
    [SerializeField]
    private GameObject birdFlyingUpGameObject;
    private Animator animator;
    #endregion

    #region METHODS
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        if (birdFlyingUpGameObject == null)
            throw new MissingReferenceException("Setar referência do prefab do birdFlyingUpGameObject");
        SetIdleFrozen();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(playerTag)) {
            SpawnBirdUp();
        }
    }

    /// <summary>
    /// Sets the frozen idle state (1 frame animation)
    /// </summary>
    public void SetIdleFrozen() {
        StartCoroutine(SetFrozenTimer());
    }

    /// <summary>
    /// Wait a random time between timeIdleFrozen bounds and calls the next animation
    /// </summary>
    /// <returns></returns>
    IEnumerator SetFrozenTimer() {
        yield return new WaitForSeconds(Random.Range(timeIdleFrozen.x, timeIdleFrozen.y));
        CallNextRandomAnimation();
    }

    /// <summary>
    /// Picks a random animation between [2,3,4] animator triggers and call it
    /// </summary>
    void CallNextRandomAnimation() {
        animator.SetTrigger(Random.Range(2, 5).ToString());
    }

    /// <summary>
    /// Spawns this bird's particle system flying up
    /// </summary>
    void SpawnBirdUp() {
        if (Random.Range(0f, 1f) < chanceToGetScared) {
            if (birdFlyingUpGameObject != null)
                Instantiate(birdFlyingUpGameObject, transform.position, Quaternion.identity);            
            Destroy(gameObject);
        }
    }
    #endregion
}
