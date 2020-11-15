using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float velocidade;
    public float tempoDeVida;

 
    void Start()
    {
        // DESTROI SOMENTE A BALA NA COLISAO
        Destroy(gameObject, tempoDeVida);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Enemy")
        {
            // DESTROI O INIMIGO E A BALA ATRAVES DE UMA COLISAO
            Destroy(c.gameObject);
            Destroy(gameObject);
        }
        else
        {
            // DESTROI SOMENTE A BALA ATRAVES DE UMA COLISAO
            Destroy(gameObject);
        }
    }


    void Update()
    {
        transform.Translate(Vector2.right * velocidade * Time.deltaTime);
    }
}
