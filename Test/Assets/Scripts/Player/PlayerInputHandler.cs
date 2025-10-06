using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInputSet input { get; private set; }

    public virtual Vector2 moveInput { get; set; }

    //------------추가-------------
    public bool Climbing { get; protected set; }   // 각 플레이어들의 점프키를 누르고 있는 동안 true/false
    //----------------------------

    public virtual void Awake()
    {
        input = new PlayerInputSet();
    }

    public virtual void Update()
    {
        // 업데이트에서 매 프레임마다 Climbing 상태 갱신
        // 그대로 두기!!
    }

    public virtual void OnEnable()
    {
        input.Enable();
        MovementInput();
    }

    public virtual void OnDisable()
    {
        input.Disable();
    }

    public virtual void MovementInput()
    {

    }

    public virtual bool InteractableInput()
    {
        return false;
    }

    public virtual bool JumpInput()
    {
        return false;
    }
}
