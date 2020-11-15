using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public CinemachineVirtualCamera cine;

    public GameObject yesBt, noBt, skipBt, closeBt;

    public Animator animator;
    private AudioSource aud;

    private Queue<string> sentences;

    private DialogueTrigger dt;
    private GameObject player;
    private MovementScript playerScript;

    private bool finalSentence;
    private bool question;
    public bool quest;
    public bool questAppearence;
    public int questId;

    // Use this for initialization
    void Start()
    {
        cine = Camera.main.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
        finalSentence = false;
        sentences = new Queue<string>();

        //aud = GetComponent<AudioSource>();
        player = MovementScript.player;

        playerScript = player.GetComponent<MovementScript>();
        dt = FindObjectOfType<DialogueTrigger>();
        quest = false;
        questAppearence = false;
        question = false;
    }

    void Update()
    {
        //Define qual o NPC está falando, para a resposta seja adequada
        if (nameText.text.ToString() == "Rei da Floresta")
        {
            dt.npcId = 1;
        }
        else{
            dt.npcId = 0;
        }

        if (Input.GetButtonDown("Jump") && !question && dt.onDialogue)
        {
            DisplayNextSentence();
        }
        else if (Input.GetButtonDown("Jump") && question && dt.onDialogue)
        {
            EndDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //playerScript.isPlaying = false;

        if (!finalSentence)
        {
            animator.SetBool("isOpen", true);
        }

        //aud.enabled = true;
        //aud.mute = false;
        nameText.text = dialogue.name;

        //Limpa as sentenças anteriores para começar a próxima
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        if (!dt.quest)
        {
            Invoke("DisplayNextSentence", 0.3f);
        }
        else
        {
            Invoke("CloseSentence", 0.3f);
        }
    }

    //Funções para botões
    public void YesSir()
    {
        if(dt.npcId == 0)
        {
            questId = 1;
        }
        if (dt.npcId == 1)
        {
            questId = 2;
        }
        quest = true;
        questAppearence = true;
        dt.YesAnswer(dt.npcId);
    }
    public void NoSir()
    {
        if (dt.npcId == 0)
        {
            questId = 2;           
        }
        if (dt.npcId == 1)
        {
            questId = 1;
        }
        quest = true;
        questAppearence = true;
        dt.NoAnswer(dt.npcId);
    }
    public void Trigger()
    {
        dt.TriggerDialogue();
    }

    //Próxima Sentença
    public void DisplayNextSentence()
    {
        if (sentences.Count == 2)
        {
            question = true;
            skipBt.SetActive(false);
            closeBt.SetActive(true);
            //noBt.SetActive(true);
            //yesBt.SetActive(true);
        }

        if(sentences.Count == 1)
        {
            finalSentence = true;
            closeBt.SetActive(true);
            noBt.SetActive(false);
            yesBt.SetActive(false);
        }
       
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //Proxima sentença quando ja foi respondido
    public void CloseSentence()
    {
        if(sentences.Count == 4)
        {
            closeBt.SetActive(true);
            noBt.SetActive(false);
            yesBt.SetActive(false);
        }
        if (sentences.Count == 3)
        {
            EndDialogue();
            return;
        }
       
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //Animação para cada letra da sentença
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            //aud.Play();
            yield return new WaitForSeconds(0.03f);
        }
    }

    //Fim do Diálogo
    public void EndDialogue()
    {
        question = false;
        playerScript.isPlaying = true;
        //aud.mute = true;
        dt.onDialogue = false;
        animator.SetBool("isOpen", false);

        cine.LookAt = MovementScript.player.transform;
        cine.Follow = MovementScript.player.transform;

        noBt.SetActive(false);
        yesBt.SetActive(false);
        closeBt.SetActive(false);
        skipBt.SetActive(true);
        MoldingFenix.molding.GetComponent<MoldingFenix>().ChangeFrame();
    }

}