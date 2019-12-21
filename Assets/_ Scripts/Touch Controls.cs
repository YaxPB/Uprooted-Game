using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour
{
    PlayerController controller;

    public Button jumpB;
    public Button dashB;
    public Button interB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touch_Pos = Camera.main.ScreenToWorldPoint(touch.position);

            //if(touch_Pos = jumpB)
            //{
            //    //activate controller.jump to true or some shit
            //}

            //if (touch_Pos = dashB)
            //{
            //    //activate controller.dash to true or some shit
            //}

            //if (touch_Pos = interB)
            //{
            //    //activate controller.interact or change input into Interact script
            //}
        }
    }
}
