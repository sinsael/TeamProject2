using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class WallDetected
{
    public float XwallCheckDistance;
    public Transform XwallCheck;
    public bool _wallDetected { get; private set; }
    public bool wallDetected => _wallDetected;
    public LayerMask WhatIsWall;

    public void UpdateWallDetected(float xDir)
    {
        bool xWall = Physics2D.Raycast(XwallCheck.position, Vector2.right * xDir, XwallCheckDistance, WhatIsWall);

        _wallDetected = xWall;
    }
}

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

public class Entity : MonoBehaviour
{
    protected StateMachine stateMachine;

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
    [Space]
    public float Movespeed;
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

    public virtual void SetVelocity(float xvelocity, float yvelocity)
    {
        rb.velocity = new Vector2(xvelocity, yvelocity);

        XHandleFlip(xvelocity);
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



    protected virtual void OnDrawGizmos()
    {

        Gizmos.DrawLine(Wall.XwallCheck.position, Wall.XwallCheck.position + new Vector3(Wall.XwallCheckDistance * XGizmoDirection, 0));
    
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0,-ground.groundCheckDistance));
    }
    protected virtual float XGizmoDirection
    {
        get
        {
            return facingRight ? 1f : -1f;
        }
    }
}

