﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Space]
    [Header("Scripts")]
    public PlayerController controller;

    [Space]
    [Header("Stats")]
    public float moveSpeed;

    public FixedJoystick joystick;

    public AudioManager _AudioManager;
    public bool canPlay = true;
    public float jCount = 0;

    void setSound()
    {
        canPlay = true;
    }

    void Update()
    {
        if (controller.canMove)
        {
            //controller.inputHorizontal = Input.GetAxisRaw("P1_Horizontal") * moveSpeed;

            controller.inputHorizontal = joystick.Direction.x * moveSpeed;

            Debug.Log(joystick.Direction);

            //if(Application.platform == RuntimePlatform.IPhonePlayer)
            //{
            //   controller.inputHorizontal = joystick.
                 
            //}

            //if(joystick.Horizontal >= .3f)
            //{
            //    controller.inputHorizontal = moveSpeed;
            //}
            //else if(joystick.Horizontal <= .3f)
            //{
            //    controller.inputHorizontal = -moveSpeed;
            //}
            //else
            //{
            //    controller.inputHorizontal = 0f;
            //}

            if (controller.inputHorizontal != 0 && canPlay && controller.Grounded)
            {
                canPlay = false;
                _AudioManager.sounds[0].pitch = Random.Range(0.7f, 1f);
                _AudioManager.sounds[0].volume = Random.Range(0.5f, 0.7f);
                Invoke("setSound", 0.75f);
                _AudioManager.Play("Footstep");
            }

            if (Input.GetButtonDown("Dash") && controller.dashReady)
            {
                InvokeDash();
            }

            if (Input.GetButtonDown("Jump") && controller.extraJumps > 0 && jCount < 1)
            {
                InvokeJump();
            }
            else if (Input.GetButtonDown("Jump") && controller.extraJumps > 0 && jCount >= 1 && controller.canDoubleJump)
            {
                InvokeDJump();
            }
        }
    }

    public void OnLanding()
    {
        jCount = 0;
    }

    public void InvokeDash()
    {
        Debug.Log("dashed");
        canPlay = false;
        _AudioManager.sounds[0].pitch = Random.Range(0.7f, 1f);
        _AudioManager.sounds[0].volume = Random.Range(0.5f, 0.7f);
        _AudioManager.Play("Dash");
        Invoke("setSound", 1f);
    }

    public void InvokeJump()
    {
        Debug.Log("hello");
        controller.inputJump = true;
        _AudioManager.sounds [2].pitch = Random.Range(1.3f, 2.5f);
        _AudioManager.sounds[2].volume = Random.Range(0.5f, 0.7f);
        _AudioManager.Play("Jump");
        jCount++;
    }

    public void InvokeDJump()
    {
        controller.inputJump = true;
        _AudioManager.Play("DJump");
        jCount = 0;
    }
}
