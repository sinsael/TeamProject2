using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInputSet input { get; private set; }

    public Vector2 moveInput { get; private set; }



    private void Awake()
    {
        input = new PlayerInputSet();

    }
    private void Update()
    {
    }

    private void OnEnable()
    {
        input.Enable();
        MovementInput();
    }
    private void OnDisable()
    {
        input.Disable();
    }


    private void MovementInput()
    {
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    public bool InteractableInput() => input.Player.Interactable.WasPressedThisFrame();
    public bool JumpInput() => input.Player.Jump.WasPressedThisFrame();
}
