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
        if (player.Wall.wallDetected && stateMachine.currentState != player.wallClimbingState)
        {
            stateMachine.ChangeState(player.wallClimbingState);
            return;
        }

        if (player.ground.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
