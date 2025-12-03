using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Second_PlayerInputHandler : PlayerInputHandler
{
    public override void MovementInput()
    {
        input.Second_Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Second_Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    public override bool InteractableInput() => input.Second_Player.Interactable.WasPressedThisFrame();
    public override bool JumpInput() => input.Second_Player.Jump.WasPressedThisFrame();

    // 2P 의 Climbing 입력 처리
    // Climbing 상태는 Jump 버튼이 눌려져 있는 동안 true, 그렇지 않으면 false
    public override void Update()
    {
        base.Update();
        Climbing = input.Second_Player.Jump.IsPressed();
    }
}
