using UnityEngine;

public class Climbing : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Hang / Slide")]
    public float wallHangDelay = 3f;            // �Ŵ޸��� �ð� (��)_��ȹ�� ��� 3��
    public float wallSlideFallSpeed = -0.3f;    // ������ �������� �ӵ�_��ȹ�� ��� 0.3 �߷°�
    public float maxFallSpeed = -8f;            // �ִ� ���� �ӵ�

    [Header("Climb")]
    public float wallClimbSpeed;           // ��Ű (W, UpArrow) ���� �� ������ �ӵ�

    [Header("Wall Jump")]
    public Vector2 wallJumpPower; // ������ �� �� (X: ����, Y: ����)
    public float controlLock = 0.15f;                   // ������ ���� �Է� ��� �ð� (��)
    public float wallCoyoteTime = 0.2f;                 // ������ ������ �ڿ��� ���� ������ ���� �ð� (��)

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

    // ���¿� ������ �� Ÿ�̸� ���� �ʱ�ȭ�ϴ� �Լ�
    public void EnterState(bool ground)
    {
        // �� �����̸� ���� ����
        if (ground) return;

        // �� ���� ������ �ٽ� ���(����/ȯ������ �߷� �ٲ���� �� ����)
        defaultGravity = rb.gravityScale;
        ResetTimers();
        rb.velocity = new Vector2(rb.velocity.x, 0f);

        Debug.Log("��Ÿ�� ���� ����");
    }

    // ���¸� ���� �� �߷��� ������� ������ �Լ�
    public void ExitState()
    {
        rb.gravityScale = defaultGravity;
    }

    // Ÿ�̸� �ʱ�ȭ ������ ���� �Լ��� �и�
    public void ResetTimers()
    {
        wallHangTimer = wallHangDelay;
        wallCoyoteTimer = wallCoyoteTime;
        lockTimer = 0f;
    }


    public void UpdateClimbingState(bool ground, bool wall, bool facing)
    {
        // �� ������ �ð� ���� ó��
        if (wallCoyoteTimer > 0f) wallCoyoteTimer -= Time.deltaTime;
        if (lockTimer > 0f) lockTimer -= Time.deltaTime;

        // + �����̸� ��Ÿ�� ���� ���� !!
        if (ground)
        {
            return;
        }

        // �� ����
        if (wall)
        {
            wallCoyoteTimer = wallCoyoteTime;
        }

        // �� �ݴ� ���� ����
        if (wall)
        {
            wallJumpingDirection = facing ? -1 : 1;
        }


    }

    public virtual void WallDirection(bool wall, bool facing)
    {
        if (wall)
        {
            wallJumpingDirection = facing ? -1 : 1;
        }
    }

    public bool CheckWallJump(bool wall, Vector2 input, Entity entity)
    {
        bool hasHorizontalInput = Mathf.Abs(input.x) > 0.1f;

        bool isPushingAwayFromWall = Mathf.Sign(input.x) == wallJumpingDirection;

        if ((wall || wallCoyoteTimer > 0f) && hasHorizontalInput && isPushingAwayFromWall && lockTimer <= 0f)
        {
            WallJump(entity);
            return true;
        }
        return false;
    }

    public bool CheckAndPerformClimb(bool ground, bool wall, bool input)
    {
        // ���� ���� �߰� 
        if (wall && !ground && input)
        {
            ResetTimers();
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0f, wallClimbSpeed);
            return true;
        }
        return false;
    }

    public void performHang(bool ground)
    {
        // + ���� �� ���� ������ ��� ����(�߷� ����) !!
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
        // X �ӵ� ����
        bool pushingIntoWall = Mathf.Abs(input.x) > 0.01f && Mathf.Sign(input.x) == (facing ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(input.x) < 0.01f)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        Debug.Log($"�� Ÿ�̸�: {wallHangTimer:F2}, �߷�: {rb.gravityScale}, Y�ӵ�: {rb.velocity.y:F2}");
    }

    public void WallJump(Entity entity)
    {
        rb.gravityScale = defaultGravity;
        entity.SetVelocity(wallJumpPower.x * wallJumpingDirection, wallJumpPower.y);


        // Climbing.controlLock ���
        lockTimer = controlLock;
    }
}
