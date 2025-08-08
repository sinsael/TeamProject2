using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class WallDetected
{
    public LayerMask WhatIsWall;
    public float XwallCheckDistance;
    public float YwallCheckDistance;
    public Transform XwallCheck;
    public Transform YwallCheck;
    [SerializeField] private bool _wallDetected;
    public bool wallDetected => _wallDetected;

    public void UpdateWallDetected(float xDir, float yDir)
    {   
        bool xWall = Physics2D.Raycast(XwallCheck.position, Vector2.right * xDir, XwallCheckDistance, WhatIsWall);
        bool yWall = Physics2D.Raycast(YwallCheck.position, Vector2.up * yDir, YwallCheckDistance, WhatIsWall);

        _wallDetected = xWall || yWall;
    }
}


public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachin;

    [Header("벽 감지")]
    public WallDetected Wall;

    public bool facingRight { get; set; } = true;
    public bool facingUp { get; set; } = true;
    public bool isFacingVertical { get; private set; }


    public int facingDir { get; private set; } = 1;

    [Header("움직임 설정")]
    [Range(0, 1)]
    public float moveTime = 0.5f;
    public bool IsMove { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachin = new StateMachine();
    }

    protected virtual void Start()
    {

    }


    protected virtual void Update()
    {
        Wall.UpdateWallDetected(XGizmoDirection, YGizmoDirection);
        stateMachin.UpdateActiveState();
    }

    public virtual void MoveBy(float x, float y)
    {
        if (IsMove) return; // 이동 중이면 무시
        Vector2 direction = new Vector2(x, y);

        if (direction == Vector2.zero) return;

        SetTransformDo(direction);
    }

    public virtual void SetTransformDo(Vector2 direction)
    {
        IsMove = true;

        XHandleFlip(direction.x);

        transform.DOMove(transform.position + (Vector3)direction, moveTime)
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            IsMove = false;
        });
    }

    public void XHandleFlip(float x)
    {
        if (x > 0 && facingRight == false)
            xFlip();
        else if (x < 0 && facingRight)
            xFlip();
    }

    public void xFlip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected virtual void yFlip(float a, float b)
    {
        if (a > 0) facingUp = true;
        else if (b < 0) facingUp = false;
    }

    protected virtual void GizmosDirection()
    {
        if (XGizmoDirection != 0)
            isFacingVertical = false;
        else if (YGizmoDirection != 0)
            isFacingVertical = true;
    }

    protected virtual void OnDrawGizmos()
    {

        if (XGizmoDirection != 0 && !isFacingVertical)
            Gizmos.DrawLine(Wall.XwallCheck.position, Wall.XwallCheck.position + new Vector3(Wall.XwallCheckDistance * XGizmoDirection, 0));
        
        if (YGizmoDirection != 0 && isFacingVertical)
            Gizmos.DrawLine(Wall.YwallCheck.position, Wall.YwallCheck.position + new Vector3(0, Wall.YwallCheckDistance * YGizmoDirection));
    }
    protected virtual float XGizmoDirection
    {
        get
        {
            return facingRight ? 1f : -1f;
        }
    }
    protected virtual float YGizmoDirection
    {
        get
        {
            return facingUp ? 1f : -1f;
        }
    }

}

