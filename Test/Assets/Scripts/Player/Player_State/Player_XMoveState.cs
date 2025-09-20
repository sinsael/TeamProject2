using UnityEngine;

public class Player_XMoveState : Player_GroundedState
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
        base.Update();

        if (player.inputSystem.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }


        if (player.Wall.wallDetected && player.inputSystem.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        


    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.SetVelocity(player.inputSystem.moveInput.x * player.MoveSpeed, player.rb.velocity.y);
    }

}
