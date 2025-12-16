using UnityEngine;


public enum PlayerStates
{
    Idle,
    Move,
    Jump,
    WallHang,
    WallSlide,
    WallClimb,
    Crouch,
    WallCrush,
    PushIdle,
    PushMove
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

        if (player.CurrentState != currentState)
        {
            ChangeState(player.CurrentState);
        }
    }

    // 플레이어 상태 변경 메서드
    public void ChangeState(PlayerStates newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        anim.SetInteger("State", (int)newState);
    }
}
