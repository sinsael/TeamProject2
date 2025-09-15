using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(player.groundDetected)
            player.SetVelocity(player.rb.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if(player.rb.velocity.y < 0)
            stateMachine.ChangeState(player.fallState);

    }
}
