using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_YMoveState : PlayerState
{
    public Player_YMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("y움직임");

    }

    public override void Update()
    {
        Vector2 inputDir = player.moveInput.normalized;

        base.Update();

        if (player.moveInput.y == 0)
            stateMachin.ChangeState(player.idleState);

        player.SetVelocity(rb.velocity.x, inputDir.y * player.moveSpeed);
    }
}
