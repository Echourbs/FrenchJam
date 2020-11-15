using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    //Distancia para começar a seguir o player
    public float distance = 7f;

    //posicao do player, pego no void start
    private Transform target;

    //velocidade
    public float speed = 20f;

    //adiciona a posição do alvo para correção
    Vector3 targetCoef = new Vector3(0, -1.5f, 0);

    //multiplicador randomico da velocidade
    float randomPercent;

    bool podePerseguirPlayer;

    private void Start()
    {
        //procura o player
        target = MovementScript.player.transform;
        //escolhe um bonus randomico para a velocidade (nota, como é porcentagem, pode reduzir tambem)
        randomPercent = Random.Range(0.95f,1.25f);
        //
        StartCoroutine(TempoPerseguirPlayer());
    }

    void Update()
    {
        //verifica a distancia con o player
        float atualDistanceFromPlayer = Vector2.Distance(transform.position, target.transform.position + targetCoef);

        //segue o player se estiver no alcance
        if (atualDistanceFromPlayer < distance && podePerseguirPlayer)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, target.position + targetCoef, (speed * randomPercent) * Time.deltaTime);

            //adiciona moeda para o jogador se estiver proximo
            if (atualDistanceFromPlayer < 0.35f)
            {
                SimpleHudScript.coinAmount+=10;
                Destroy(this.gameObject);
            }
        }
           
        
    }

    IEnumerator TempoPerseguirPlayer(){
        yield return new WaitForSeconds(0.5f);
        podePerseguirPlayer = true;
    }

 
}
