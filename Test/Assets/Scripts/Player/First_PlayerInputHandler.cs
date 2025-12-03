using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First_PlayerInputHandler : PlayerInputHandler
{
    public override void MovementInput()
    {
        input.First_Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.First_Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }
    
    public override bool InteractableInput() => input.First_Player.Interactable.WasPressedThisFrame();
    public override bool JumpInput() => input.First_Player.Jump.WasPressedThisFrame();

    // 1P 의 Climbing 입력 처리
    // Climbing 상태는 Jump 버튼이 눌려져 있는 동안 true, 그렇지 않으면 false
    public override void Update()
    {
        base.Update();
        Climbing = input.First_Player.Jump.IsPressed();
    }
}
