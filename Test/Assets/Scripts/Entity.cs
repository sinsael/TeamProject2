using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class WallDetected
{
    public float XwallCheckDistance;
    public float YwallCheckDistance;
    public Transform XwallCheck;
    public Transform YwallCheck;
    [SerializeField] private bool _wallDetected;
    public bool wallDetected => _wallDetected;
    public LayerMask WhatIsWall;

    public void UpdateWallDetected(float xDir, float yDir)
    {   
        bool xWall = Physics2D.Raycast(XwallCheck.position, Vector2.right * xDir, XwallCheckDistance, WhatIsWall);
        bool yWall = Physics2D.Raycast(YwallCheck.position, Vector2.up * yDir, YwallCheckDistance, WhatIsWall);

        _wallDetected = xWall || yWall;
    }
}


public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    [Header("벽 감지")]
    public WallDetected Wall;

    public bool facingRight { get; set; } = true;
    public bool facingUp { get; set; } = true;
    public bool isFacingVertical { get; private set; }


    public int facingDir { get; private set; } = 1;


    public Vector2 direction { get; set; }
    public float speed;
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
        Wall.UpdateWallDetected(XGizmoDirection, YGizmoDirection);
        stateMachine.UpdateActiveState();
    }

    public virtual void MoveBy(float x, float y)
    {
        if (direction == Vector2.zero) return;

        SetTransformDo(speed * direction);
    }

    public virtual void SetTransformDo(Vector2 direction)
    {
        IsMove = true;

        XHandleFlip(direction.x);

        transform.DOMove(transform.position + (Vector3)direction, 0.1f)
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

