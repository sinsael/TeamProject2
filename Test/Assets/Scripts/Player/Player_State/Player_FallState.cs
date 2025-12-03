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
        if (player.Wall.wallDetected && stateMachine.currentState != player.Player_WallClimbingState)
        {
            stateMachine.ChangeState(player.Player_WallClimbingState);
            return;
        }

        if (player.ground.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
