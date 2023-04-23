using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrawlMoveState : PlayerGroundedState
{
    private bool crawlInput;

    public PlayerCrawlMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetColliderHeight(playerData.crawlColliderHeight);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetColliderHeight(playerData.crouchColliderHeight);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            crawlInput = player.InputHandler.GrabInput;

            player.SetVelocityX(playerData.crouchMovementVelocity * player.FacingDirection);
            player.CheckIfShouldFlip(xInput);

            if (xInput == 0)
            {
 
                stateMachine.ChangeState(player.CrouchIdleState);
                
            }
            else if (yInput != -1 || !crawlInput)
            {
                stateMachine.ChangeState(player.CrouchMoveState);
            }

        } 
    }
}
