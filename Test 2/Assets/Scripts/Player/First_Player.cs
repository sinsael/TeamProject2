using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First_Player : Player
{
    [Header("Crouch")] //1P 전용 웅크리기 (Crouch)
    public Collider2D standCollider;
    public Collider2D crouchCollider;

    private bool isCrouching = false;

    public PushOBJHandler pushHandler { get; private set; }

    private Interaction interaction;

    protected override void Awake()
    {
        base.Awake();

        if (standCollider != null) standCollider.enabled = true;
        if (crouchCollider != null) crouchCollider.enabled = false;

        pushHandler = GetComponent<PushOBJHandler>();
        interaction = GetComponent<Interaction>();
        pushHandler.Init(this);

    }

    protected override void Start()
    {
        base.Start();
        Second_Player p2 = FindAnyObjectByType<Second_Player>();
        if (p2 != null)
        {
            otherPlayer = p2.GetComponent<Second_Player>();
        }
    }

    protected override void Update()
    {
        isCrouchingMovementInputBlock();
        pushHandler.PushingotherMoveBlock();

        base.Update();

        pushHandler.PushingSystem();
        Crouch();
    }

    private void Crouch()
    {
        if (pushHandler != null && pushHandler.IsPushing)
        {
            if (isCrouching) StopCrouch();
            return;
        }

        if (wall.IswallDetected && !ground.IsgroundDetected)
        {

        }
        else if (!ground.IsgroundDetected && !wall.IswallDetected)
        {

        }
        else if (!wall.IswallDetected && ground.IsgroundDetected)
        {
            if (inputSystem.CrouchInput() && !isCrouching)
            {
                StartCrouch();
                CurrentState = PlayerStates.Crouch;
            }
            else if (!inputSystem.CrouchInput() && isCrouching)
            {
                StopCrouch();
            }
        }
    }
    private void StartCrouch()
    {
        isCrouching = true;

        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        if (standCollider != null) standCollider.enabled = false;
        if (crouchCollider != null) crouchCollider.enabled = true;
    }

    private void StopCrouch()
    {
        isCrouching = false;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (standCollider != null) standCollider.enabled = true;
        if (crouchCollider != null) crouchCollider.enabled = false;
    }

    private void isCrouchingMovementInputBlock()
    {
        Vector2 input = inputSystem.moveInput;

        if (isCrouching)
        {
            input.x = 0f;
            inputSystem.moveInput = input;
            return;
        }
    }
}
