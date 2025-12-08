using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial_UI : MonoBehaviour
{
    public Button offButton;
    private const string TutorialKey = "IsTutorialViewed";

    void Start()
    {
        // 1. 이미 봤다면 꺼버림
        if (PlayerPrefs.GetInt(TutorialKey, 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        offButton.onClick.AddListener(CloseTutorial);

        // 2. 바로 멈추지 말고, 페이드 인이 끝날 때까지 기다렸다가 멈춤
        StartCoroutine(PauseGameRoutine());
    }

    IEnumerator PauseGameRoutine()
    {
        // [핵심 수정] 1초 정도 기다립니다 (페이드 인 애니메이션 시간만큼)
        // WaitForSeconds는 Time.timeScale의 영향을 받으므로
        // 시간이 멈추기 전에는 정상적으로 작동합니다.
        yield return new WaitForSeconds(1.0f);

        Time.timeScale = 0f; // 페이드가 끝난 뒤에 시간 정지
    }

    public void CloseTutorial()
    {
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();

        Time.timeScale = 1f; // 시간 다시 흐르게
        gameObject.SetActive(false);
    }
}