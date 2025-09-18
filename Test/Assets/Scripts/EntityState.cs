using UnityEngine;

public abstract class EntityState
{
    
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;


    public EntityState(StateMachine stateMachin, string animBoolName)
    {
        this.stateMachine = stateMachin;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
       anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        anim.SetFloat("YVelocity", rb.velocity.y);
    }
    
    public virtual void FixedUpdate()
    {

    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

}
