using System.Collections;
using UnityEngine;

/// <summary>
/// Created by Marcelino Borges.
/// 
/// Small rat in enviroment, which will run from a side to another
/// in random points between this object and the maxBound object (child of this gameobject)
/// </summary>
public class RatEnviroment : MonoBehaviour {
    #region PROPERTIES
    [Tooltip("Reference child gameobject called MaxBound")]
    public Transform maxBound;
    public float moveSpeed = 5f;
    [MinMaxSlider(.1f,5f)]
    [Tooltip("Choose min and max time the rat will idle")]
    public Vector2 timeIdling = new Vector2(.5f,1f);

    private Animator animator;
    private Vector3 moveDirection, nextTargetPoint, ratStartPosition, boundPosition;
    private bool moving = true, facingLeft = true;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region METHODS
    void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start() {
        if (maxBound == null)
            throw new MissingReferenceException("Referenciar o gameobject filho chamado MaxBound");
        ratStartPosition = transform.position;
        boundPosition = maxBound.position;
        moveDirection = Vector3.Normalize(maxBound.position - transform.position);
        nextTargetPoint = maxBound.position;
        SetMoving();
    }

    void Update() {
        if (moving) {
            if (Vector3.Distance(transform.position, nextTargetPoint) > 0.1f) {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);             
            } else {
                StartCoroutine(SetIdling());
                SortNextTargetPoint();
            }
        } 
    }

    /// <summary>
    /// Sets idle animation and how long the rat will keep at this state
    /// After that time, calls to moving state
    /// </summary>    
    private IEnumerator SetIdling() {
        moving = false;
        animator.SetBool("moving", false);
        float idlingTime = Random.Range(timeIdling.x, timeIdling.y);
        yield return new WaitForSeconds(idlingTime);
        SetMoving();
    }

    /// <summary>
    /// Sets movement animation
    /// </summary>
    private void SetMoving() {
        moving = true;
        animator.SetBool("moving", true);
    }

    /// <summary>
    /// Picks a random point inside the line formed from the start position of maxBound gameobject and the rat.
    /// Then sets the correct direction vector to that point
    /// and checks if needs to flip the sprite
    /// </summary>    
    private void SortNextTargetPoint() {
        float relativePoint = Random.Range(0f, 1f);
        nextTargetPoint = LerpByDistance(boundPosition, ratStartPosition, relativePoint);
        moveDirection = Vector3.Normalize(nextTargetPoint - transform.position);
        CheckDirectionAndFlip();
    }

    /// <summary>
    /// Returns a point between 2 positions relative to an alpha.
    /// </summary>
    /// <param name="A">Position 1</param>
    /// <param name="B">Position 2</param>
    /// <param name="alpha">0 to 1, meaning distance percentage between the 2 positions</param>
    /// <returns>The actual position</returns>
    public Vector3 LerpByDistance(Vector3 A, Vector3 B, float alpha) {
        Vector3 P = alpha * (B - A) + A;
        return P;
    }

    /// <summary>
    /// Checks the movement direction and flips accordingly
    /// </summary>
    private void CheckDirectionAndFlip() {
        if(moveDirection.x > 0) {
            if (!spriteRenderer.flipX)
                spriteRenderer.flipX = true;
        } else {
            if (spriteRenderer.flipX)
                spriteRenderer.flipX = false;
        }
    }

    /// <summary>
    /// Showing the trajectory line and the gameobject is selected at the scene
    /// </summary>
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(maxBound.position, transform.position);
    }
    #endif

    #endregion
}
