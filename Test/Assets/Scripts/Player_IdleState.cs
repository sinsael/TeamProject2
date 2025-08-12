using UnityEngine;

public class Player_IdleState : PlayerState
{
    float LastYinput;

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



        if (player.inputSystem.moveInput.x != 0)
        {
            LastYinput = 0;
        }
        else if (player.inputSystem.moveInput.y != 0)
        {
            LastYinput = player.inputSystem.moveInput.y;
        }

        anim.SetFloat("YVelocity", LastYinput);

        if (player.inputSystem.moveInput.x != 0 && !player.Wall.wallDetected)
            stateMachine.ChangeState(player.xMoveState);
        else if (player.inputSystem.moveInput.y != 0 && !player.Wall.wallDetected)
            stateMachine.ChangeState(player.yMoveState);
    }
}
