using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_YMoveState yMoveState { get; private set; }

    [Header("움직임 설정")]
    public float moveSpeed;
    public Vector2 moveInput { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, stateMachin, "idle");
        xMoveState = new Player_XMoveState(this, stateMachin, "xMove");
        yMoveState = new Player_YMoveState(this, stateMachin, "yMove");

    }

    protected override void Start()
    {
        base.Start();
        stateMachin.Initialize(idleState);
    }

    protected override float XGizmoDirection
    {
        get
        {
            return moveInput.x != 0 ? Mathf.Sign(moveInput.x) : 0f;
        }
    }

    protected override float YGizmoDirection
    {
        get
        {
            return moveInput.y != 0 ? Mathf.Sign(-moveInput.y) : 0f;
        }
    }

    void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx =>
        {
            var raw = ctx.ReadValue<Vector2>();
            moveInput = Mathf.Abs(raw.x) > 0.1f ? new Vector2(raw.x, 0) : new Vector2(0, raw.y);
        };

        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        input.Disable();
    }
}
