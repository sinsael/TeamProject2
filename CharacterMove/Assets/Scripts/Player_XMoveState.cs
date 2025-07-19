using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_XMoveState : PlayerState
{
    public Player_XMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("x움직임");
    }

    public override void Update()
    {
        Vector2 inputDir = player.moveInput.normalized;
        
        base.Update();


        if (player.moveInput.x == 0)
            stateMachin.ChangeState(player.idleState);

        player.SetVelocity(inputDir.x * player.moveSpeed, rb.velocity.y);
    }
}
