using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class RockBreak : MonoBehaviour
{
    public PlayerController controller;
    public Collider2D box;
    public Animator rockAnim;
    public CameraShake shakie;

    public bool hit;

    //public CinemachineVirtualCamera VirtualCamera;
    //private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    //public AnimationCurve curvy;

    //public float despawnTime = 0.1f;

    //public float ShakeAmplitude = 5f;         // Cinemachine Noise Profile Parameter
    //public float ShakeFrequency = 0.5f;         // Cinemachine Noise Profile Parameter

    //public float ShakeDuration = 0.5f;          // Time the Camera Shake effect will last
    //public float ShakeElapsedTime = 0f;
    //private bool shake;

    void Start()
    {
        hit = false;
        //shake = false;
        // Get Virtual Camera Noise Profile
        //if (VirtualCamera != null)
        //    virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionEnter2D(collision);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player" && controller.isDashing == true)
        {
            box.enabled = false;
            rockAnim.SetBool("isBroken", true);
            FindObjectOfType<AudioManager>().Play("RockBreak");

            Debug.Log("rock hit");
            hit = true;

            shakie.rockie = this;

            //StartCoroutine(cameraShake.Shake(1f, 1f));

            //if (VirtualCamera != null && virtualCameraNoise != null)
            //{
            //    // Set Cinemachine Camera Noise parameters
            //    virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
            //    virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
            //    shake = true;
            //}
        }   
    }

    private void Update()
    {
        //if (shake)
        //{
        //   // Update Shake Timer
        //    ShakeElapsedTime += Time.deltaTime;
        //    if (ShakeElapsedTime > ShakeDuration)
        //    {
        //        // If Camera Shake effect is over, reset variables

        //        virtualCameraNoise.m_FrequencyGain = 0f;    
        //        virtualCameraNoise.m_AmplitudeGain = 0f;
        //        ShakeElapsedTime = 0f;
        //    }
        //}
    }

}
