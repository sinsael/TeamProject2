using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        //점프
        if(ground.IsgroundDetected)
            player.SetVelocity(player.rb.velocity.x, player.JumpForce);
    }

    public override void Update()
    {
        base.Update();

        // 추락상태로 변경
        if(player.rb.velocity.y < 0)
            stateMachine.ChangeState(player.fallState);

    }
}
