using UnityEngine;

public class Stage2FinshObj : MonoBehaviour
{
    public Interaction_PipeLever lever1;
    public Interaction_PipeLever lever2;
    public Interaction_PipeLever lever3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(lever1.On == false && lever2.On == false && lever3.On == false)
        {
            GameManager.Instance.ChangeGameState(GameState.GameClear);
        }
    }
}
