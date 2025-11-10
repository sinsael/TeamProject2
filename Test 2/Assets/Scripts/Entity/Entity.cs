using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class WallDetected
{
    public float XwallCheckDistance; // 벽 감지 거리
    public Transform XwallCheck; // 벽 감지 위치
    public int DetectedWallDirection { get; private set; }
    public bool IswallDetected { get; private set; }
    public LayerMask WhatIsWall; // 벽 레이어

    // 벽 감지 업데이트
    public void UpdateWallDetected(float xDir)
    {
        IswallDetected = Physics2D.Raycast(XwallCheck.position, Vector2.right * xDir, XwallCheckDistance, WhatIsWall);

        if (IswallDetected)
        {
            DetectedWallDirection = (int)xDir;
        }
    }   

    // 땅에 닿아있으면 벽 감지 무시
    public void MaskByGround(bool grounded)
    {
        if (grounded) IswallDetected = false;
    }

}

// 벽 감지 클래스와 같음
[Serializable]
public class GroundDetected
{
    public float GroundCheckDistance;
    public Transform GroundCheck;
    public bool IsgroundDetected { get; private set; }
    public LayerMask WhatIsGround;

    // + 땅 감지 업데이트 !!
    public void HandleCollisionDetection()
    {
        IsgroundDetected = Physics2D.Raycast(GroundCheck.position, Vector2.down, GroundCheckDistance, WhatIsGround);
    }

}

// 모든 엔티들의 기초가 되는 스크립트
public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    [Header("벽 감지")]
    public WallDetected wall;

    [Header("땅 감지")]
    public GroundDetected ground;

   

    public bool _FacingRight { get; private set; } = true;


    public Vector2 Direction { get; set; }

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
        //Wall.UpdateWallDetected(XGizmoDirection);        // 호출 순서 변경
        //ground.HandleCollisionDetection();
        //stateMachine.UpdateActiveState();

        ground.HandleCollisionDetection();                 // 1) 땅 먼저 
        wall.UpdateWallDetected(XGizmoDirection);          // 2) 벽
        wall.MaskByGround(ground.IsgroundDetected);          // 3) 땅이면 벽 false + 추가
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
    }



    // x축 방향에 따른 캐릭터 뒤집기
    public void XHandleFlip(float x)
    {
        if (x > 0 && _FacingRight == false)
            xFlip();
        else if (x < 0 && _FacingRight)
            xFlip();
    }

    // 캐릭터 뒤집기
    public void xFlip()
    {
        _FacingRight = !_FacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    // 기즈모 그리기
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wall.XwallCheck.position, wall.XwallCheck.position + new Vector3(wall.XwallCheckDistance * XGizmoDirection, 0));

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -ground.GroundCheckDistance));
    }
    // x축 벽 감지 기즈모 방향 설정
    public virtual float XGizmoDirection
    {
        get
        {
            return _FacingRight ? 1f : -1f;
        }
    }
}

