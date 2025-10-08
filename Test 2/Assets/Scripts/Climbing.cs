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
    public float wallClimbSpeed;           // ��Ű (W, UpArrow) ���� �� ������ �ӵ�

    [Header("Wall Jump")]
    public Vector2 wallJumpPower; // ������ �� �� (X: ����, Y: ����)
    public float wallJumpDuration = 1f;                 // ������ ���� �ð� (������ ������ �� ���� ���� ���� �ð�)
    public float controlLock = 0.15f;                   // ������ ���� �Է� ��� �ð� (��)
    public float wallCoyoteTime = 0.2f;                 // ������ ������ �ڿ��� ���� ������ ���� �ð� (��)
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

    // ���¿� ������ �� Ÿ�̸� ���� �ʱ�ȭ�ϴ� �Լ�
    public void EnterState()
    {
        // �� �����̸� ���� ����
        if (ground.IsgroundDetected) return;

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
        wallHangTimer = climbing.wallHangDelay;
        wallCoyoteTimer = climbing.wallCoyoteTime;
        lockTimer = 0f;
    }


    public void UpdateClimbingState()
    {
        // �� ������ �ð� ���� ó��
        if (wallCoyoteTimer > 0f) wallCoyoteTimer -= Time.deltaTime;
        if (lockTimer > 0f) lockTimer -= Time.deltaTime;

        // + �����̸� ��Ÿ�� ���� ���� !!
        if (ground.IsgroundDetected)
        {
            return;
        }

        // �� ����
        if (wall.IswallDetected)
        {
            wallCoyoteTimer = climbing.wallCoyoteTime;
        }

        // �� �ݴ� ���� ����
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
        // ���� ���� �߰� 
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
        // + ���� �� ���� ������ ��� ����(�߷� ����) !!
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
        // X �ӵ� ����
        bool pushingIntoWall = Mathf.Abs(Input.moveInput.x) > 0.01f && Mathf.Sign(Input.moveInput.x) == (player._FacingRight ? 1 : -1);
        if (pushingIntoWall || Mathf.Abs(Input.moveInput.x) < 0.01f)
            rb.velocity = new Vector2(0f, rb.velocity.y);

        Debug.Log($"�� Ÿ�̸�: {wallHangTimer:F2}, �߷�: {rb.gravityScale}, Y�ӵ�: {rb.velocity.y:F2}");
    }

    public void WallJump()
    {
        rb.gravityScale = defaultGravity;
        player.SetVelocity(climbing.wallJumpPower.x * wallJumpingDirection, climbing.wallJumpPower.y);

        // ����� ����
        player.wallAttachLockTimer = 0.2f;

        // Climbing.controlLock ���
        lockTimer = climbing.controlLock;
    }
}
