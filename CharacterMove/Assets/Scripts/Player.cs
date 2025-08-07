using System;
using Unity.VisualScripting;
using UnityEngine;



[Serializable]
public class NPCDetected
{
    public LayerMask WhatIsNpc;
    public float XNpcCheckDistance;
    public float YNpcCheckDistance;
    public Transform XNpcCheck;
    public Transform YNpcCheck;
    [SerializeField] bool _npcDetected;
    public bool npcDetected => _npcDetected;

    public void UpdateNpcDetected(float xdir, float ydir)
    {
        _npcDetected |= Physics2D.Raycast(XNpcCheck.position, Vector2.right * xdir, WhatIsNpc);
        _npcDetected |= Physics2D.Raycast(YNpcCheck.position, Vector2.up * ydir, WhatIsNpc);
    }

}


public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_YMoveState yMoveState { get; private set; }
    public Vector2 moveInput { get; private set; }



    [Header("NPC 감지")]
    public NPCDetected npc;



    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, stateMachin, "Idle");
        xMoveState = new Player_XMoveState(this, stateMachin, "XMove");
        yMoveState = new Player_YMoveState(this, stateMachin, "YMove");

    }

    protected override void Start()
    {
        base.Start();
        stateMachin.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        npc.UpdateNpcDetected(moveInput.x, moveInput.y);
        yFlip(moveInput.y, moveInput.y);
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
            return moveInput.y != 0 ? Mathf.Sign(moveInput.y) : 0f;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(npc.XNpcCheck.position, npc.XNpcCheck.position + new Vector3(facingRight ? npc.XNpcCheckDistance : -npc.XNpcCheckDistance, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(npc.YNpcCheck.position, npc.YNpcCheck.position + new Vector3(0, facingUp ? npc.YNpcCheckDistance : -npc.YNpcCheckDistance));
    }

    void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx =>
        {
            Vector2 raw = ctx.ReadValue<Vector2>();

            // 대각선 입력일 경우 x 또는 y 중 절댓값이 더 큰 방향만 살림
            if (Mathf.Abs(raw.x) > Mathf.Abs(raw.y))
                moveInput = new Vector2(Mathf.Sign(raw.x), 0); // 수평 우선
            else
                moveInput = new Vector2(0, Mathf.Sign(raw.y)); // 수직 우선
        };

        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        input.Disable();
    }

    public override void MoveBy(float x, float y)
    {
        if (Wall.wallDetected) return;

        base.MoveBy(x, y);
    }





}

