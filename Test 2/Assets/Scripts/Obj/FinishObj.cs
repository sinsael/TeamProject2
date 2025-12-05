using UnityEngine;

public class FinishObj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameManager.Instance.ChangeGameState(GameState.GameClear);
        }
    }
}
