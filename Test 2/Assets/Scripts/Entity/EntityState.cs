using UnityEngine;

public abstract class EntityState
{
    
    protected StateMachine stateMachine;
    protected string animBoolName;

   protected Animator anim;
    protected Rigidbody2D rb;

    // 생성자
    public EntityState(StateMachine stateMachin, string animBoolName)
    {
        this.stateMachine = stateMachin;
        this.animBoolName = animBoolName;
    }

    // 상태 진입
    public virtual void Enter()
    {
       anim.SetBool(animBoolName, true);
    }

    // 상태 업데이트
    public virtual void Update()
    {
        anim.SetFloat("YVelocity", rb.velocity.y);
    }

    // 상태 고정 업데이트
    public virtual void FixedUpdate()
    {

    }

    // 상태 종료
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

}
