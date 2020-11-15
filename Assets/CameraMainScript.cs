using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainScript : MonoBehaviour
{
    GameObject player;

    [Tooltip("distancia minima para começar a mover")]
    public float distanceToMove;

    public float maxVerticalDistance;

    [Tooltip("velocidade maxima da camera")]
    public float maxSpeed;

    [Header("correção de altura do sprite")]
    public float horizontalPlayerMotionCorrection;

    [Header("correção de altura do centro do player (referente ao sprite)")]
    public float playerHeightCorrection;


    

    [Header("fisica do player e correção")]
    Rigidbody2D rbPlayer;
    [Tooltip("afeta quando ")]
    public float velocityUntilPhysicsApplyOnFormula;
    public float physicsHeightCorrectionPercent;

    bool playerRoll;

    Vector3 playerStartPosition;

    Vector3 desirablePosition;

    MovementScript playerMove;

    void Start()
    {
        //encontra o player
        player = MovementScript.player;
        rbPlayer = player.GetComponent<Rigidbody2D>();
        playerStartPosition = player.transform.position;
        playerMove = player.GetComponent<MovementScript>();
    }

    void FixedUpdate()
    {

        //reseta
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = playerStartPosition;
        }

        //correção de posição da camera quando em queda
        float physicsOnFormula = 0;

        if (rbPlayer.velocity.y < velocityUntilPhysicsApplyOnFormula)
        {
            physicsOnFormula = (Mathf.Clamp(rbPlayer.velocity.y, -maxVerticalDistance, maxVerticalDistance) / physicsHeightCorrectionPercent);
        }

        //calculo de posição desejavel da camera

        float height = (player.transform.position.y + playerHeightCorrection) + physicsOnFormula;

        float direction = Input.GetAxis("Horizontal");

        if(playerMove.isRoling>0)
        {
            // direction = playerMove.directionMovement;
        }

        float width = player.transform.position.x + (direction * horizontalPlayerMotionCorrection);

        desirablePosition = new Vector3(width, height, transform.position.z);

        float calculatedDistance = Vector2.Distance(transform.position, desirablePosition);
 
        if (calculatedDistance > distanceToMove)
        {
            transform.position = Vector3.Lerp(transform.position, desirablePosition, maxSpeed * Time.deltaTime);
        }
        
    }


    

}
