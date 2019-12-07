using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchOn : MonoBehaviour
{
    public MovingPlatform mp;
    public Animator anim;
    public GameObject vCamera;
    public GameObject mainVCamera;
    public float cameraTimeDuration;
    private float cameraTime;
    private bool complete;
    private bool pressed;

    private void Start()
    {
        pressed = false;
    }

    public void setSwitchOn()
    {
        if (!pressed)
        {
            FindObjectOfType<AudioManager>().Play("Switch");
            anim.SetBool("isOn", true);
            ActivateCamera();

            if (!mp.enabled)
            {
                mp.enabled = true;
            }
            Debug.Log("platform on");

            ActivateCamera();
            pressed = true;
        }
    }

    public void ActivateCamera()
    {
        if (!complete)
        {
            vCamera.SetActive(true);
            mainVCamera.SetActive(false);

            Invoke("ResetCamera", cameraTimeDuration);
        }

        //cameraTime -= Time.deltaTime;

        //if (cameraTime <= 0)
        //{
        //    vCamera.SetActive(false);
        //    mainVCamera.SetActive(true);
        //    cameraTime = cameraTimeDuration;
        //}
    }

    public void ResetCamera()
    {
        vCamera.SetActive(false);
        mainVCamera.SetActive(true);

        complete = true;
    }
}
