using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    public PlayerController controller;

    public CanvasOrder _CanvasOrder;

    private Queue<string> sentences;

    public bool amTalking;
    public bool canSkipText;
    public bool canStartDio;
    public bool finished;

    string sentence;

    public event Action DialogueEnded;

    void Start()
    {
        amTalking = false;
        canSkipText = true;
        canStartDio = true;
        sentences = new Queue<string>();
        finished = false;
    }

    void Awake()
    {
        instance = this;    
    }

    void Update()
    {
        if (amTalking && canSkipText && Input.GetButton("Interact"))
        {
            InvokeSentence();
        }
    }

    void CanSkipReset()
    {
        canSkipText = true;
    }

    public void StartDialogue (Dialogue dialogue)
    {
        Debug.Log("I am called");
        if (canStartDio)
        {
            finished = false;
            canStartDio = false;
            amTalking = true;
            animator.SetBool("IsOpen", true);
            nameText.text = dialogue.name;
            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                    sentences.Enqueue(sentence);
                    Debug.Log("hello" + sentences.Count);
            }

            DisplayNextSentence();
        }

        controller.FreezePlayer(true);
    }

    public void DisplayNextSentence ()
    {
        Invoke("CanSkipReset", 0.5f);
        Debug.Log("aha");
        Debug.Log("npthere"+sentences.Count);
        if (sentences.Count == 0)
        {
            print("Im Ending");
            EndDialogue();
        }
        if (sentences != null || sentences.Count != 0)
        {
            sentence = sentences.Dequeue();
            Debug.Log("I Work");
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue ()
    {
        canStartDio = true;
        amTalking = false;
        finished = true;
        _CanvasOrder.MoveUp();
        animator.SetBool("IsOpen", false);

        controller.FreezePlayer(false);
        Debug.Log(finished);
        Interaction.Inter.stop = true;
        Debug.Log("Here" + sentences.Count);
        //Debug.Log()

        DialogueEnded?.Invoke();
    }

    public void InvokeSentence()
    {
        if (!finished)
        {
            canSkipText = false;
            DisplayNextSentence();
        }
    }
}
