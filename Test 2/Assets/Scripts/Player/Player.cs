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

    [Header("플레이어 움직임 설정")]
    public float MoveSpeed;
    public float JumpForce;


    public Player otherPlayer;
    public float PlayerDetectRadius;
    public LayerMask WhatisPlayer;



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

        if (wall.IswallDetected && !ground.IsgroundDetected)
        {
            climbing.UpdateClimbingState(ground.IsgroundDetected, wall.IswallDetected, _FacingRight);
        }
        else if (!ground.IsgroundDetected)
        {
            
        }
        else
        {
            
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (wall.IswallDetected && !ground.IsgroundDetected)
        {
            if (climbing.CheckWallJump(wall.IswallDetected, inputSystem.moveInput))
            {
                SetVelocity(climbing.wallJumpPower.x * climbing.wallJumpingDirection, climbing.wallJumpPower.y);
            }
            else if (inputSystem.Climbinginput())
            {
                climbing.PerformClimb();
            }
            else if (climbing.wallHangTimer > 0f)
            {
                climbing.performHang(ground.IsgroundDetected);
            }
            else if (climbing.wallHangTimer <= 0f)
            {
                climbing.performSlide(ground.IsgroundDetected, _FacingRight, inputSystem.moveInput);
            }
        }
        else if (!ground.IsgroundDetected)
        {
            if (inputSystem.moveInput.x != 0)
            {
                SetVelocity(inputSystem.moveInput.x * MoveSpeed, rb.velocity.y);
            }
        }
        else
        {
            if (inputSystem.JumpInput())
            {
                SetVelocity(rb.velocity.x, JumpForce);

            }
            else if (inputSystem.moveInput.x != 0)
            {
                SetVelocity(inputSystem.moveInput.x * MoveSpeed, rb.velocity.y);

            }
            else
            {
                SetVelocity(0, rb.velocity.y); 
            }
        }
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
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, PlayerDetectRadius, WhatisPlayer);

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
        Gizmos.DrawWireSphere(transform.position, PlayerDetectRadius);
    }

    public virtual void PlayerCrazy()
    {

    }
}
