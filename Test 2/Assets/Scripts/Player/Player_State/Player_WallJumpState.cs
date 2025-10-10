using UnityEngine;

public class Player_WallJumpState : PlayerState
{

	public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		player.XHandleFlip(player.inputSystem.moveInput.x);
	}

	public override void Update()
	{
		base.Update();

		// 추락상태로 변경
		if (player.rb.velocity.y < 0)
			stateMachine.ChangeState(player.fallState);

		if (wall.IswallDetected)
			stateMachine.ChangeState(player.wallHangState);
	}

	public override void Exit()
	{
		base.Exit();
		climbing.ExitState();
	}
}
