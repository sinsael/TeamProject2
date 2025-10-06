using UnityEngine;

// 벽 매달리기 / 오르기(W) / 벽점프(반대 방향) / 슬라이드
public class Player_WallClimbingState : Player_AiredState
{
    private Climing climing => player.climingscript;

    public Player_WallClimbingState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();

        climing.EnterState();
    }

    public override void Update()
    {
        base.Update();

        climing.UpdateClimbingState();

        // 벽에서 완전히 떨어졌다면 낙하로 전환
        if (climing.isOnWall && climing.wallCoyoteTimer <= 0f)
        {
            stateMachine.ChangeState(player.fallState);
            return;
        }

        if (climing.CheckAndPerformWallJump())
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        if (climing.CheckAndPerformClimb())
        {
            return;
        }


        climing.performHangAndSlide();
    }

    public override void Exit()
    {
        base.Exit();
        climing.ExitState();
    }
}