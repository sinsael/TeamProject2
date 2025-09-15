using System;
using UnityEngine;

[Serializable]
public class ObjectDetected
{
    public Transform ObjCheck;
    public float ObjCheckRadius;

    public LayerMask WhatIsObj;

    public Collider2D[] ObjColliders;

    public static event Action<Collider2D> OnObjDetected;

    public void UpdateObjDetected()
    {
        ObjColliders = Physics2D.OverlapCircleAll(ObjCheck.position, ObjCheckRadius, WhatIsObj);
        
        foreach(var collider in ObjColliders)
        {
            OnObjDetected?.Invoke(collider);
        }
    }

}


public class Player : Entity
{
    public PlayerInputHandler inputSystem { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }


    [Header("상호작용 오브젝트 감지")]
    public ObjectDetected obj;
    public float jumpForce = 5;

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
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        direction = new Vector2(inputSystem.moveInput.x, 0);

        obj.UpdateObjDetected();

        if (inputSystem.moveInput != Vector2.zero)
        {
            XHandleFlip(inputSystem.moveInput.x);
        }

        Intertable();
    }

    void Intertable()
    {
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(obj.ObjCheck.position, obj.ObjCheckRadius);

    }
}

