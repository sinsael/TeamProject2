using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EntityState
{
    
    protected StateMachine stateMachin;
    protected string animBoolName;

    // protected Animator anim;
    protected Rigidbody2D rb;


    public EntityState(StateMachine stateMachin, string animBoolName)
    {
        this.stateMachin = stateMachin;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
       // anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
       // anim.SetBool(animBoolName, false);
    }

}
