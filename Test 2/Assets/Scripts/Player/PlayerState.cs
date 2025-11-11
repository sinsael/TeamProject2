public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet playerInput;
    protected GroundDetected ground;
    protected WallDetected wall;
    protected Climbing climbing;

    protected PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        playerInput = player.inputSystem.input;
        ground = player.ground;
        wall = player.wall;
        climbing = player.climbing;
    }

}
