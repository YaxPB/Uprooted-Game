using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class SacrificeAbility : MonoBehaviour
{
    public PlayerController controller;
    public GameObject barrier;
    public GameObject hudImage;
    public GameObject abilityImage;
    public AudioManager _AudioManager;
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    
    private bool complete;
    public bool sacrificed;
    public float freezeTime = 5f;

    public float ShakeAmplitude = 5f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 0.5f;         // Cinemachine Noise Profile Parameter

    public float ShakeDuration = 0f;          // Time the Camera Shake effect will last
    public float ShakeElapsedTime = 0f;
    private bool shake;


    private void Start()
    {
        sacrificed = false;
        shake = false;
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }
    public void sacrificeDoubleJump()
    {
        controller.canDoubleJump = false;
        if (!complete)
            Sacrifice();
    }

    public void sacrificeMossClimb()
    {
        controller.canMossClimb = false;
        if (!complete)
            Sacrifice();
    }

    public void sacrificeRockDash()
    {
        controller.canRockDash = false;
        if (!complete)
            Sacrifice();
    }

    public void sacrificeSetCheckPoint()
    {
        controller.canSetCheckPoint = false;
    }

    private void Sacrifice()
    {
        controller.FreezePlayer(true);
        StartCoroutine(controller.DelayedFreeze(false, freezeTime));

        if (barrier != null)
        {

            _AudioManager.Play("Sacrifice");
            _AudioManager.Play("Rockbreak");
            sacrificed = true;
            if (VirtualCamera != null && virtualCameraNoise != null)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
                shake = true;
            }
            //Destroy(barrier, 3f);
        }

        hudImage.SetActive(false);
        abilityImage.SetActive(true);

        complete = true;
    }

    private void Update()
    {
        if (shake)
        {
            // Update Shake Timer
            ShakeElapsedTime += Time.deltaTime;
            if (ShakeElapsedTime > ShakeDuration)
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                virtualCameraNoise.m_FrequencyGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }
    }
}
