using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float JumpPower;
    [SerializeField] private int numberOfJumps;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckSizeX;
    [SerializeField] private float wallCheckSizeY;
    [SerializeField] private float timeTillNextGrab = .2f;

    [SerializeField] private Vector2 ledgeClimbOffset1;
    [SerializeField] private Vector2 ledgeClimbOffset2;
    [SerializeField] private Vector2 ledgeClimbOffsetLeft1;
    [SerializeField] private Vector2 ledgeClimbOffsetLeft2;
    

    private Rigidbody2D rb;
    private Animator anim;

    private enum AnimationState {idle, walk, jump, ledgeClimb};
    private AnimationState state;
    private bool isFacingRight;
    private float dirX;
    private bool canMove;
    private bool canFlip;
    private int jumpLeft;
    private bool canJump;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool canGrabLedge = true;
    private bool canClimbLedge;
    private LedgeDetector ledge;
    private Vector2 ledgeClimbStartPos;
    private Vector2 ledgeClimbEndPos;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ledge = GetComponentInChildren<LedgeDetector>();

        canMove = true;
        canFlip = true;
        isFacingRight = true;
        jumpLeft = numberOfJumps;
    }


    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        CheckSurrounding();
        CheckCanJump();
        CheckCanLedgeClimb();
        UpdateMovement();
        UpdateAnimation();

    }



    private void CheckMovementDirection()
    {
        if (rb.velocity.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && isFacingRight)
        {
            Flip();
            
        }
    }

    private void Flip()
    {
        if (canFlip)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);

        }
    }

    private void CheckInput()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void CheckSurrounding()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.BoxCast(wallCheck.position, new Vector2(wallCheckSizeX, wallCheckSizeY), 0, transform.right, 2f, whatIsGround);
    }

    private void UpdateMovement()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(dirX * walkSpeed, rb.velocity.y);
        }

    }

    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            jumpLeft--;
        }
    }

    private void CheckCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpLeft = numberOfJumps;
        }

        if (jumpLeft <= 0)
        {
            canJump = false;
        } 
        else
        {
            canJump = true;
        }
    }

    private void CheckCanLedgeClimb()
    {

        if (ledge.isLedgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            Vector2 ledgePos = ledge.transform.position;

            if (isFacingRight)
            {
                ledgeClimbStartPos = ledgePos + ledgeClimbOffset1;
                ledgeClimbEndPos = ledgePos + ledgeClimbOffset2;
            }
            else
            {
                ledgeClimbStartPos = ledgePos + ledgeClimbOffsetLeft1;
                ledgeClimbEndPos = ledgePos + ledgeClimbOffsetLeft2;
            }

            canClimbLedge = true;

            canMove = false;
            canFlip = false;
        }
        if (canClimbLedge) 
        {
            transform.position = ledgeClimbStartPos;
        }


    }   

    public void FinishLedgeClimb()
    {
        Debug.Log("Finish climb");
        transform.position = ledgeClimbEndPos;
        canClimbLedge = false;
        Invoke("AllowLedgeGrab", timeTillNextGrab);
        canMove = true;
        canFlip = true;

    }

    private void AllowLedgeGrab() => canGrabLedge = true;

    private void UpdateAnimation()
    {

        if (dirX < 0f)
        {
            state = AnimationState.walk;
        }
        else if (dirX > 0f)
        {
            state = AnimationState.walk;
        }
        else
        {
            state = AnimationState.idle;
        }

        if (rb.velocity.y > 0)
        {
            state = AnimationState.jump;

        }

        if (canClimbLedge)
        {
            state = AnimationState.ledgeClimb;

        }

        anim.SetInteger("state", (int)state);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireCube(wallCheck.position, new Vector3(wallCheckSizeX, wallCheckSizeY, 0));
    }
}
        