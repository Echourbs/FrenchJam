using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulo : MonoBehaviour
{
    



    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    
    Rigidbody2D rb;





    MovementScript playerControler;


    public bool performingImpulsionedFall;
   
    bool canPerformFallImpulse;

    [SerializeField]
    float fallImpulse;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControler = GetComponent<MovementScript>();
        canPerformFallImpulse = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControler.climbingLadder == false)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
        
        
       
        
        if(playerControler.climbingLadder == false)
        {
            if (playerControler.onGround == false)
            {
                if (canPerformFallImpulse == true)
                {
                    //print(Input.GetAxisRaw("Vertical"));
                    if (Input.GetButtonDown("Crounch"))
                    {
                        print("performed");
                        rb.AddForce(Vector2.down * fallImpulse);
                        performingImpulsionedFall = true;
                        canPerformFallImpulse = false;
                    }
                }
            }
            else
            {
                performingImpulsionedFall = false;
                canPerformFallImpulse = true;
            }
        }
        else
        {
            canPerformFallImpulse = true;
        }

        

    }




}
