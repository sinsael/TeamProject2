using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        climbing.performSlide(ground.IsgroundDetected, player._FacingRight, player.inputSystem.moveInput);

        if (ground.IsgroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
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
