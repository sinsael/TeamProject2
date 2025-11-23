using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player : Entity
{
    // 컴포넌트들
    public PlayerInputHandler inputSystem { get; private set; }
    public Interaction interact { get; private set; }
    public Sanity sanity { get; private set; }
    public Climbing climbing { get; private set; }
    public Entity_Stat playerStat { get; private set; }

    public PlayerStates CurrentState;

    [Header("플레이어 움직임 설정")]
    public float MoveSpeed;
    public float JumpForce;

    public Player otherPlayer;
    public float PlayerDetectRadius;
    public LayerMask WhatisPlayer;
    public Transform playerCheck;



    protected override void Awake()
    {
        base.Awake();

        inputSystem = GetComponent<PlayerInputHandler>();
        interact = GetComponent<Interaction>();
        sanity = GetComponent<Sanity>();
        climbing = GetComponent<Climbing>();
        playerStat = GetComponent<Entity_Stat>();

    }

    protected override void Start()
    {
        base.Start();
        MoveSpeed = playerStat.GetSpeed();
        JumpForce = playerStat.GetJumpForce();
    }

    protected override void Update()
    {
        base.Update();

        // 입력에 따른 방향 설정
        Direction = new Vector2(inputSystem.moveInput.x, 0f);

        XHandleFlip(inputSystem.moveInput.x);

        interact.FindBestTarget();
        interact.HandleTargetChange();
        Intertable();
        HandleProximitySanity();

        HandleMovementLogic();
    }

    private void HandleMovementLogic()
    {
        // --- [A] 벽 상태 로직 ---
        if (wall.IswallDetected && !ground.IsgroundDetected)
        {
            climbing.UpdateClimbingState(ground.IsgroundDetected, wall.IswallDetected, _FacingRight);

            // A-1. 벽 점프 (가장 높은 우선순위)
            // ★ 여기서 CheckWallJump를 한 번만 호출하므로 이중 호출 문제 해결
            if (climbing.CheckWallJump(wall.IswallDetected, inputSystem.moveInput))
            {
                SetVelocity(climbing.wallJumpPower.x * climbing.wallJumpingDirection, climbing.wallJumpPower.y);
                CurrentState = PlayerStates.Jump; // 상태 강제 설정
                Debug.Log("Wall Jumped");
                return; // 점프했으면 다른 벽 로직 무시
            }

            // A-2. 벽 타기 (Climb)
            if (inputSystem.Climbinginput())
            {
                climbing.PerformClimb();
                CurrentState = PlayerStates.WallClimb;
            }
            // A-3. 벽 매달리기 (Hang)
            else if (climbing.wallHangTimer > 0f)
            {
                climbing.performHang(ground.IsgroundDetected);
                CurrentState = PlayerStates.WallHang;
            }
            // A-4. 벽 슬라이드 (Slide)
            else
            {
                climbing.performSlide(ground.IsgroundDetected, _FacingRight, inputSystem.moveInput);
                CurrentState = PlayerStates.WallSlide;
            }
        }
        else
        {
            // ★ 벽에서 벗어났다면 즉시 중력 복구 (공중부양 버그 방지)
            if (climbing != null) climbing.ExitState();

            // B-1. 공중 (점프 중이거나 떨어지는 중)
            if (!ground.IsgroundDetected)
            {
                // 공중 이동 제어
                if (inputSystem.moveInput.x != 0)
                {
                    SetVelocity(inputSystem.moveInput.x * MoveSpeed, rb.linearVelocity.y);
                }

                // 상태 결정 (상승 중이면 Jump, 아니면 떨어지는 모션이지만 여기선 Jump로 통일하신 듯 합니다)
                CurrentState = PlayerStates.Jump;
            }
            // B-2. 땅 (Ground)
            else
            {
                if (inputSystem.JumpInput())
                {
                    SetVelocity(rb.linearVelocity.x, JumpForce);
                    CurrentState = PlayerStates.Jump;
                    Debug.Log("Jumped");
                }
                else if (inputSystem.moveInput.x != 0)
                {
                    SetVelocity(inputSystem.moveInput.x * MoveSpeed, rb.linearVelocity.y);
                    CurrentState = PlayerStates.Move;
                }
                else if (inputSystem.CrouchInput())
                {
                    SetVelocity(0, rb.linearVelocity.y);
                    CurrentState = PlayerStates.Crouch;
                }
                else
                {
                    SetVelocity(0, rb.linearVelocity.y);
                    CurrentState = PlayerStates.Idle;
                }
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();


    }


    // 상호작용 처리
    private void Intertable()
    {
        if (inputSystem.InteractableInput())
        {
            interact?.Interact();
        }
    }

    private void HandleProximitySanity()
    {
        // 1. 참조가 없으면 (싱글 플레이 등) 실행 중지
        if (sanity == null || otherPlayer == null)
            return;

        // 2. 내 위치(transform.position)를 기준으로 PlayerDetectRadius 반경 안에
        //    WhatisPlayer 레이어에 해당하는 모든 콜라이더를 찾습니다.
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(playerCheck.position, PlayerDetectRadius, WhatisPlayer);

        bool isOtherPlayerNearby = false;

        // 3. 감지된 콜라이더들 중에 'otherPlayer'가 있는지 확인합니다.
        foreach (var collider in detectedColliders)
        {
            // 감지된 콜라이더의 게임 오브젝트가 'otherPlayer'의 게임 오브젝트와 같다면
            if (collider.gameObject == otherPlayer.gameObject)
            {
                isOtherPlayerNearby = true;
                break;
            }
        }

        // 4. [핵심] 'otherPlayer'가 내 감지 범위 안에 있을 때
        if (isOtherPlayerNearby)
        {
            // 정신력 회복
            sanity.RegenerateSanpoint();
        }
        else // 'otherPlayer'가 내 감지 범위 밖에 있을 때
        {
            // 정신력 지속 감소
            sanity.DrainSanpoint();
        }
    }

    // 디버그용 기즈모
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerCheck.position, PlayerDetectRadius);
    }

    public virtual void PlayerCrazy()
    {

    }
}
