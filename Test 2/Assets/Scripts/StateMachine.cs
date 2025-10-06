//모든 애니메이션 기초가 되는 스크립트
public class StateMachine
{
    public EntityState currentState { get; private set; } // 현쟁 상태
    public bool canChangeState = true; // 상태 변경 가능 여부

    // 상태 초기화
    public void Initialize(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }

    // 상태 변경
    public void ChangeState(EntityState newState)
    {
        if (canChangeState == false)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // 현재 상태 업데이트
    public void UpdateActiveState()
    {
        currentState.Update();
    }

    // 현재 상태 고정 업데이트
    public void FixedUpdateActiveState()
    {
        currentState.FixedUpdate();
    }
}
