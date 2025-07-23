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

        player.ResetXFlip();
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.y == 0 || player.wallDetected)
        {
            stateMachin.ChangeState(player.idleState);
            return;
        }

        if (!player.IsMove)
        {
            player.MoveBy(0, Mathf.Sign(player.moveInput.y));
        }
    }
}
