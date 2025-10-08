using UnityEngine;

public class Player_IdleState : Player_GroundedState
{

    public Player_IdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, player.rb.velocity.y);
        Debug.Log("Idle State Entered");
    }

    public override void Update()
    {
        base.Update();


        if (player.inputSystem.moveInput.x != 0 && !player.wall.IswallDetected)
            stateMachine.ChangeState(player.xMoveState);

    }


}
