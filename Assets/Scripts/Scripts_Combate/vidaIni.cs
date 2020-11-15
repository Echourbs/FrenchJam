using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vidaIni : MonoBehaviour
{
    public float maxVida;
    public Image barraVida;
    public HealthBarScript GradientHealthBar;

    [HideInInspector]
    public float vida;

    void Start()
    {
        vida = maxVida;
    }

    
    void Update()
    {
        barraVida.fillAmount = vida / maxVida;

        GradientHealthBar.SetHealthByImage(vida, maxVida);
    }
}
