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
        if (player.wall.IswallDetected && stateMachine.currentState != player.wallHangState)
        {
            stateMachine.ChangeState(player.wallHangState);
            return;
        }

        if (player.ground.IsgroundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
