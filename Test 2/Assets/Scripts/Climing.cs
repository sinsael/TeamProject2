using System;
using UnityEngine;

[Serializable]
public class ClimingSetting
{
    [Header("Hang / Slide")]
    public float wallHangDelay = 3f;            // �Ŵ޸��� �ð� (��)_��ȹ�� ��� 3��
    public float wallSlideFallSpeed = -0.3f;    // ������ �������� �ӵ�_��ȹ�� ��� 0.3 �߷°�
    public float maxFallSpeed = -8f;            // �ִ� ���� �ӵ�

    [Header("Climb")]
    public float wallClimbSpeed = 2f;           // ��Ű (W, UpArrow) ���� �� ������ �ӵ�

    [Header("Wall Jump")]
    public Vector2 wallJumpPower = new Vector2(2f, 4f); // ������ �� �� (X: ����, Y: ����)
    public float wallJumpDuration = 1f;                 // ������ ���� �ð� (������ ������ �� ���� ���� ���� �ð�)
    public float controlLock = 0.15f;                   // ������ ���� �Է� ��� �ð� (��)
    public float wallCoyoteTime = 0.2f;                 // ������ ������ �ڿ��� ���� ������ ���� �ð� (��)
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

    // ���¿� ������ �� Ÿ�̸� ���� �ʱ�ȭ�ϴ� �Լ�
    public void EnterState()
    {
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
        wallHangTimer = climing.wallHangDelay;
        wallCoyoteTimer = 0f;
        lockTimer = 0f;
    }

    public void UpdateClimbingState()
    {
        // �� ������ �ð� ���� ó��
        if (wallCoyoteTimer > 0f) wallCoyoteTimer -= Time.deltaTime;
        if (lockTimer > 0f) lockTimer -= Time.deltaTime;

        // �� ����
        if (wall.wallDetected)
        {
            isOnWall = true;
            wallCoyoteTimer = climing.wallCoyoteTime;
        }
        else
        {
            isOnWall = false;
        }

        // �� �ݴ� ���� ����
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

        // X �ӵ� ����
        bool pushingIntoWall = Mathf.Abs(Input.moveInput.x) > 0.01f && Mathf.Sign(Input.moveInput.x) == (player.facingRight ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(Input.moveInput.x) < 0.01f)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        Debug.Log($"�� Ÿ�̸�: {wallHangTimer:F2}, �߷�: {rb.gravityScale}, Y�ӵ�: {rb.velocity.y:F2}");
        Debug.Log(Time.timeScale);
    }

    private void WallJump()
    {
        rb.gravityScale = defaultGravity;
        rb.velocity = new Vector2(wallJumpingDirection * climing.wallJumpPower.x, climing.wallJumpPower.y);

        // �ٶ󺸴� ���� ����
        if ((wallJumpingDirection > 0 && !player.facingRight) ||
            (wallJumpingDirection < 0 && player.facingRight))
        {
            player.xFlip();
        }

        // ����� ����
        player.wallAttachLockTimer = 0.2f;

        // climing.controlLock ���
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
