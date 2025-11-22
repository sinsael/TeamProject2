using UnityEngine;


public enum PlayerStates
{
    Idle,
    Move,
    Jump,
    WallHang,
    WallSlide,
    WallClimb,
    Crouch
}

public class Player_AnimationController : MonoBehaviour
{
    Player player;
    Animator anim;
    Rigidbody2D rb;

    public PlayerStates currentState { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        anim.SetFloat("Yvelocity", rb.linearVelocity.y);

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
            ChangeState(PlayerStates.Jump);
        }
        else
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                ChangeState(PlayerStates.Jump);
            }
            else if (player.inputSystem.moveInput.x != 0)
            {
                ChangeState(PlayerStates.Move);
            }
            else if (player.inputSystem.CrouchInput())
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
