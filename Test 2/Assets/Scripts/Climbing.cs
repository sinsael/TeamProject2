using UnityEngine;

public class Climbing : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Hang / Slide")]
    public float wallHangDelay = 3f;           
    public float wallSlideFallSpeed = -0.3f; 
    public float maxFallSpeed = -8f;       

    [Header("Climb")]
    public float wallClimbSpeed;

    [Header("Wall Jump")]
    public Vector2 wallJumpPower;
    public float controlLock = 0.15f;         
    public float wallCoyoteTime = 0.2f;           

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

    public void EnterState(bool ground)
    {
        // �� �����̸� ���� ����
        if (ground) return;

        defaultGravity = rb.gravityScale;
        ResetTimers();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        Debug.Log("��Ÿ�� ���� ����");
    }

    public void ExitState()
    {
        rb.gravityScale = defaultGravity;
    }

    public void ResetTimers()
    {
        wallHangTimer = wallHangDelay;
        wallCoyoteTimer = wallCoyoteTime;
        lockTimer = 0f;
    }


    public void UpdateClimbingState(bool ground, bool wall, bool facing)
    {
        if (wallCoyoteTimer > 0f) wallCoyoteTimer -= Time.deltaTime;
        if (lockTimer > 0f) lockTimer -= Time.deltaTime;

        if (ground)
        {
            return;
        }

        if (wall)
        {
            wallCoyoteTimer = wallCoyoteTime;
        }

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
            rb.linearVelocity = new Vector2(0f, wallClimbSpeed);
    }

    public void performHang(bool ground)
    {
        if (ground)
        {
            rb.gravityScale = defaultGravity;
            return;
        }

        wallHangTimer -= Time.deltaTime;


        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;


    }

    public void performSlide(bool ground, bool facing,Vector2 input)
    {
        if (ground)
        {
            rb.gravityScale = defaultGravity;
            return;
        }

        rb.gravityScale = defaultGravity;
        float velocityY = rb.linearVelocity.y;

        if (velocityY < wallSlideFallSpeed) velocityY = wallSlideFallSpeed;
        if (velocityY < maxFallSpeed) velocityY = maxFallSpeed;

        rb.linearVelocity = new Vector2(0f, velocityY);

        bool pushingIntoWall = Mathf.Abs(input.x) > 0.01f && Mathf.Sign(input.x) == (facing ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(input.x) < 0.01f)
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
}
