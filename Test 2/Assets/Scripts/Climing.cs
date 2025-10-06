using System;
using UnityEngine;

[Serializable]
public class ClimingSetting
{
    [Header("Hang / Slide")]
    public float wallHangDelay = 3f;            // 매달리는 시간 (초)_기획서 대로 3초
    public float wallSlideFallSpeed = -0.3f;    // 벽에서 떨어지는 속도_기획서 대로 0.3 중력값
    public float maxFallSpeed = -8f;            // 최대 낙하 속도

    [Header("Climb")]
    public float wallClimbSpeed = 2f;           // 윗키 (W, UpArrow) 누를 때 오르는 속도

    [Header("Wall Jump")]
    public Vector2 wallJumpPower = new Vector2(2f, 4f); // 벽점프 시 힘 (X: 수평, Y: 수직)
    public float wallJumpDuration = 1f;                 // 벽점프 유지 시간 (벽에서 떨어진 후 점프 영향 지속 시간)
    public float controlLock = 0.15f;                   // 벽점프 직후 입력 잠금 시간 (초)
    public float wallCoyoteTime = 0.2f;                 // 벽에서 떨어진 뒤에도 점프 가능한 유예 시간 (초)
}

public class Climing : MonoBehaviour
{
    Rigidbody2D rb;
    WallDetected wall;
    GroundDetected ground;
    ClimingSetting climing;
    PlayerInputHandler Input;
    Player player;
    

    public bool isOnWall { get; private set; } = false;
    public float wallHangTimer { get; private set; }
    public float defaultGravity { get; private set; }
    public float lockTimer { get; private set; }
    public float wallCoyoteTimer { get; private set; }
    public float wallJumpingDirection { get; private set; }

    bool pullingOffWall;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        ground = player.ground;
        climing = player.climing;
        wall = player.Wall;
        Input = player.inputSystem;

        if (rb != null)
        {
            defaultGravity = rb.gravityScale;
        }

        wallHangTimer = climing.wallHangDelay;
        wallCoyoteTimer = 0f;
        defaultGravity = rb.gravityScale;
    }

    // 상태에 진입할 때 타이머 등을 초기화하는 함수
    public void EnterState()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        Debug.Log("벽타기 상태 진입");
    }

    // 상태를 나갈 때 중력을 원래대로 돌리는 함수
    public void ExitState()
    {
        rb.gravityScale = defaultGravity;
    }

    // 타이머 초기화 로직을 별도 함수로 분리
    public void ResetTimers()
    {
        wallHangTimer = climing.wallHangDelay;
        wallCoyoteTimer = 0f;
        lockTimer = 0f;
    }

    public void UpdateClimbingState()
    {
        // 매 프레임 시간 감소 처리
        if (wallCoyoteTimer > 0f) wallCoyoteTimer -= Time.deltaTime;
        if (lockTimer > 0f) lockTimer -= Time.deltaTime;

        // 벽 감지
        if (wall.wallDetected)
        {
            isOnWall = true;
            wallCoyoteTimer = climing.wallCoyoteTime;
        }
        else
        {
            isOnWall = false;
        }

        // 벽 반대 방향 저장
        if (isOnWall)
        {
            wallJumpingDirection = player.facingRight ? -1 : 1;
        }


    }

    public virtual void WallDirection()
    {
        if (isOnWall)
        {
            wallJumpingDirection = player.facingRight ? -1 : 1;
        }
    }

    public bool CheckAndPerformWallJump()
    {
        pullingOffWall = Mathf.Abs(Input.moveInput.x) >= 0.4f && Mathf.Sign(Input.moveInput.x) == wallJumpingDirection;

        if ((isOnWall || wallCoyoteTimer > 0f) && pullingOffWall && lockTimer <= 0f)
        {
            WallJump();
            return true;
        }
        return false;
    }

    public bool CheckAndPerformClimb()
    {
        if (isOnWall && Input.Climbinginput())
        {
            ResetTimers();  
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0f, climing.wallClimbSpeed); 
            return true;
        }
            return false;
    }

    public void performHangAndSlide()
    {
        wallHangTimer -= Time.deltaTime;

        if (wallHangTimer > 0f)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.gravityScale = defaultGravity;
            float y = rb.velocity.y;

            if (y < climing.wallSlideFallSpeed) y = climing.wallSlideFallSpeed;
            if (y < climing.maxFallSpeed) y = climing.maxFallSpeed;

            rb.velocity = new Vector2(0f, y);
        }

        // X 속도 억제
        bool pushingIntoWall = Mathf.Abs(Input.moveInput.x) > 0.01f && Mathf.Sign(Input.moveInput.x) == (player.facingRight ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(Input.moveInput.x) < 0.01f)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        Debug.Log($"행 타이머: {wallHangTimer:F2}, 중력: {rb.gravityScale}, Y속도: {rb.velocity.y:F2}");
        Debug.Log(Time.timeScale);
    }

    private void WallJump()
    {
        rb.gravityScale = defaultGravity;
        rb.velocity = new Vector2(wallJumpingDirection * climing.wallJumpPower.x, climing.wallJumpPower.y);

        // 바라보는 방향 반전
        if ((wallJumpingDirection > 0 && !player.facingRight) ||
            (wallJumpingDirection < 0 && player.facingRight))
        {
            player.xFlip();
        }

        // 재부착 방지
        player.wallAttachLockTimer = 0.2f;

        // climing.controlLock 사용
        lockTimer = climing.controlLock;

        ResetTimers();
    }

    public void CheckGroundDetected()
    {
        if (ground.groundDetected)
        {
            ResetTimers();
        }
    }
}
