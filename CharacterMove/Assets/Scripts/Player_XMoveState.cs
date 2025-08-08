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

        if (player.inputSystem.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        
        if (!player.IsMove)
        {
           if (player.Wall.wallDetected)
            {
                stateMachine.ChangeState(player.idleState);
                return;
            }

            player.MoveBy(Mathf.Sign(player.inputSystem.moveInput.x), 0);
        }

    }

}
