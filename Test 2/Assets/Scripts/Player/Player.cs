using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private PlayerWallCrushAnimEvent wallCrushEvent;
    private bool interactHeld;
    private float nextWallCrushTime; //추가
    private float otherInputLock;

    private PushOBJHandler pushHandler;



    protected override void Awake()
    {
        base.Awake();

        inputSystem = GetComponent<PlayerInputHandler>();
        interact = GetComponent<Interaction>();
        sanity = GetComponent<Sanity>();
        climbing = GetComponent<Climbing>();
        playerStat = GetComponent<Entity_Stat>();

        wallCrushEvent = GetComponentInChildren<PlayerWallCrushAnimEvent>(true); // 추가
        pushHandler = GetComponent<PushOBJHandler>();

    }

    protected override void Start()
    {
        base.Start();
        MoveSpeed = playerStat.GetSpeed();
        JumpForce = playerStat.GetJumpForce();
    }

    protected override void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.GameOver)
        {
            SetVelocity(0, 0);

            return;
        }

        if (Time.timeScale == 0)
            return;
        base.Update();

        if (Time.time < otherInputLock)
        {
            SetVelocity(0f, rb.linearVelocity.y);
            CurrentState = PlayerStates.WallCrush;
            return;
        }

        Direction = new Vector2(inputSystem.moveInput.x, 0f);


        interact.FindBestTarget();
        interact.HandleTargetChange();
        Intertable();
        HandleProximitySanity();

        HandleMovementLogic();

        if (CurrentState != PlayerStates.WallHang && CurrentState != PlayerStates.WallClimb)
        {
            XHandleFlip(inputSystem.moveInput.x);
        }
    }

    private void HandleMovementLogic()
    {

        if (wall.IswallDetected && !ground.IsgroundDetected)
        {
            climbing.UpdateClimbingState(ground.IsgroundDetected, wall.IswallDetected, _FacingRight);

            if (climbing.CheckWallJump(wall.IswallDetected, inputSystem.moveInput))
            {
                SetVelocity(climbing.wallJumpPower.x * climbing.wallJumpingDirection, climbing.wallJumpPower.y);
                CurrentState = PlayerStates.Jump;
                Debug.Log("Wall Jumped");
                return;
            }

            if (climbing.lockTimer > 0f)
            {
                return;
            }

            if (inputSystem.Climbinginput())
            {
                climbing.PerformClimb();
                CurrentState = PlayerStates.WallClimb;
            }
            else if (climbing.wallHangTimer > 0f)
            {
                climbing.performHang(ground.IsgroundDetected);
                CurrentState = PlayerStates.WallHang;
            }

            else
            {
                climbing.performSlide(ground.IsgroundDetected, _FacingRight, inputSystem.moveInput);
                CurrentState = PlayerStates.WallSlide;
            }
        }
        else
        {
            if (climbing != null) climbing.ExitState();


            if (!ground.IsgroundDetected)
            {
                if (inputSystem.moveInput.x != 0)
                {
                    SetVelocity(inputSystem.moveInput.x * MoveSpeed, rb.linearVelocity.y);
                }
                CurrentState = PlayerStates.Jump;
            }
            else
            {
                if (pushHandler != null && pushHandler.IsPushing)
                {
                    float x = inputSystem.moveInput.x;

                    if (Mathf.Abs(x) > 0.01f)
                    {
                        SetVelocity(x * MoveSpeed, rb.linearVelocity.y);
                        CurrentState = PlayerStates.PushMove;
                    }
                    else
                    {
                        SetVelocity(0f, rb.linearVelocity.y);
                        CurrentState = PlayerStates.PushIdle;
                    }

                    return;
                }

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
    protected virtual void Intertable()
    {
        if (inputSystem.InteractableInput())
        {
            interact?.Interact();
        }
    }

    private void HandleProximitySanity()
    {
        if (sanity == null || otherPlayer == null)
            return;

        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(playerCheck.position, PlayerDetectRadius, WhatisPlayer);

        bool isOtherPlayerNearby = false;

        foreach (var collider in detectedColliders)
        {
            if (collider.gameObject == otherPlayer.gameObject)
            {
                isOtherPlayerNearby = true;
                break;
            }
        }

        if (isOtherPlayerNearby)
        {
            sanity.RegenerateSanpoint();
        }
        else
        {
            sanity.DrainSanpoint();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerCheck.position, PlayerDetectRadius);
    }

    public virtual void PlayerCrazy()
    {
        GameManager.Instance.ChangeGameState(GameState.GameOver);
    }

    public bool StartWallCrush(Interaction_BreakWall target)
    {

        otherInputLock = Time.time + 0.6f;
        nextWallCrushTime = Time.time + 0.8f;

        SetVelocity(0f, rb.linearVelocity.y);
        CurrentState = PlayerStates.WallCrush;

        if (wallCrushEvent != null) wallCrushEvent.Begin(target);

        return true;
    }
    public void EndWallCrushFromAnim()
    {
        otherInputLock = 0f;
    }
}
