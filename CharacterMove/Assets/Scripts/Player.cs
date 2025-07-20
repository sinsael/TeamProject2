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
    public float walkCount;
    private float currentWalkCount;
    public bool canMove;




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
    public IEnumerator MovePlayer(float x, float y)
    {
        canMove = false; 

        float elapsed = 0f;
        float duration = 0.2f;


        while (elapsed < duration)
        {

            SetTransform(x, y);

            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        canMove = true;
    }

}

