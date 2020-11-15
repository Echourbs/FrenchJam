using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSmartPatrol : MonoBehaviour
{
    [SerializeField]
    bool ColizorDeParede;
    [HideInInspector]
    public bool EncontrouParede,AcabouChao;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            if (ColizorDeParede)
            {
                EncontrouParede = true;
            }
            else
            {
                AcabouChao = false;
            }
            
        }
        
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            if (ColizorDeParede)
            {
                EncontrouParede = false;
            }
            else
            {
                AcabouChao = true;
            }

        }
    }
}
