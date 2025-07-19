using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_IdleState : PlayerState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("idle");
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x != 0)
            stateMachin.ChangeState(player.xMoveState);
        else if (player.moveInput.y != 0)
            stateMachin.ChangeState(player.yMoveState);
    }
}
