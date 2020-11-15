using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTrailScript : MonoBehaviour
{
    public Sprite[] fogsPossibles;
    SpriteRenderer mySprite;

    public float lifeTime;
    
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        mySprite.sprite = fogsPossibles[Random.Range(0, fogsPossibles.Length)];
        transform.position = new Vector2(transform.position.x + Random.Range(-0.125f, 0.125f), transform.position.y + Random.Range(-0.125f, 0.125f));
        Destroy(gameObject, lifeTime);
    }

    
    void Update()
    {
        //reduz tamanho e opacidade com o tempo
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime / lifeTime);
        mySprite.color = Vector4.MoveTowards(mySprite.color, Vector4.zero, Time.deltaTime / lifeTime);
    }
}
