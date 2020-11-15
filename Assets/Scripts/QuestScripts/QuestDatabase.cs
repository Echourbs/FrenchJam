using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDatabase : MonoBehaviour
{
    private DialogueManager dm;

    private string questText;
    public Text questTxt;

    private bool isOn;

    void Start()
    {
        dm = FindObjectOfType<DialogueManager>();
        questTxt.enabled = true;
    }

    void Update()
    {
        questTxt.text = questText;

        //Se o jogador respondeu a pergunta da quest principal, aparece o objetivo na tela
        if (dm.questAppearence)
        {
            StartCoroutine(ShowAndHideAnimation());
        }

        if (dm.questId == 1)
        {
            questText = "Seu inimigo agora é a Rainha da Floresta, enfrente-a e conquiste o prêmio com a Rainha do Mal";
        }

        if (dm.questId == 2)
        {
            questText = "Seu inimigo agora é a Rainha do Mal, enfrente-a e conquiste o prêmio com a Rainha da Floresta";
        }
    }

    IEnumerator ShowAndHideAnimation()
    {
        //Pequena animação para sumir e aparecer o objetivo da quest principal 
        dm.questAppearence = false;
        for (int i = 0; i < 5; i++)
        {
            questTxt.enabled = false;
            yield return new WaitForSeconds(0.8f);
            questTxt.enabled = true;
            yield return new WaitForSeconds(1.6f);
        }
    }
}
