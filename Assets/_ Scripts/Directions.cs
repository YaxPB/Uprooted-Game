using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Directions : MonoBehaviour
{
    public GameObject UIObject;
    public Interaction directions;

    void Start()
    {
        UIObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && directions.shouldDisplay == false && directions.touchingDialogue)
        {
            UIObject.SetActive(false);
        }
        else if (collision.tag == "Player" && directions.shouldSwitch == false && directions.touchingSwitch)
        {
            UIObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && directions.shouldDisplay && directions.touchingDialogue)
        {
            UIObject.SetActive(true);
        }
        else if (collision.tag == "Player" && directions.shouldSwitch && directions.touchingSwitch)
        {
            UIObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UIObject.SetActive(false);
            //directions.shouldDisplay = true;
        }
    }
}
