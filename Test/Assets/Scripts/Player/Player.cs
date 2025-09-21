using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public PlayerInputHandler inputSystem { get; private set; }
    protected IInteraction interaction;

    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }

    [Header("플레이어 움직임 설정")]
    public float MoveSpeed = 5;
    public float jumpForce = 5;

    [Header("상호작용 오브젝트 감지")]
    public Interaction interact;




    protected override void Awake()
    {
        base.Awake();

        inputSystem = GetComponent<PlayerInputHandler>();

        idleState = new Player_IdleState(this, stateMachine, "Idle");
        xMoveState = new Player_XMoveState(this, stateMachine, "XMove");
        jumpState = new Player_JumpState(this, stateMachine, "JumpFall");
        fallState = new Player_FallState(this, stateMachine, "JumpFall");

    }
    protected override void Start()
    {
        base.Start();

        interaction = GetComponent<IInteraction>();
        
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        direction = new Vector2(inputSystem.moveInput.x, 0);

        interact.FindBestTarget();
        interact.HandleTargetChange();
        interact.UpdateObjDetected();
        Intertable();

    }

    private void Intertable()
    {
        if (inputSystem.InteractableInput())
        {
            interact.Interact();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interact.ObjCheck.position, interact.ObjCheckRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interact.interactionCheck.position, interact.interactionRadius);

    }
}

