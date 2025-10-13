

// 땅에 있을때에 상태들 관리 스크립트
public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.inputSystem.JumpInput())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        
    }


}
