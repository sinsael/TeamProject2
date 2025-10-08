using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // 공중에서 벽을 만나면 즉시 벽타기
        if (player.wall.IswallDetected && stateMachine.currentState != player.wallHangState)
        {
            stateMachine.ChangeState(player.wallHangState);
            return;
        }

        if (player.ground.IsgroundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
