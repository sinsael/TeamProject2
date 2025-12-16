using UnityEngine;

// 제출 전날에 만들라고 시키는게 맞냐
public class Stage2FinshObj : MonoBehaviour
{
    public Interaction_PipeLever lever1; // bool 값 가져오기
    public Interaction_PipeLever lever2; // 동일
    public Interaction_PipeLever lever3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(lever1.On == false && lever2.On == false && lever3.On == false)
        {
            GameManager.Instance.ChangeGameState(GameState.GameClear);
        }
    }
}
