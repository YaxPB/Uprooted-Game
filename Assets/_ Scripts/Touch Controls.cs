using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPhaseDisplay : MonoBehaviour
{
    PlayerController controller;

    public Text phaseDisplayText;
    private Touch theTouch;
    private float timeTouchEnded;
    private float displayTime = .5f;

    public Button jumpB;
    public Button dashB;
    public Button interB;
    

    void Update()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            phaseDisplayText.text = theTouch.phase.ToString();

            if (theTouch.phase == TouchPhase.Ended)
            {
                timeTouchEnded = Time.time;
            }
        }

        else if (Time.time - timeTouchEnded > displayTime)
        {
            phaseDisplayText.text = "";
        }

        //// Check if there is a touch
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    // Check if finger is over a UI element 
        //    if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //    {
        //        Debug.Log("UI is touched");
        //        //so when the user touched the UI(buttons) call your UI methods 
        //    }
        //    else
        //    {
        //        Debug.Log("UI is not touched");
        //        //so here call the methods you call when your other in-game objects are touched 
        //    }
        //}
    }
}
