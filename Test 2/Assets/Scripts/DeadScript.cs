using UnityEngine;

public class DeadScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.ChangeGameState(GameState.GameOver);
        }
    }
}
