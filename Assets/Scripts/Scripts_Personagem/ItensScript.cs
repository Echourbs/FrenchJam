using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItensScript : MonoBehaviour
{
    public void Potion()
    {
        PlayerHealth ph = FindObjectOfType<PlayerHealth>();
        if(ph.currentHealth < 100)
        {
            ph.currentHealth += 20;
        }
        ph.healthBar.SetHealth(ph.currentHealth);
        Destroy(gameObject);
    }

}
