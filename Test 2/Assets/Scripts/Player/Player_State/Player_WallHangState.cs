using UnityEngine;

// 벽 매달리기 / 오르기(W) / 벽점프(반대 방향) / 슬라이드
public class Player_WallHangState : PlayerState
{
    public Player_WallHangState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();

        // + 지면이면 진입 금지: 곧바로 Grounded로 ---!!!
        if (ground.IsgroundDetected)
        {
            stateMachine.ChangeState(
                Mathf.Abs(player.inputSystem.moveInput.x) > 0.01f ? player.xMoveState : player.idleState
            );
            return;
        }

        climbing.EnterState(ground.IsgroundDetected);
    }

    public override void Update()
    {
        base.Update();

        climbing.UpdateClimbingState(ground.IsgroundDetected, wall.IswallDetected, player._FacingRight);

        // + 지면이면 상태 전환 --- !!!
        if (ground.IsgroundDetected)
        {
            stateMachine.ChangeState(
                Mathf.Abs(player.inputSystem.moveInput.x) > 0.01f ? player.xMoveState : player.idleState
            );
            return;
        }

        if (climbing.CheckWallJump(wall.IswallDetected, player.inputSystem.moveInput, player))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (climbing.CheckAndPerformClimb(ground.IsgroundDetected, wall.IswallDetected, player.inputSystem.Climbinginput()))
        {
            stateMachine.ChangeState(player.wallClimbedState);
            return;
        }

        if (climbing.wallHangTimer > 0f)
        {
            climbing.performHang(ground.IsgroundDetected);
        }
        else if (climbing.wallHangTimer <= 0f)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        climbing.ExitState();
    }
}