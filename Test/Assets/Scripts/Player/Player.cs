using UnityEngine;



[System.Serializable] // 벽타기 관련수치들
public class WallClimbSettings
{
    [Header("Hang / Slide")]
    public float wallHangDelay = 3f;            // 매달리는 시간 (초)_기획서 대로 3초
    public float wallSlideFallSpeed = -0.3f;    // 벽에서 떨어지는 속도_기획서 대로 0.3 중력값
    public float maxFallSpeed = -8f;            // 최대 낙하 속도

    [Header("Climb")]
    public float wallClimbSpeed = 2f;           // 윗키 (W, UpArrow) 누를 때 오르는 속도

    [Header("Wall Jump")]
    public Vector2 wallJumpPower = new Vector2(2f, 4f); // 벽점프 시 힘 (X: 수평, Y: 수직)
    public float wallJumpDuration = 1f;                 // 벽점프 유지 시간 (벽에서 떨어진 후 점프 영향 지속 시간)
    public float controlLock = 0.15f;                   // 벽점프 직후 입력 잠금 시간 (초)
    public float wallCoyoteTime = 0.2f;                 // 벽에서 떨어진 뒤에도 점프 가능한 유예 시간 (초)
}

public class Player : Entity, IInteraction_circle
{
    // 컴포넌트들
    public PlayerInputHandler inputSystem { get; private set; }
    public Interaction interact { get; set; }
    public Sanity sanity { get; set; }

    // 상태들
    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallClimbingState Player_WallClimbingState { get; private set; } // + 벽타기 상태 추가 !!

    [Header("플레이어 움직임 설정")]
    public float MoveSpeed = 5;
    public float jumpForce = 5;
    [Header("초당 San수치 하락 설정")]
    public float Sanamount;
    public float Saninterval;
    protected bool PlayerDetectRay = false;


    // -----------추가-----------
    [Header("벽타기 설정")]
    public WallClimbSettings wallClimbSettings;

    public float wallAttachLockTimer { get; set; } = 0.2f;

    // -----------끝-----------


    protected override void Awake()
    {
        base.Awake();

        inputSystem = GetComponent<PlayerInputHandler>();
        interact = GetComponent<Interaction>();
        sanity = GetComponent<Sanity>();

        // 상태들 초기화
        idleState = new Player_IdleState(this, stateMachine, "Idle");
        xMoveState = new Player_XMoveState(this, stateMachine, "XMove");
        jumpState = new Player_JumpState(this, stateMachine, "JumpFall");
        fallState = new Player_FallState(this, stateMachine, "JumpFall");
        Player_WallClimbingState = new Player_WallClimbingState(this, stateMachine, "Wall"); // + 벽타기 상태 초기화 추가 !!
    }

    protected override void Start()
    {
        base.Start();

        // 초기 상태 설정
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        // 잠금 타이머 감소
        if (wallAttachLockTimer > 0f)
            wallAttachLockTimer -= Time.deltaTime;


        // 입력에 따른 방향 설정
        direction = new Vector2(inputSystem.moveInput.x, 0f);

        WallLock();
        interact.FindBestTarget();
        interact.HandleTargetChange();
        interact.UpdateObjDetected();
        Intertable();
        FarawayPlayer();

    }

    private void WallLock()
    {
        // -----------추가-----------
        // 벽을 감지하면 벽타기 전이 (단, 입력 잠금(ControlLock) 중이면 금지)
        if (Wall.wallDetected
            && wallAttachLockTimer <= 0f
            && stateMachine.currentState != Player_WallClimbingState)
        {
            stateMachine.ChangeState(Player_WallClimbingState);
            return;
        }
        // -----------끝-----------
    }

    // 상호작용 처리
    private void Intertable()
    {
        if (inputSystem.InteractableInput())
        {
            interact.Interact();
        }
    }

    // 디버그용 기즈모
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (interact == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interact.ObjCheck.position, interact.ObjCheckRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interact.interactionCheck.position, interact.interactionRadius);

    }

    public virtual void OnHitByRay()
    {
        PlayerDetectRay = true;
    }

    public virtual void OnLeaveRay()
    {
        PlayerDetectRay = false;
    }

    public virtual void FarawayPlayer()
    {
        if (!PlayerDetectRay)
        {
            sanity.StartReduceSanity(Sanamount, Saninterval);
            sanity.StopIncreaseSanity();
        }
        else if (PlayerDetectRay)
        {
            sanity.StopReduceSanity();
            sanity.StartIncreaseSanity(Sanamount, Saninterval);
        }
    }
}
