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
    public float wallClimbSpeed;           // 윗키 (W, UpArrow) 누를 때 오르는 속도

    [Header("Wall Jump")]
    public Vector2 wallJumpPower; // 벽점프 시 힘 (X: 수평, Y: 수직)
    public float wallJumpDuration = 1f;                 // 벽점프 유지 시간 (벽에서 떨어진 후 점프 영향 지속 시간)
    public float controlLock = 0.15f;                   // 벽점프 직후 입력 잠금 시간 (초)
    public float wallCoyoteTime = 0.2f;                 // 벽에서 떨어진 뒤에도 점프 가능한 유예 시간 (초)
}

public class Climbing : MonoBehaviour
{
    Rigidbody2D rb;
    WallDetected wall;
    GroundDetected ground;
    ClimingSetting climbing;
    PlayerInputHandler Input;
    Player player;
    

    public float wallHangTimer { get; private set; }
    public float defaultGravity { get; private set; }
    public float lockTimer { get; private set; }
    public float wallCoyoteTimer { get; private set; }
    public float wallJumpingDirection { get; private set; }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        ground = player.ground;
        climbing = player.climbing;
        wall = player.wall;
        Input = player.inputSystem;

        if (rb != null)
        {
            defaultGravity = rb.gravityScale;
        }

        wallHangTimer = climbing.wallHangDelay;
        wallCoyoteTimer = 0f;
        defaultGravity = rb.gravityScale;
    }

    // 상태에 진입할 때 타이머 등을 초기화하는 함수
    public void EnterState()
    {
        // ★ 지면이면 진입 금지
        if (ground.IsgroundDetected) return;

        // ★ 진입 순간에 다시 백업(버프/환경으로 중력 바뀌었을 수 있음)
        defaultGravity = rb.gravityScale;
        ResetTimers();
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
        wallHangTimer = climbing.wallHangDelay;
        wallCoyoteTimer = climbing.wallCoyoteTime;
        lockTimer = 0f;
    }


    public void UpdateClimbingState()
    {
        // 매 프레임 시간 감소 처리
        if (wallCoyoteTimer > 0f) wallCoyoteTimer -= Time.deltaTime;
        if (lockTimer > 0f) lockTimer -= Time.deltaTime;

        // + 지면이면 벽타기 로직 차단 !!
        if (ground.IsgroundDetected)
        {
            return;
        }

        // 벽 감지
        if (wall.IswallDetected)
        {
            wallCoyoteTimer = climbing.wallCoyoteTime;
        }

        // 벽 반대 방향 저장
        if (wall.IswallDetected)
        {
            wallJumpingDirection = player._FacingRight ? -1 : 1;
        }


    }

    public virtual void WallDirection()
    {
        if (wall.IswallDetected)
        {
            wallJumpingDirection = player._FacingRight ? -1 : 1;
        }
    }

    public bool CheckWallJump()
    {
        bool hasHorizontalInput = Mathf.Abs(Input.moveInput.x) > 0.1f;

        bool isPushingAwayFromWall = Mathf.Sign(Input.moveInput.x) == wallJumpingDirection;

        if ((wall.IswallDetected ||wallCoyoteTimer > 0f ) && hasHorizontalInput && isPushingAwayFromWall && lockTimer <= 0f)
        {
            WallJump();
            return true;
        }
        return false;
    }

    public bool CheckAndPerformClimb()
    {
        // 지면 금지 추가 
        if (wall.IswallDetected && !ground.IsgroundDetected && Input.Climbinginput())
        {
            ResetTimers();
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0f, climbing.wallClimbSpeed);
            return true;
        }
        return false;
    }

    public void performHang()
    {
        // + 진행 중 지면 닿으면 즉시 종료(중력 복원) !!
        if (ground.IsgroundDetected)
        {
            rb.gravityScale = defaultGravity;
            return;
        }

        wallHangTimer -= Time.deltaTime;

       
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;


    }

    public void performSlide()
    {
        if (ground.IsgroundDetected)
        {
            rb.gravityScale = defaultGravity;
            return;
        }

        rb.gravityScale = defaultGravity;
        float y = rb.velocity.y;

        if (y < climbing.wallSlideFallSpeed) y = climbing.wallSlideFallSpeed;
        if (y < climbing.maxFallSpeed) y = climbing.maxFallSpeed;

        rb.velocity = new Vector2(0f, y);
        // X 속도 억제
        bool pushingIntoWall = Mathf.Abs(Input.moveInput.x) > 0.01f && Mathf.Sign(Input.moveInput.x) == (player._FacingRight ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(Input.moveInput.x) < 0.01f)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        Debug.Log($"행 타이머: {wallHangTimer:F2}, 중력: {rb.gravityScale}, Y속도: {rb.velocity.y:F2}");
    }

    public void WallJump()
    {
        rb.gravityScale = defaultGravity;
        player.SetVelocity(climbing.wallJumpPower.x * wallJumpingDirection, climbing.wallJumpPower.y);

        // 재부착 방지
        player.wallAttachLockTimer = 0.2f;

        // Climbing.controlLock 사용
        lockTimer = climbing.controlLock;
    }
}
