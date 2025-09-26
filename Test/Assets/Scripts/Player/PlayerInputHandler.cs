using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInputSet input { get; private set; }

    public virtual Vector2 moveInput {get; set;}


    public virtual void Awake()
    {
        input = new PlayerInputSet();

    }
    public virtual void Update()
    {
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
