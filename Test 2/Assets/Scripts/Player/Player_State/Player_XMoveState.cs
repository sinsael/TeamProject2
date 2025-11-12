
public class Player_XMoveState : Player_GroundedState
{
    public Player_XMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }



    public override void Update()
    {
        base.Update();

        if (player.inputSystem.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

}
