using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public Image image;
    public Gradient gradient;
    public Image fill;

    [SerializeField]
    bool jogador;
    GameObject player;

    private void Start()
    {
        player = MovementScript.player;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {

        slider.value = health;


        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetHealthByImage(float Health,float MaxHealth)
    {
        image.color = gradient.Evaluate(Health / MaxHealth);
    }

    private void Update()
    {
        if (jogador)
        {
            slider.value = player.GetComponent<PlayerHealth>().currentHealth;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}
