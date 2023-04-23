using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isTouchingClimable;
    protected bool isTouchingLedge;
    protected bool grabInput;
    protected bool jumpInput;
    protected int xInput;
    protected int yInput;
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingClimable = player.CheckIfTouchingClimable();
        isTouchingLedge = player.CheckIfTouchingLedge();

        if (isTouchingClimable & isTouchingWall && !isTouchingLedge)
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
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;
        grabInput = player.InputHandler.GrabInput;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput)
        {
            player.WallJumpState.DetermineWallJumpDirection(isTouchingClimable);
            stateMachine.ChangeState(player.WallJumpState);
        }

        if (isGrounded && !grabInput)
        {
            stateMachine.ChangeState(player.IdleState);
        } 
        else if (!isTouchingClimable || (xInput != player.FacingDirection && !grabInput))
        {
            Debug.Log("isTouchingClimable: " + isTouchingClimable);
            stateMachine.ChangeState(player.InAirState);
        } 
        else if (isTouchingClimable &&isTouchingWall && !isTouchingLedge)
        {
            stateMachine.ChangeState(player.LedgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
