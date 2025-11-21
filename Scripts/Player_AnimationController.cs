using UnityEngine;


public enum PlayerStates
{
    Idle,
    Move,
    Jump,
    Fall,
    WallHang,
    WallSlide,
    WallClimb,
    Crouch
}

public class Player_AnimationController : MonoBehaviour
{
    Player player;
    Animator anim;

    public PlayerStates currentState { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (player.wall.IswallDetected && !player.ground.IsgroundDetected)
        {
            if (player.climbing.CheckWallJump(player.wall.IswallDetected, player.inputSystem.moveInput))
            {
                ChangeState(PlayerStates.Jump);
            }
            else if (player.inputSystem.Climbinginput())
            {
                ChangeState(PlayerStates.WallClimb);
            }
            else if (player.climbing.wallHangTimer > 0f)
            {
                ChangeState(PlayerStates.WallHang);
            }
            else if (player.climbing.wallHangTimer <= 0f)
            {
                ChangeState(PlayerStates.WallSlide);
            }
        }
        else if (!player.ground.IsgroundDetected)
        {
        }
        else
        {
            if (player.inputSystem.JumpInput())
            {
                ChangeState(PlayerStates.Jump);
            }
            else if (player.inputSystem.moveInput.x != 0)
            {
                ChangeState(PlayerStates.Move);
            }
            else if (player.inputSystem.CrouchInput())  //¿õÅ©¸®±â
            {
                ChangeState(PlayerStates.Crouch);
            }
            else
            {
                ChangeState(PlayerStates.Idle);
            }
        }
    }

    public void ChangeState(PlayerStates newState)
    {
        if (currentState == newState) return;

        currentState = newState;    

        anim.SetInteger("State", (int)newState);
    }
}
