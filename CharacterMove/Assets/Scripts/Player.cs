using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;



[Serializable]
public class ObjectDetected
{
    public LayerMask WhatIsObj;
    public float XObjCheckDistance;
    public float YObjCheckDistance;
    public Transform XObjCheck;
    public Transform YObjCheck;
    public Collider2D detected;
    public Collider2D UpdateObjDetected(float xdir, float ydir)
    {
        RaycastHit2D xHit = new RaycastHit2D();
        RaycastHit2D yHit = new RaycastHit2D();

        bool xHitDetected = false;
        bool yHitDetected = false;

        if (xdir != 0)
        {
            xHit = Physics2D.Raycast(XObjCheck.position, Vector2.right * Mathf.Sign(xdir), XObjCheckDistance, WhatIsObj);
            xHitDetected = xHit.collider != null;
        }

        if (ydir != 0)
        {
            yHit = Physics2D.Raycast(YObjCheck.position, Vector2.up * Mathf.Sign(ydir), YObjCheckDistance, WhatIsObj);
            yHitDetected = yHit.collider != null;
        }

        if (xdir == 0)
        {
            xHit = Physics2D.Raycast(XObjCheck.position, Vector2.right, XObjCheckDistance, WhatIsObj);
            xHitDetected = xHit.collider != null;
        }

        if (ydir == 0)
        {
            yHit = Physics2D.Raycast(YObjCheck.position, Vector2.up, YObjCheckDistance, WhatIsObj);
            yHitDetected = yHit.collider != null;
        }

        if (xHitDetected && yHitDetected)
        {
            detected = xHit.distance <= yHit.distance ? xHit.collider : yHit.collider;
        }
        else if (xHitDetected)
        {
            detected = xHit.collider;
        }
        else if (yHitDetected)
        {
            detected = yHit.collider;
        }
        else
        {
            detected = null;
        }

        return detected;
    }


}


public class Player : Entity
{
    public PlayerInputHandler inputSystem { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_XMoveState xMoveState { get; private set; }
    public Player_YMoveState yMoveState { get; private set; }



    [Header("NPC 감지")]
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
        Intertable();
    }

    void Intertable()
    {
        if (inputSystem.InteractableInput())
        {
            Debug.Log(obj.UpdateObjDetected(inputSystem.moveInput.x, inputSystem.moveInput.y));
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

