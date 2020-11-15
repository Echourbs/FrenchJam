using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool onDialogue;
    private bool onTrigger;
    public bool quest;
    public GameObject canvas;
    public static GameObject dialogueTrigger;

    public int npcId;
    DialogueManager dm;


    void Start()
    {
        dialogueTrigger = this.gameObject;
        dm = FindObjectOfType<DialogueManager>();
        onDialogue = false;

        //Opções de Diálogo
        if (npcId == 0)
        {
            dialogue.name = "Rainha do Mal";
            dialogue.sentences[0] = "Olá, sou a rainha do mal";
            dialogue.sentences[1] = "Seja bem vindo ao meu mundo";
            dialogue.sentences[2] = "Me ajude a matar a rainha da floresta e terás tudo o que quiser";
        }

        if (npcId == 1)
        {
            dialogue.name = "Rei da Floresta";
            dialogue.sentences[0] = "Olá, sou o rei da floresta";
            dialogue.sentences[1] = "Fico feliz com a sua chegada";
            dialogue.sentences[2] = "Me ajude a buscar a paz pelo reino, por favor, bravo guerreiro";
        }

        if(npcId == 2)
        {
            dialogue.name = "Voz Desconhecida";
            dialogue.sentences[0] = "Bravo Guerreiro!";
            dialogue.sentences[1] = "Você encontrou a lendária fênix...";
            dialogue.sentences[2] = "Ajude-a a salvar o mundo dela e ela o ajudará a salvar o seu...";
        }
    }

    public void YesAnswer(int i)
    {
        quest = true;
        //Opções de Diálogo
        if (i == 0)
        {
            dialogue.name = "Rainha do Mal";
            dialogue.sentences[0] = "Muito obrigada, guerreiro!";
        }

        if (i == 1)
        {
            dialogue.name = "Rei da Floresta";
            dialogue.sentences[0] = "Muito obrigada, guerreiro!";
        }

        if(i == 2)
        {
            dialogue.name = "Voz Desconhecida";
            dialogue.sentences[0] = "";
        }
    }

    public void NoAnswer(int i)
    {
        quest = true;
        //Opções de Diálogo
        if (i == 0)
        {
            dialogue.name = "Rainha do Mal";
            dialogue.sentences[0] = "Se você se junta ao inimigo, nao hesitarei em te matar";
        }
        if (i == 1)
        {
            dialogue.name = "Rei da Floresta";
            dialogue.sentences[0] = "Se você se junta ao inimigo, nao hesitarei em te matar";
        }

        if (i == 2)
        {
            dialogue.name = "Voz Desconhecida";
            dialogue.sentences[0] = "";
        }
    }

    void Update()
    {
        /*Se estiver no raio do trigger, aparece o canvas
        if (onTrigger && !dm.quest)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
        */

        //Se o canvas estiver ativo e o usuario aperta o botão, aparece o dialogo
        if(onTrigger && Input.GetKeyDown(KeyCode.V) && !dm.quest)
        {
            onDialogue = true;
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        //Inicia o diálogo
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        onDialogue = true;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            onTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            onTrigger = false;
        }
    }
}
