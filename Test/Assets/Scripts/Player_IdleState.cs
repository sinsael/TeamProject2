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
        

        if (player.inputSystem.moveInput.x != 0 && !player.Wall.wallDetected)
            stateMachine.ChangeState(player.xMoveState);
    }
}
