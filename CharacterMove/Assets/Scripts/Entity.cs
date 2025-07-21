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
    [Space]
    [Header("움직임 설정")]
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

    public virtual void SetTransform(float x, float y)
    {
        if (IsMove) return; // 이동 중이면 무시
        StartCoroutine(MoveBy(new Vector2(x, y)));
    }

    public virtual IEnumerator MoveBy(Vector2 direction)
    {
        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)direction;

        float elapsed = 0f;
        IsMove = true;

        // 이동 방향에 따라 좌우 반전 한번 처리 (필요하면 매 프레임으로 조정 가능)
        XHandleFlip(direction.x);

        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / moveTime);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;

        IsMove = false;
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

