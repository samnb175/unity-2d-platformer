using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool isGrounded;
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool coyoteTime;
    private bool isJumping;
    private bool isTouchingWall;
    private bool isTouchingClimable;
    private bool isTouchingClimableBack;
    private bool isTouchingLedge;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingClimable = player.CheckIfTouchingClimable();
        isTouchingClimableBack = player.CheckIfTouchingClimableBack();
        isTouchingLedge = player.CheckIfTouchingLedge();

        Debug.Log("isTouchingWall" + isTouchingWall);
        Debug.Log("isTouchingClimable" + isTouchingClimable);
        Debug.Log("isTouchingLedge" + isTouchingLedge);

        if ((isTouchingWall || isTouchingClimable) && !isTouchingLedge)
        {
            player.LedgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        CheckCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;

        CheckJumpMultipler();

        if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if ((isTouchingWall || isTouchingClimable) && !isTouchingLedge && !isGrounded) 
        {
            Debug.Log("ledgeCllimState: " + (isTouchingWall || isTouchingClimable));
            stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingClimable || isTouchingClimableBack))
        {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingClimable);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingClimable && grabInput)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        else if (isTouchingClimable && xInput == player.FacingDirection && player.CurrentVelocity.y < 0f)
        {
            Debug.Log("isTouchingClimable: " + isTouchingClimable);
            stateMachine.ChangeState(player.WallSlideState);
        }

        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);
            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));
        }
    }

    private void CheckJumpMultipler()
    {
        if (isJumping)
        {
            //Debug.Log("isJumping" + isJumping);
            if (jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                //Debug.Log("hello");
                isJumping = false;
            } else if (player.CurrentVelocity.y < 0f)
            {
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;
    public void SetIsJumping() => isJumping = true;
}
