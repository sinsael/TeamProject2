using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallClimbedState : PlayerState
{
    public Player_WallClimbedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();

        if (!climbing.CheckAndPerformClimb(ground.IsgroundDetected, wall.IswallDetected, player.inputSystem.Climbinginput()))
        {
            stateMachine.ChangeState(player.wallHangState);
            return;
        }

        if (climbing.CheckWallJump(wall.IswallDetected, player.inputSystem.moveInput))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        climbing.ExitState();
    }
}
