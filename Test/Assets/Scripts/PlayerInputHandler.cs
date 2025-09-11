using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInputSet input { get; private set; }

    public Vector2 moveInput { get; private set; }
    private Vector2 _rawInput; // �Է��� �ӽ÷� ���� ����

    private float lastHorizontalPressTime;


    private void Awake()
    {
        input = new PlayerInputSet();

        lastHorizontalPressTime = -1f;
    }
    private void Update()
    {
        ProcessInputPriority();
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

    public void ProcessInputPriority()
    {
        float threshold = 0.1f;
        float rawX = Mathf.Abs(_rawInput.x) < threshold ? 0 : Mathf.Sign(_rawInput.x);

        moveInput = new Vector2(rawX, 0);

    }

    private void MovementInput()
    {
        input.Player.Horizontal.performed += ctx =>
        {
            _rawInput.x = ctx.ReadValue<float>();
            lastHorizontalPressTime = Time.time;
        };
        input.Player.Horizontal.canceled += ctx => _rawInput.x = 0;
    }

    public bool InteractableInput() => input.Player.Interactable.WasPressedThisFrame();
}
