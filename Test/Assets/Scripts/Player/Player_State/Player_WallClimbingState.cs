using UnityEngine;

// 벽 매달리기 / 오르기(W) / 벽점프(반대 방향) / 슬라이드
public class Player_WallClimbingState : Player_AiredState
{
    // S는 player.wallClimbSettings의 단축 참조(alias)
    private WallClimbSettings S => player.wallClimbSettings;

    private bool isOnWall;
    private float wallHangTimer;
    private float defaultGravity;
    private float lockTimer;
    private float wallCoyoteTimer;

    private float wallJumpingDirection;
    private bool isWallJumping; //오류 보고 온거면 그냥 필요없는데 넣은거임

    public Player_WallClimbingState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        defaultGravity = rb.gravityScale;
        isOnWall = false;
        wallHangTimer = S.wallHangDelay;              // S.wallHangDelay 사용
        lockTimer = 0f;
        wallCoyoteTimer = 0f;
        isWallJumping = false;

        rb.velocity = new Vector2(rb.velocity.x, 0f);
        Debug.Log("벽에 붙음");
    }

    public override void Exit()
    {
        rb.gravityScale = defaultGravity;
        isWallJumping = false;
    }

    public override void Update()
    {
        base.Update();

        // ── 벽 감지 ──
        bool wallDetected = player.Wall.wallDetected;
        if (wallDetected)
        {
            isOnWall = true;
            wallCoyoteTimer = S.wallCoyoteTime;       // S.wallCoyoteTime 사용
        }
        else
        {
            isOnWall = false;
            wallCoyoteTimer -= Time.deltaTime;
        }

        // 벽에서 완전히 떨어졌다면 낙하로 전환
        if (!isOnWall && wallCoyoteTimer <= 0f)
        {
            rb.gravityScale = defaultGravity;
            stateMachine.ChangeState(player.fallState);
            return;
        }

        float h = player.inputSystem.moveInput.x;
        bool climbing = player.inputSystem.Climbing;

        // ── 1) 벽점프 입력 판정 ──
        if (isOnWall)
        {
            wallJumpingDirection = player.facingRight ? -1 : 1; // 벽 반대 방향 저장
        }

        bool pullingOffWall = Mathf.Abs(h) >= 0.4f && Mathf.Sign(h) == wallJumpingDirection;

        if ((isOnWall || wallCoyoteTimer > 0f) && pullingOffWall && lockTimer <= 0f)
        {
            WallJump();
            return;
        }

        // ── 2) 윗키로 오르기 (Climb) ──
        if (isOnWall && climbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0f, S.wallClimbSpeed);    // S.wallClimbSpeed 사용
            wallHangTimer = S.wallHangDelay;                    // S.wallHangDelay 사용
            return;
        }

        // ── 3) 매달리기 (Hang) ──
        rb.gravityScale = 0f;
        if (wallHangTimer > 0f)
        {
            wallHangTimer -= Time.deltaTime;
            rb.velocity = Vector2.zero;
        }
        else
        {
            // ── 4) 슬라이드 (Slide) ──
            rb.gravityScale = defaultGravity;
            float y = rb.velocity.y;

            // S.wallSlideFallSpeed, S.maxFallSpeed 사용
            if (y < S.wallSlideFallSpeed) y = S.wallSlideFallSpeed;
            if (y < S.maxFallSpeed) y = S.maxFallSpeed;

            rb.velocity = new Vector2(0f, y);
        }

        // X 속도 억제
        bool pushingIntoWall = Mathf.Abs(h) > 0.01f && Mathf.Sign(h) == (player.facingRight ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(h) < 0.01f)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        if (lockTimer > 0f) lockTimer -= Time.deltaTime;
    }

    private void WallJump()
    {
        isWallJumping = true;
        rb.gravityScale = defaultGravity;
        rb.velocity = Vector2.zero;

        // S.wallJumpPower.x, S.wallJumpPower.y 사용
        rb.velocity = new Vector2(wallJumpingDirection * S.wallJumpPower.x, S.wallJumpPower.y);

        // 바라보는 방향 반전
        if ((wallJumpingDirection > 0 && !player.facingRight) ||
            (wallJumpingDirection < 0 && player.facingRight))
        {
            player.xFlip();
        }

        // 재부착 방지
        player.wallAttachLockTimer = 0.2f;

        // S.controlLock 사용
        lockTimer = S.controlLock;

        // 상태 전환
        stateMachine.ChangeState(player.fallState);

        // 일정 시간 후 점프 잠금 해제
        player.StartCoroutine(WallJumpEndDelay());
    }

    private System.Collections.IEnumerator WallJumpEndDelay()
    {
        yield return new WaitForSeconds(S.wallJumpDuration);     // S.wallJumpDuration 사용
        isWallJumping = false;
    }
}