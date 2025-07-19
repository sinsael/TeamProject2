using System.Collections;
using System.Collections.Generic;
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

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        XHandleFlip(xVelocity);
    }

    public void XHandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingRight == false)
            xFlip();
        else if (xVelocity < 0 && facingRight)
            xFlip();
    }

    public void xFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = facingDir * -1;
    }

    private void HandleCollisionDetecion()
    {
        bool xWall = Physics2D.Raycast(XwallCheck.position, Vector2.right * XGizmoDirection, XwallCheckDistance, WhatIsWall);
        bool Ywall = Physics2D.Raycast(YwallCheck.position, Vector2.up * YGizmoDirection, YwallCheckDistance, WhatIsWall);

        if (xWall || Ywall)
        {
            Debug.Log("벽 충돌 감지");
        }
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

