// 공중에 있을때의 상태들 관리 스크립트
public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Update()
    {
        base.Update();

        // 공중에서 벽에 닿는 순간 즉시 벽타기 상태로 전환
        if (wall.IswallDetected)
        {
            stateMachine.ChangeState(player.wallHangState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // 공중에서의 움직임 설정
         if (player.inputSystem.moveInput.x != 0)
            player.SetVelocity(player.inputSystem.moveInput.x * player.MoveSpeed, rb.velocity.y);
    
        if(player.wall.IswallDetected && stateMachine.currentState != player.wallHangState)
        {
            stateMachine.ChangeState(player.wallHangState);
            return;
        }
    }
}
