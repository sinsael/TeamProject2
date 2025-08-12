using UnityEngine;

public class Player_YMoveState : PlayerState
{
    public Player_YMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Debug.Log("y움직임");




    }

    public override void Update()
    {
        base.Update();

        // Y축 애니메이션을 위한 파라미터 설정은 그대로 유지합니다.
        if (player.inputSystem.moveInput.y != 0)
            anim.SetFloat("YVelocity", player.inputSystem.moveInput.y);

        // 1순위: Y축 입력이 없어졌는가? (키를 뗐거나 다른 방향키로 전환)
        if (player.inputSystem.moveInput.y == 0)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        // 2순위: 이동 중이 아닐 때만 다음 행동을 결정합니다.
        if (!player.IsMove)
        {
            // 벽에 막혔는가? 그렇다면 더 나아가지 않고 Idle 상태로 돌아갑니다.
            if (player.Wall.wallDetected)
            {
                stateMachine.ChangeState(player.idleState);
                return;
            }

            // 위 조건에 모두 해당하지 않으면, 계속해서 Y축으로 이동합니다.
            player.MoveBy(0, Mathf.Sign(player.inputSystem.moveInput.y));
        }
    }
}
