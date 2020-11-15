using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFogDamage : MonoBehaviour
{
    [SerializeField]
    float damageDealt;

    [SerializeField]
    float speed;

    float pseudoSpeed = 0;
    [SerializeField]
    float timeUntilOrbReachsMaxSpeed;


    [SerializeField]
    float heightCorrection;
    Transform playerPosition;

    [SerializeField]
    bool orbHasAceleration;

    [SerializeField]
    float releaseDistance;
    bool reached = false;

    public GameObject fogTrail;
    [SerializeField]
    float delayFogGeneration;

    Transform pseudoTarget;

    Vector3 dir;


    void Start()
    {



        pseudoTarget = new GameObject().transform;
        
        pseudoTarget.name = "pseudoTarget";

        reached = false;
        playerPosition = MovementScript.player.transform;
        GenerateTrail();

        if (orbHasAceleration == false)
        {
            pseudoSpeed = speed;
        }

        pseudoTarget.position = playerPosition.position + new Vector3(0, heightCorrection, 0);
    }

    void GenerateTrail()
    {
        Instantiate(fogTrail, transform.position, fogTrail.transform.rotation);

        Invoke("GenerateTrail", delayFogGeneration);
    }

    void Update()
    {


        //aumenta vblocidade da orb
        if (orbHasAceleration)
        {
            pseudoSpeed = Mathf.MoveTowards(pseudoSpeed, speed, (speed / timeUntilOrbReachsMaxSpeed) * Time.deltaTime);

            //para de acelerar caso esteja na velocidade maxima
            if(pseudoSpeed == speed)
            {
                orbHasAceleration = false;
            }
        }


        if(reached == false)
        {
            pseudoTarget.position = playerPosition.position + new Vector3(0, heightCorrection, 0);
            
            dir = (pseudoTarget.position - transform.position).normalized;
            //Debug.DrawLine(transform.position, pseudoTarget.position, Color.blue);

            if(Vector3.Distance(transform.position, pseudoTarget.position) < releaseDistance)
            {
                reached = true;
            }

        } 
        

        transform.Translate(dir * pseudoSpeed * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {   
        if(pseudoTarget != null)
        {
            if (reached == false)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, pseudoTarget.position);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, dir * 2);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.gameObject == MovementScript.player)
        {
            MovementScript.player.GetComponent<PlayerHealth>().TakeDamage(Mathf.RoundToInt(damageDealt));
            Destroy(pseudoTarget.gameObject);
            Destroy(gameObject);
        }
        else if(Col.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            Destroy(pseudoTarget.gameObject);
            Destroy(gameObject);
        }
    }

}
