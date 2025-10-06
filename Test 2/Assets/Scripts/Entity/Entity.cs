using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class WallDetected
{
    public float XwallCheckDistance; // 벽 감지 거리
    public Transform XwallCheck; // 벽 감지 위치
    public bool _wallDetected { get; private set; } // 벽 감지 여부
    public bool wallDetected => _wallDetected;
    public LayerMask WhatIsWall; // 벽 레이어

    // 벽 감지 업데이트
    public void UpdateWallDetected(float xDir)
    {
        bool xWall = Physics2D.Raycast(XwallCheck.position, Vector2.right * xDir, XwallCheckDistance, WhatIsWall);

        _wallDetected = xWall;
    }
}

// 벽 감지 클래스와 같음
[Serializable]
public class GroundDetected
{
    public float groundCheckDistance;
    public Transform groundCheck;
    public bool groundDetected { get; private set; }
    public LayerMask WhatIsGround;

    public void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, WhatIsGround);
    }

}

// 모든 엔티들의 기초가 되는 스크립트
public class Entity : MonoBehaviour
{
    public StateMachine stateMachine;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    [Header("벽 감지")]
    public WallDetected Wall;

    [Header("땅 감지")]
    public GroundDetected ground;

    public bool facingRight { get; set; } = true;
    public bool isFacingVertical { get; private set; }


    public int facingDir { get; private set; } = 1;


    public Vector2 direction { get; set; }
    public bool IsMove { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }


    protected virtual void Update()
    {
        Wall.UpdateWallDetected(XGizmoDirection);
        ground.HandleCollisionDetection();
        stateMachine.UpdateActiveState();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdateActiveState();
    }

    // 엔티티 움직임 설정
    public virtual void SetVelocity(float xvelocity, float yvelocity)
    {
        rb.velocity = new Vector2(xvelocity, yvelocity);

        XHandleFlip(xvelocity);
    }



    // x축 방향에 따른 캐릭터 뒤집기
    public void XHandleFlip(float x)
    {
        if (x > 0 && facingRight == false)
            xFlip();
        else if (x < 0 && facingRight)
            xFlip();
    }

    // 캐릭터 뒤집기
    public void xFlip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    // 기즈모 그리기
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(Wall.XwallCheck.position, Wall.XwallCheck.position + new Vector3(Wall.XwallCheckDistance * XGizmoDirection, 0));

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -ground.groundCheckDistance));
    }
    // x축 벽 감지 기즈모 방향 설정
    protected virtual float XGizmoDirection
    {
        get
        {
            return facingRight ? 1f : -1f;
        }
    }
}

