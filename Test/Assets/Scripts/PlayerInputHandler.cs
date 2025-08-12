using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInputSet input { get; private set; }

    public Vector2 moveInput { get; private set; }
    private Vector2 _rawInput; // 입력을 임시로 받을 변수

    private float lastHorizontalPressTime;
    private float lastVerticalPressTime;

    private void Awake()
    {
        input = new PlayerInputSet();

        lastHorizontalPressTime = -1f;
        lastVerticalPressTime = -1f;
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
        float rawY = Mathf.Abs(_rawInput.y) < threshold ? 0 : Mathf.Sign(_rawInput.y);

        if (rawX != 0 && rawY != 0)
        {
            if (lastHorizontalPressTime > lastVerticalPressTime)
                moveInput = new Vector2(rawX, 0);
            else
                moveInput = new Vector2(0, rawY);
        }
        else
        {
            moveInput = new Vector2(rawX, rawY);
        }
    }

    private void MovementInput()
    {
        input.Player.Horizontal.performed += ctx =>
        {
            _rawInput.x = ctx.ReadValue<float>();
            lastHorizontalPressTime = Time.time;
        };
        input.Player.Horizontal.canceled += ctx => _rawInput.x = 0;

        input.Player.Vertical.performed += ctx =>
        {
            _rawInput.y = ctx.ReadValue<float>();
            lastVerticalPressTime = Time.time;
        };
        input.Player.Vertical.canceled += ctx => _rawInput.y = 0;
    }

    public bool InteractableInput() => input.Player.Interactable.WasPressedThisFrame();
}
