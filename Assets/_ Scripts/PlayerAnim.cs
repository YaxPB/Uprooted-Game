using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator anim;
    public PlayerController controller;
    public FixedJoystick joystick;

    public float passive = 0;
    public float passiveTime = 3.5f;
    public float dashTime;
    public float dashTimer = 0.5f;
    private float inputHorizontal;
    private float jCount;

    private bool inputJump;
    private bool dashInput;
    public bool reset;

    void Start()
    {
        anim = GetComponent<Animator>();
        reset = false;
        dashTime = 0;
        jCount = 0;
    }

    void Update()
    {
        //inputHorizontal = Input.GetAxisRaw("P1_Horizontal");
        inputHorizontal = joystick.Direction.x;
        inputJump = Input.GetButtonDown("Jump");                    //fix
        dashInput = Input.GetButtonDown("Dash");                    //^

        dashTime += Time.deltaTime;

        if (controller.canMove)
        {
            if (inputHorizontal > 0 || inputHorizontal < 0)
            {
                passive = 0;
                anim.SetBool("isRunning", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isDashing", false);
                reset = false;
            }
            else
            {
                anim.SetBool("isRunning", false);
                passive += Time.deltaTime;
                if (passive > passiveTime && !reset)
                {
                    anim.SetBool("isIdle", true);
                    passive = 0;
                    Invoke("Reset", 4.5f);
                }
            }

            if (inputJump)
            {
                jCount++;

                if (jCount <= 1)
                {
                    passive = 0;
                    anim.SetBool("isJumping", true);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isDashing", false);
                    reset = false;
                }

                if (jCount > 1 && controller.canDoubleJump)
                {
                    passive = 0;
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isDJumping", true);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isDashing", false);
                    reset = false;
                }
            }

            if (dashInput && controller.dashReady)
            {
                if (dashTime > dashTimer)
                {
                    passive = 0;
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isIdle", false);
                    anim.SetBool("isDashing", true);
                    reset = false;
                    Invoke("DashSet", 0.25f);
                }
            }
        }
    }

    private void Reset()
    {
        anim.SetBool("isIdle", false);
        reset = true;
    }

    private void DashSet()
    {
        dashTime = 0;
        anim.SetBool("isDashing", false);
    }

    public void OnLanding()
    {
        anim.SetBool("isJumping", false);
        anim.SetBool("isDJumping", false);
        dashTime = 0;
        jCount = 0;
    }
}