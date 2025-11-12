// 공중에 있을때의 상태들 관리 스크립트
public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Update()
    {
        base.Update();

        player.XHandleFlip(player.inputSystem.moveInput.x);

        wallHang();
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

    }


    public void wallHang()
    {
        bool isMovingTowardsWall = (rb.velocity.x * player.wall.DetectedWallDirection) > 0;
        bool isPushingTowardsWall = (player.inputSystem.moveInput.x * player.wall.DetectedWallDirection) > 0;
        bool shouldStickToWall = isMovingTowardsWall || (isPushingTowardsWall);


        if (player.wall.IswallDetected && shouldStickToWall)
        {
            stateMachine.ChangeState(player.wallHangState);
            return;
        }
    }
}
