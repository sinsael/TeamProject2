using UnityEngine;

public class Player_XMoveState : PlayerState
{

    public Player_XMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("x움직임");
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0 && !player.IsMove || player.Wall.wallDetected)
        {
            stateMachin.ChangeState(player.idleState);
            return;
        }

        if (!player.IsMove)
        {
            player.MoveBy(Mathf.Sign(player.moveInput.x), 0);
        }

    }


}
