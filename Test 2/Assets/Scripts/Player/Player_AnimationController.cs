using UnityEngine;


public enum PlayerStates
{
    Idle,
    Move,
    Jump,
    Fall,
    WallHang,
    WallSlide,
    WallClimb
}

public class Player_AnimationController : MonoBehaviour
{
    Player player;
    Animator anim;

    public PlayerStates currentState { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
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
            else
            {
                ChangeState(PlayerStates.Idle);
            }
        }
    }

    public void ChangeState(PlayerStates newState)
    {
        // 이미 같은 상태면 아무것도 하지 않음
        if (currentState == newState) return;

        currentState = newState;

        // [핵심]
        // 애니메이터에 'State' 숫자를 딱 한 번 설정합니다.
        anim.SetInteger("State", (int)newState);
    }
}
