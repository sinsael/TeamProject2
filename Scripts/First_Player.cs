using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First_Player : Player
{
    [Header("Crouch")] //1P 전용 웅크리기 (Crouch)
    public Collider2D standCollider;
    public Collider2D crouchCollider;

    private bool isCrouching = false;

    protected override void Awake()
    {
        base.Awake();

        if (standCollider != null) standCollider.enabled = true;
        if (crouchCollider != null) crouchCollider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

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
}
