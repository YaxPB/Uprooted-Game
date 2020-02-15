using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject currentInterObj = null;

    public Animator anim;
    public DialogueTrigger dt;
    public PlayerController controller;

    private bool touchingCP;
    private bool touchingDJ;
    private bool touchingMC;
    private bool touchingRD;
    public bool touchingSwitch;
    public bool touchingDialogue;
    public bool stop;

    public bool shouldSwitch;
    public bool shouldDisplay;

    public static Interaction Inter;

    void Start()
    {
        Inter = this;
        shouldDisplay = true;
        shouldSwitch = true;
    }

    void Update()
    {
        if (Input.GetButton("Interact") && currentInterObj)
        {
            InvokeInteraction();
        }
    }

    //Indicates which trigger collider is being used
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Checkpoint"))
        {
            currentInterObj = col.gameObject;
            Debug.Log(col.name);

            touchingCP = true;
        }

        if (col.CompareTag("SacrificeDJ"))
        {
            currentInterObj = col.gameObject;
            Debug.Log(col.name);

            touchingDJ = true;
        }

        if (col.CompareTag("SacrificeMC"))
        {
            currentInterObj = col.gameObject;
            Debug.Log(col.name);

            touchingMC = true;
        }

        if (col.CompareTag("SacrificeRD"))
        {
            currentInterObj = col.gameObject;
            Debug.Log(col.name);

            touchingRD = true;
        }

        if (col.CompareTag("Switch"))
        {
            currentInterObj = col.gameObject;
            Debug.Log(col.name);

            touchingSwitch = true;
        }

        if (col.CompareTag("Dialogue"))
        {
            dt = col.GetComponent<DialogueTrigger>();
            currentInterObj = col.gameObject;
            Debug.Log(col.name);

            touchingDialogue = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentInterObj)
        {
            currentInterObj = null;
        }

        touchingCP = false;
        touchingDJ = false;
        touchingMC = false;
        touchingRD = false;
        touchingSwitch = false;
        touchingDialogue = false;

        shouldDisplay = true;
        stop = false;
    }

    public void InvokeInteraction()
    {
        if (touchingCP)
        {
            //Invokes function in player controller
            currentInterObj.SendMessage("setCheckpoint");
            Debug.Log("Set Checkpoint");
        }

        if (touchingDJ)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isDashing", false);

            currentInterObj.SendMessage("sacrificeDoubleJump");
            Debug.Log("Sacrificed Double Jump");
        }

        if(touchingMC)
        {
            currentInterObj.SendMessage("sacrificeMossClimb");
            Debug.Log("Sacrificed Moss Climb");
        }

        if(touchingRD)
        {
            currentInterObj.SendMessage("sacrificeRockDash");
            Debug.Log("Sacrificed Rock Dash");
        }

        if(touchingSwitch)
        {
            shouldSwitch = false;
            currentInterObj.SendMessage("setSwitchOn");
        }

        if(touchingDialogue)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isDashing", false);

            shouldDisplay = false;

            if (!stop)
            {
                dt.TriggerDialogue();
                Debug.Log(stop);
            }
            Debug.Log(stop);
            //stop = true; 
        }
    }
}
