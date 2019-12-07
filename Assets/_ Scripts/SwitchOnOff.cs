using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOnOff : MonoBehaviour
{
    public MovingPlatform mp;


    public void setSwitch()
    {
        FindObjectOfType<AudioManager>().Play("Switch");

        if (mp.enabled)
        {
            mp.enabled = false;
        }
        else if (!mp.enabled)
        {
            mp.enabled = true;
        }
        Debug.Log("platform on");
    }
}
