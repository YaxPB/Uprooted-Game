using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("Stats")]

    [SerializeField] private float jumpForce = 600f;
    public int doubleJumpVelocity = 19;
    [Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f; // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;  // How much to smooth out the movement
    const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    const float CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private float inputVertical;
    public float inputHorizontal;
    public int extraJumps;
    public int extraJumpsValue;
    private float freezePeriod;
    public float freezePeriodDuration;

    [Space]
    [Header("Dash Stats")]

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private float dashCoolDown;
    public float dashCoolDownValue;
    public float activeDashPeriod;
    private int direction;
    private int extraDash;
    public int extraDashValue;
    public bool dashReady;
    private bool canDash;

    [Space]
    [Header("Moss Climb")]

    public float climbSpeed;
    public float climbSpeedCap;
    public float checkWallDistance;
    public float defGravityScale = 3.5f;
    private bool isClimbingUp;
    private bool isClimbingDown;
    private bool isTouchingWall;
    public float wallJumpVerForce;
    public float wallJumpHorForce;
    public LayerMask mossWall;
    public LayerMask canClimbWalls;
    public Transform leftStart;
    public Transform leftEnd;
    public Transform rightStart;
    public Transform rightEnd;
    public Transform checkWall;

    [Space]
    [Header("Booleans")]

    [SerializeField] private bool AirControl = true; // Whether or not a player can steer while jumping;
    private bool FacingRight = true; // For determining which way the player is currently facing.
    public bool Grounded; // Whether or not the player is grounded.
    public bool isDashing;
    public bool inputJump = false;
    public bool inputCrouch = false;
    public bool isFrozen;
    public bool canMove = true;

    [Space]
    [Header("Abilities Booleans")]

    public bool canMossClimb;
    public bool canDoubleJump;
    public bool canRockDash;
    public bool canSetCheckPoint;
   
    [Space]
    [Header("References")]

    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private Transform CeilingCheck; // A position marking where to check for ceilings
    [SerializeField] private Collider2D CrouchDisableCollider; // A collider that will be disabled when crouching
    private Rigidbody2D rb;
    private float movementXVelocity;
    public Vector3 spawnPoint;
    public PlayerMovement playerMovement;
    public Animator anim;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }
    public BoolEvent OnCrouchEvent;
    private bool wasCrouching = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    void Start()
    {
        dashTime = startDashTime;
        extraDash = extraDashValue;
        extraJumps = extraJumpsValue;

        //spawnPoint starts at the player position during the first frame
        spawnPoint = gameObject.transform.position;

        canMossClimb = true;
        canDoubleJump = true;
        canRockDash = true;
        canSetCheckPoint = true;
        isFrozen = false;
    }

    private void FixedUpdate()
    {
        inputVertical = Input.GetAxisRaw("P1_Vertical");

        Move(inputHorizontal * Time.fixedDeltaTime, inputCrouch, inputJump);
        
        /*if((inputHorizontal > 0 || inputHorizontal < 0) && Grounded ) 
        {
            FindObjectOfType<AudioManager>().Play("Footstep");
        }*/

        inputJump = false;

        bool wasGrounded = Grounded;
        Grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)  
        {
            if (colliders[i].gameObject != gameObject)
            {
                Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        isTouchingWall = Physics2D.Linecast(rightStart.position, rightEnd.position, mossWall);

        if (isTouchingWall)
        {
            Debug.DrawLine(rightStart.position, rightEnd.position, Color.yellow);
        }
        else
        {
            Debug.DrawLine(rightStart.position, rightEnd.position, Color.white);
        }

        //climbing
        if (isTouchingWall)
        {
            extraJumps = 1;
            if (inputVertical > 0)
            {
                isClimbingUp = true;
            }
            else
            {
                isClimbingUp = false;
            }

            if (inputVertical < 0)
            {
                isClimbingDown = true;
            }
            else
            {
                isClimbingDown = false;
            }
        }

        if (isTouchingWall && canMossClimb)
        {
            if (isClimbingUp)
            {
                if (rb.velocity.y < climbSpeedCap) 
                {
                    rb.AddRelativeForce(Vector2.up * climbSpeed);
                    anim.SetBool("isClimbing", true);
                    anim.SetBool("isHanging", false);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isDJumping", false);
                }
            }
            else if (isClimbingDown)
            {
                if (rb.velocity.y < climbSpeedCap)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddRelativeForce(Vector2.down * climbSpeed);
                    anim.SetBool("isClimbing", true);
                    anim.SetBool("isHanging", false);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isDJumping", false);
                }
            }
            else
            {
                anim.SetBool("isClimbing", false);
                anim.SetBool("isHanging", true);
                anim.SetBool("isJumping", false);
                anim.SetBool("isDJumping", false);
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.gravityScale = 0;
                //rb.AddForce(//some upwards velocity to negate the downwards velocity//,  ForceMode.VelocityChange);
            }
        }
        else
        {
            anim.SetBool("isClimbing", false);
            anim.SetBool("isHanging", false);
            rb.gravityScale = defGravityScale;
        }
    }

    void Update()
    {
        //Reset when grounded
        if (Grounded == true)
        {
            extraJumps = extraJumpsValue;
            extraDash = extraDashValue;
        }

        //Dash cool down
        if (!dashReady)
        {
            dashCoolDown -= Time.deltaTime;

            if (dashCoolDown <= 0)
            {
                dashReady = true;
            }
        }

        //Dash
        if (direction == 0)
        {
            //Left = 1
            if (!FacingRight && Input.GetButtonDown("Dash") && extraDash > 0 && dashReady)
            {
                direction = 1;
                extraDash--;
                dashReady = false;
                dashCoolDown = dashCoolDownValue;
            }
            //Right = 2
            else if (FacingRight && Input.GetButtonDown("Dash") && extraDash > 0 && dashReady)
            {
                direction = 2;
                extraDash--;
                dashReady = false;
                dashCoolDown = dashCoolDownValue;
            }
            //Down = 3
            else if (inputVertical < 0 && Input.GetButtonDown("Dash") && extraDash > 0 && dashReady)
            {
                direction = 3;
                extraDash--;
                dashReady = false;
                dashCoolDown = dashCoolDownValue;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (canRockDash && canDash)
                {
                    if (direction == 1)
                    {
                        rb.velocity = Vector2.left * dashSpeed;
                        isDashing = true;
                        Invoke("notDashing", activeDashPeriod);
                    }
                    else if (direction == 2)
                    {
                        rb.velocity = Vector2.right * dashSpeed;
                        isDashing = true;
                        Invoke("notDashing", activeDashPeriod);
                    }
                    else if (direction == 3)
                    {
                        rb.velocity = Vector2.down * dashSpeed;
                    }
                }
            }
        }

        if (isFrozen)
        {
            canMove = false;
            canDash = false;
        }
        else
        {
            canMove = true;
            canDash = true;
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(CeilingCheck.position, CeilingRadius, WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (Grounded || AirControl)
        {
            // If crouching
            if (crouch)
            {
                if (!wasCrouching)
                {
                    wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= CrouchSpeed;

                // Disable one of the colliders when crouching
                if (CrouchDisableCollider != null)
                    CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (CrouchDisableCollider != null)
                    CrouchDisableCollider.enabled = true;

                if (wasCrouching)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            if (!isFrozen)
            {
                // Move the character by finding the target velocity
                float targetXVelocity = move * 10f;
                // And then smoothing it out and applying it to the character
                float smoothX = Mathf.SmoothDamp(rb.velocity.x, targetXVelocity, ref movementXVelocity, MovementSmoothing);
                rb.velocity = new Vector2(smoothX, rb.velocity.y);
            }

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && FacingRight)
            {
                Flip();
            }
        }
        
        if (jump)
        {
            rb.gravityScale = defGravityScale;
            if (Grounded)
            {
                // Add a vertical force to the player.
                rb.AddForce(new Vector2(0f, jumpForce));
            }
            else if ((isTouchingWall && !FacingRight) || Grounded)
            {
                rb.AddForce(new Vector2(wallJumpVerForce, wallJumpHorForce));
                Flip();
            }
            else if (isTouchingWall || Grounded)
            {
                rb.AddForce(new Vector2(-wallJumpVerForce, wallJumpHorForce));
                Flip();
            }
            else if (extraJumps > 0 && canDoubleJump)
            {
                rb.velocity = new Vector3(0, doubleJumpVelocity, 0);
                extraJumps--;
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Respawn()
    {
        //Respawns player at startingSpawn
        gameObject.transform.position = spawnPoint;
    }

    private void notDashing()
    {
        //Resets isDashing to false
        if (isDashing == true)
        {
            isDashing = false;
        }
    }

    public void FreezePlayer(bool value)
    {
        isFrozen = value;
    }

    public IEnumerator DelayedFreeze(bool value, float duration)
    {
        yield return new WaitForSeconds(duration);
        isFrozen = value;
    }
}
