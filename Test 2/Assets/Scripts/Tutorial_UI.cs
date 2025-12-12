using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial_UI : MonoBehaviour
{
    public Button offButton;
    private const string TutorialKey = "IsTutorialViewed";

    IEnumerator Start()
    {
        // 1. 이미 봤다면 꺼버림
        if (PlayerPrefs.GetInt(TutorialKey, 0) == 1)
        {
            gameObject.SetActive(false);
            yield break; // 코루틴 종료
        }

        offButton.onClick.AddListener(CloseTutorial);

        // [핵심 수정] GameManager가 WaitAndFadeIn에서 'GamePlay'로 상태를 바꾸는 것을 기다려야 합니다.
        // GameManager가 1프레임(yield return null)을 쉬므로, 우리는 안전하게 2프레임을 쉽니다.
        yield return null;
        yield return null;

        // 2. 이제 확실하게 게임을 멈춥니다.
        // 이때는 이미 GameManager가 GamePlay로 설정을 마친 뒤라 덮어씌워지지 않습니다.
        GameManager.Instance.ChangeGameState(GameState.GamePause);
    }



    public void CloseTutorial()
    {
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();

        GameManager.Instance.ChangeGameState(GameState.GamePlay);
        gameObject.SetActive(false);
    }
}