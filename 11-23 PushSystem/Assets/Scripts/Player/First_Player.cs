using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class First_Player : Player
{
    [Header("Crouch")]
    public Collider2D standCollider;
    public Collider2D crouchCollider;

    private bool isCrouching = false;

    public PushOBJHandler pushHandler { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        if (standCollider != null) standCollider.enabled = true;
        if (crouchCollider != null) crouchCollider.enabled = false;

        pushHandler = GetComponent<PushOBJHandler>();
        pushHandler.Init(this);
    }

    protected override void Update()
    {
        isCrouchingMovementInputBlock();
        pushHandler.PushingotherMoveBlock();

        base.Update();

        pushHandler.PushingSystem();
        Crouch();
    }

    // ==========================
    //    웅크리기 처리
    // ==========================
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

        if (standCollider != null) standCollider.enabled = false;
        if (crouchCollider != null) crouchCollider.enabled = true;
    }

    private void StopCrouch()
    {
        isCrouching = false;

        if (standCollider != null) standCollider.enabled = true;
        if (crouchCollider != null) crouchCollider.enabled = false;
    }

    // 웅크리면 이동 불가
    private void isCrouchingMovementInputBlock()
    {
        Vector2 input = inputSystem.moveInput;

        if (isCrouching)
        {
            input.x = 0f;
            input.y = 0f;
            inputSystem.moveInput = input;
            return;
        }
    }
}
