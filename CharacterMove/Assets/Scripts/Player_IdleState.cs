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



        if (player.moveInput.y != 0)
            LastYinput = player.moveInput.y;

        if (player.moveInput.x != 0)
            LastYinput = 0;

        anim.SetFloat("YVelocity", LastYinput);

        if (player.moveInput.x != 0 && !player.Wall.wallDetected)
            stateMachin.ChangeState(player.xMoveState);
        else if (player.moveInput.y != 0 && !player.Wall.wallDetected)
            stateMachin.ChangeState(player.yMoveState);
    }
}
