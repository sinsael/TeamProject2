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
}
