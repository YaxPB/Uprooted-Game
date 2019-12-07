using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCamera : MonoBehaviour
{
    public GameObject vCamera;
    public GameObject mainVCamera;
    public DialogueTrigger dt;
    public Collider2D dialogueCollider;

    public DialogueManager manager;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            dt.TriggerDialogue();
            ActivateCamera();
            dialogueCollider.enabled = false;
        }
    }

    public void ActivateCamera()
    {
        vCamera.SetActive(true);
        mainVCamera.SetActive(false);

        DialogueManager.instance.DialogueEnded += ResetCamera;
    }

    public void ResetCamera()
    {
        vCamera.SetActive(false);
        mainVCamera.SetActive(true);

        DialogueManager.instance.DialogueEnded -= ResetCamera;
    }
}
