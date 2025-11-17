using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Second_PlayerInputHandler : PlayerInputHandler
{
    public override void Awake()
    {
        input = new PlayerInputSet();
    }

    public override void OnEnable()
    {
        input.Second_Player.Enable();
    }
    public override void OnDisable()
    {
        input.Second_Player.Disable();
    }
    public override void MovementInput()
    {
        input.Second_Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Second_Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    public override bool InteractableInput() => input.Second_Player.Interactable.WasPressedThisFrame();
    public override bool JumpInput() => input.Second_Player.Up.WasPressedThisFrame();
    public override bool Climbinginput() => input.Second_Player.Up.IsPressed();
}
