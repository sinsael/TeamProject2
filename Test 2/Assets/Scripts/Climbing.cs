using UnityEngine;

public class Climbing : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Hang / Slide")]
    public float wallHangDelay = 3f;            // 매달리는 시간 (초)_기획서 대로 3초
    public float wallSlideFallSpeed = -0.3f;    // 벽에서 떨어지는 속도_기획서 대로 0.3 중력값
    public float maxFallSpeed = -8f;            // 최대 낙하 속도

    [Header("Climb")]
    public float wallClimbSpeed;           // 윗키 (W, UpArrow) 누를 때 오르는 속도

    [Header("Wall Jump")]
    public Vector2 wallJumpPower; // 벽점프 시 힘 (X: 수평, Y: 수직)
    public float controlLock = 0.15f;                   // 벽점프 직후 입력 잠금 시간 (초)
    public float wallCoyoteTime = 0.2f;                 // 벽에서 떨어진 뒤에도 점프 가능한 유예 시간 (초)

    public float wallHangTimer { get; private set; }
    public float defaultGravity { get; private set; }
    public float lockTimer { get; private set; }
    public float wallCoyoteTimer { get; private set; }
    public float wallJumpingDirection { get; private set; }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            defaultGravity = rb.gravityScale;
        }

        wallCoyoteTimer = 0f;
        defaultGravity = rb.gravityScale;
    }

    // 상태에 진입할 때 타이머 등을 초기화하는 함수
    public void EnterState(bool ground)
    {
        // ★ 지면이면 진입 금지
        if (ground) return;

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
        wallHangTimer = wallHangDelay;
        wallCoyoteTimer = wallCoyoteTime;
        lockTimer = 0f;
    }


    public void UpdateClimbingState(bool ground, bool wall, bool facing)
    {
        // 매 프레임 시간 감소 처리
        if (wallCoyoteTimer > 0f) wallCoyoteTimer -= Time.deltaTime;
        if (lockTimer > 0f) lockTimer -= Time.deltaTime;

        // + 지면이면 벽타기 로직 차단 !!
        if (ground)
        {
            return;
        }

        // 벽 감지
        if (wall)
        {
            wallCoyoteTimer = wallCoyoteTime;
        }

        // 벽 반대 방향 저장
        if (wall)
        {
            wallJumpingDirection = facing ? -1 : 1;
        }


    }

    public bool CheckWallJump(bool wall, Vector2 input)
    {
        bool hasHorizontalInput = Mathf.Abs(input.x) > 0.1f;

        bool isPushingAwayFromWall = Mathf.Sign(input.x) == wallJumpingDirection;

        if ((wall || wallCoyoteTimer > 0f) && hasHorizontalInput && isPushingAwayFromWall && lockTimer <= 0f)
        {
            rb.gravityScale = defaultGravity;
            lockTimer = controlLock;
            return true;
        }
        return false;
    }

    public void PerformClimb()
    {
            ResetTimers();
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0f, wallClimbSpeed);
    }

    public void performHang(bool ground)
    {
        // + 진행 중 지면 닿으면 즉시 종료(중력 복원) !!
        if (ground)
        {
            rb.gravityScale = defaultGravity;
            return;
        }

        wallHangTimer -= Time.deltaTime;


        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;


    }

    public void performSlide(bool ground, bool facing,Vector2 input)
    {
        if (ground)
        {
            rb.gravityScale = defaultGravity;
            return;
        }

        rb.gravityScale = defaultGravity;
        float velocityY = rb.velocity.y;

        if (velocityY < wallSlideFallSpeed) velocityY = wallSlideFallSpeed;
        if (velocityY < maxFallSpeed) velocityY = maxFallSpeed;

        rb.velocity = new Vector2(0f, velocityY);
        // X 속도 억제
        bool pushingIntoWall = Mathf.Abs(input.x) > 0.01f && Mathf.Sign(input.x) == (facing ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(input.x) < 0.01f)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        Debug.Log($"행 타이머: {wallHangTimer:F2}, 중력: {rb.gravityScale}, Y속도: {rb.velocity.y:F2}");
    }
}
