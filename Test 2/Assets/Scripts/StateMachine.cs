//��� �ִϸ��̼� ���ʰ� �Ǵ� ��ũ��Ʈ
public class StateMachine
{
    public EntityState currentState { get; private set; } // ���� ����
    public bool canChangeState = true; // ���� ���� ���� ����

    // ���� �ʱ�ȭ
    public void Initialize(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.Enter();
    }

    // ���� ����
    public void ChangeState(EntityState newState)
    {
        if (canChangeState == false)
            return;

        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    // ���� ���� ������Ʈ
    public void UpdateActiveState()
    {
        currentState.Update();
    }

    // ���� ���� ���� ������Ʈ
    public void FixedUpdateActiveState()
    {
        currentState.FixedUpdate();
    }
}
