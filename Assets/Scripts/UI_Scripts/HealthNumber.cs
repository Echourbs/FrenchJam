using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthNumber : MonoBehaviour
{
    private Text numberTxt;
    private PlayerHealth ph;

    void Start()
    {
        ph = MovementScript.player.GetComponent<PlayerHealth>();
        numberTxt = GetComponent<Text>();
    }

    
    void Update()
    {
        if(ph.currentHealth >= 0)
        {
            numberTxt.text = (ph.currentHealth).ToString() + " / " + (ph.maxHealth).ToString();
        }
        else if(ph.currentHealth < 0)
        {
            numberTxt.text = "0 / " + (ph.maxHealth).ToString();
        }
    }
}
