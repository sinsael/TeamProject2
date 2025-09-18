
public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

       
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
         if (player.inputSystem.moveInput.x != 0)
            player.SetVelocity(player.inputSystem.moveInput.x * player.Movespeed, rb.velocity.y);
    }
}
