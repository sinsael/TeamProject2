using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // ���߿��� ���� ������ ��� ��Ÿ��
        if (player.Wall.wallDetected && stateMachine.currentState != player.Player_WallClimbingState)
        {
            stateMachine.ChangeState(player.Player_WallClimbingState);
            return;
        }

        if (player.ground.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
