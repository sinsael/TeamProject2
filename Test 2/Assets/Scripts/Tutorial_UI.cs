using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 코루틴 사용을 위해 필요

public class Tutorial_UI : MonoBehaviour
{
    public Button offButton;

    // 튜토리얼을 봤는지 저장할 키 값 (이름은 자유롭게 변경 가능)
    private const string TutorialKey = "IsTutorialViewed";

    void Start()
    {
        if (PlayerPrefs.GetInt(TutorialKey, 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        offButton.onClick.AddListener(CloseTutorial);

        StartCoroutine(PauseGameRoutine());
    }

    // 시간을 멈추는 코루틴
    IEnumerator PauseGameRoutine()
    {
        yield return null;
        Time.timeScale = 0f;
    }

    public void CloseTutorial()
    {
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();

        Time.timeScale = 1f;

        gameObject.SetActive(false);
    }
}