using UnityEngine;

public class Player : Entity, IInteraction_circle
{
    // 컴포넌트들
    public PlayerInputHandler inputSystem { get; private set; }
    public Interaction interact { get;  set; }
    public Sanity sanity { get; set; }

    // 상태들
    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }

    [Header("플레이어 상세 설정")]
    public float MoveSpeed = 5;
    public float jumpForce = 5;
    [Header("초당 San수치 하락 설정")]
    public float Sanamount;
    public float Saninterval;
    protected bool PlayerDetectRay = false;




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

        // 입력에 따른 방향 설정
        direction = new Vector2(inputSystem.moveInput.x, 0);

        interact.FindBestTarget();
        interact.HandleTargetChange();
        interact.UpdateObjDetected();
        Intertable();
        FarawayPlayer();
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
            sanity.StartReduceSanity(Sanamount,Saninterval);
            sanity.StopIncreaseSanity();
        }
        else if (PlayerDetectRay)
        { 
            sanity.StopReduceSanity();
            sanity.StartIncreaseSanity(Sanamount,Saninterval);
        }
    }
}