using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class First_PlayerInputHandler : PlayerInputHandler
{
    public override void Awake()
    {
        input = new PlayerInputSet();
    }

    public override void OnEnable()
    {
        input.First_Player.Enable();
    }

    public override void OnDisable()
    {
        input.First_Player.Disable();
    }   
    public override void MovementInput()
    {
        input.Second_Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Second_Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    public override bool InteractableInput() => input.First_Player.Interactable.WasPressedThisFrame();
    public override bool JumpInput() => input.First_Player.Up.WasPressedThisFrame();
    public override bool Climbinginput() => input.First_Player.Up.IsPressed();
}
