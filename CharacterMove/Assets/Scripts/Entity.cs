using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Burst.Intrinsics;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ProjectWindowCallback;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachin;

    private bool facingRight = true;
    private bool facingUp = true;
    public int facingDir { get; private set; } = 1;


    [Header("충돌 설정")]
    [SerializeField] protected LayerMask WhatIsWall;
    [SerializeField] private float XwallCheckDistance;
    [SerializeField] private float YwallCheckDistance;
    [SerializeField] private Transform XwallCheck;
    [SerializeField] private Transform YwallCheck;
    public bool wallDetected { get; private set; }
    [Space]
    [Header("움직임 설정")]
    [Range(0, 1)]
    public float moveTime = 0.5f;
    public bool IsMove { get; private set; }

    protected virtual void Awake()
    {
        //  anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachin = new StateMachine();
    }

    protected virtual void Start()
    {

    }


    protected virtual void Update()
    {
        HandleCollisionDetecion();
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

    public void ResetXFlip()
    {
        if (!facingRight)
        {
            xFlip(); // 왼쪽이면 오른쪽으로 되돌림
        }
    }

    private void HandleCollisionDetecion()
    {
        bool xWall = Physics2D.Raycast(XwallCheck.position, Vector2.right * XGizmoDirection, XwallCheckDistance, WhatIsWall);
        bool Ywall = Physics2D.Raycast(YwallCheck.position, Vector2.up * YGizmoDirection, YwallCheckDistance, WhatIsWall);

        if (xWall || Ywall)
            wallDetected = true;
        else
            wallDetected = false;
    }

    protected virtual void OnDrawGizmos()
    {
        if (XGizmoDirection != 0)
        {
            Gizmos.DrawLine(XwallCheck.position, XwallCheck.position + new Vector3(XwallCheckDistance * XGizmoDirection, 0));
        }
        if (YGizmoDirection != 0)
        {
            Gizmos.DrawLine(YwallCheck.position,
                YwallCheck.position + new Vector3(0, YwallCheckDistance * YGizmoDirection));
        }
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

