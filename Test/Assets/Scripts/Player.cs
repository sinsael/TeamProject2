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

    public Collider2D UpdateObjDetected(bool xdir, bool ydir, bool Dir)
    {
        RaycastHit2D hit;

        if (Dir)
        {
            Vector2 Xdir = xdir ? Vector2.up : Vector2.down;

            
            hit = Physics2D.Raycast(YObjCheck.position, Xdir, YObjCheckDistance, WhatIsObj);
        }
        else
        {
            Vector2 Ydir = ydir ? Vector2.left : Vector2.right;
            hit = Physics2D.Raycast(XObjCheck.position, Ydir, XObjCheckDistance, WhatIsObj);
        }

        return hit.collider;
    }

}


public class Player : Entity
{
    public PlayerInputHandler inputSystem { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_YMoveState yMoveState { get; private set; }


    [Header("상호작용 오브젝트 감지")]
    public ObjectDetected obj;

    protected override void Awake()
    {
        base.Awake();

        inputSystem = GetComponent<PlayerInputHandler>();

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


        if (inputSystem.moveInput != Vector2.zero)
        {
            GizmosDirection();
            yFlip(inputSystem.moveInput.y, inputSystem.moveInput.y);
            XHandleFlip(inputSystem.moveInput.x);
            return;
        }
        else
        {
            Intertable();
        }
    }

    void Intertable()
    {
        if (inputSystem.InteractableInput())
        {
            Debug.Log(obj.UpdateObjDetected(facingRight,facingUp,isFacingVertical));
        }
    }

    protected override float XGizmoDirection
    {
        get
        {
            if (inputSystem == null || inputSystem.moveInput == null)
                return 0f;

            return inputSystem.moveInput.x != 0 ? Mathf.Sign(inputSystem.moveInput.x) : 0f;
        }
    }

    protected override float YGizmoDirection
    {
        get
        {
            if (inputSystem == null || inputSystem.moveInput == null)
                return 0f;

            return inputSystem.moveInput.y != 0 ? Mathf.Sign(inputSystem.moveInput.y) : 0f;
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (isFacingVertical)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(obj.YObjCheck.position, obj.YObjCheck.position + new Vector3(0, facingUp ? obj.YObjCheckDistance : -obj.YObjCheckDistance));
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(obj.XObjCheck.position, obj.XObjCheck.position + new Vector3(facingRight ? obj.XObjCheckDistance : -obj.XObjCheckDistance, 0));
        }

    }

    public override void MoveBy(float x, float y)
    {
        if (Wall.wallDetected) return;

        base.MoveBy(x, y);
    }
}

