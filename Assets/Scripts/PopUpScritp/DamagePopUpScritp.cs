using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUpScritp : MonoBehaviour
{

    float sizeValue;


    //priprio componente de texto
    //TextMesh myAsText;
    Text myAsText;

    //alpha da cor do texto para fins de fade
    float alpha;
    float newValueSize;


    void Start()
    {
        newValueSize = 1f;

        //impede o popup de nascer invertido
        if(transform.eulerAngles != Vector3.zero)
        {
            transform.eulerAngles = Vector3.zero;
        }
        
    }

    void Update()
    {
        //inicia o fade apos o popup ser gerado
        if(myAsText != null)
        {
            //troca a cor / da o fade no popup
            alpha = Mathf.MoveTowards(alpha, 0, 1.4f * Time.deltaTime);
            myAsText.color = new Color(myAsText.color.r, myAsText.color.g, myAsText.color.b, alpha);

            //troca o tamanho/ reduz o tamanho do popup
            newValueSize = Mathf.MoveTowards(newValueSize,0, 1.25f * Time.deltaTime);
            myAsText.transform.localScale = new Vector3(newValueSize, newValueSize, transform.localScale.z);

            //deleta o popup quando ficar invisivel
            if (alpha == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //void de instancia
    public void SetDamageValue(int displayableDamage)
    {
        // pega o componente, o valor do alpha da cor e troca tamanho e troca o texto para o dano
        //myAsText = this.GetComponent<TextMesh>();
        
        //caça o componente de texto nele mesmo ou em um objeto parenteado
        if(GetComponent<Text>() == true)
        {
            myAsText = this.GetComponent<Text>();
        }
        else
        {
            myAsText = this.GetComponentInChildren<Text>();
        }


        alpha = myAsText.color.a;
        myAsText.transform.localScale = new Vector3(sizeValue,sizeValue, transform.localScale.z);
        newValueSize = sizeValue;
        myAsText.text = (displayableDamage.ToString());
    }


    //como utilizar
    //no outro codigo, aonde tem o dano, crie um public GameObject para o prefab do popup e o adicione no inspetor posteriormente

    //exemplo:
    //public GameObject PopUpPrefab;

    //na area de dano coloque as seguintes linhas de codigo:
    //GameObject PopUpDamage = Instantiate(PopUpPrefab, transform.position, PopUpPrefab.transform.rotation);
    //PopUpDamage.GetComponent<DamagePopUpScritp>().SetDamageValue(damage);

    //notas: 
    //o popup é gerado no centro do objeto que o criar, portando, se nascer muito baixo é possivel colocar + new vector3(x,y,z) na posicao dele e aumentar a posicao y dele
    //exemplo:
    //Instantiate(PopUpPrefab, transform.position + new Vector3(0, 0.75f, 0), PopUpPrefab.transform.rotation);]
    //PopUpDamage.GetComponent<DamagePopUpScritp>().SetDamageValue(damage);

    //Ou entao, criar uma ancora para sua posicao
    //public Transform ancora;

    //GameObject PopUpDamage = Instantiate(PopUpPrefab, ancora.transform.position, PopUpPrefab.transform.rotation);
    //PopUpDamage.GetComponent<DamagePopUpScritp>().SetDamageValue(damage);




}
