using System;
using UnityEngine;

[Serializable]
public class ObjectDetected
{
    public float XObjCheckDistance;
    public float YObjCheckDistance;
    public Transform XObjCheck;
    public Transform YObjCheck;

    public LayerMask WhatIsObj;

    public Collider2D UpdateObjDetected(bool xdir, bool Dir)
    {
        RaycastHit2D hit;

            Vector2 Xdir = xdir ? Vector2.right : Vector2.left;
            hit = Physics2D.Raycast(XObjCheck.position, Xdir, XObjCheckDistance, WhatIsObj);


        return hit.collider;
    }

}


public class Player : Entity
{
    public PlayerInputHandler inputSystem {get; private set; }

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

        obj.UpdateObjDetected(facingRight, isFacingVertical);

        direction = new Vector2(inputSystem.moveInput.x, 0);

        if (inputSystem.moveInput != Vector2.zero)
        {
            XHandleFlip(inputSystem.moveInput.x);
        }

        Intertable();
    }

    void Intertable()
    {
        if (inputSystem.InteractableInput())
        {
            Debug.Log(obj.UpdateObjDetected(facingRight, isFacingVertical));
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (isFacingVertical)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(obj.XObjCheck.position, obj.XObjCheck.position + new Vector3(facingRight ? obj.XObjCheckDistance : -obj.XObjCheckDistance, 0));
        }

    }
}

