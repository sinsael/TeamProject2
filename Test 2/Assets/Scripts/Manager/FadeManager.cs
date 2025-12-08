using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("설정")]
    [Tooltip("페이드 되는 데 걸리는 시간(초)")]
    public float fadeDuration = 1.0f;

    [Header("참조")]
    public Image fadeImage;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fadeImage.gameObject.SetActive(false);
    }

    // 페이드 인: 화면이 밝아짐 (검은색 -> 투명)
    public void FadeIn(Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(1, 0, onComplete));
    }

    // 페이드 아웃: 화면이 어두워짐 (투명 -> 검은색)
    public void FadeOut(Action onComplete = null)
    {
        StartCoroutine(FadeRoutine(0, 1, onComplete));
    }

    [ContextMenu("Test Fade In")]
    private void TestFadeIn()
    {
        FadeIn(); // 파라미터 없이 호출
    }

    [ContextMenu("Test Fade Out")]
    private void TestFadeOut()
    {
        FadeOut(); // 파라미터 없이 호출
    }

    // 실제 페이드 로직을 처리하는 코루틴
    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, Action onComplete)
    {
        fadeImage.gameObject.SetActive(true);

        float elapsedTime = 0f;

        // 페이드 중에는 클릭 입력을 막음
        fadeImage.raycastTarget = true;

        // 현재 이미지의 색상을 가져옴 (Color 구조체)
        Color currentColor = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // 알파값 계산
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            // 색상의 알파(a) 값만 변경하여 다시 할당
            currentColor.a = newAlpha;
            fadeImage.color = currentColor;

            yield return null;
        }

        // 확실하게 최종 값으로 설정
        currentColor.a = endAlpha;
        fadeImage.color = currentColor;

        // 페이드 인이 끝났을 때만 클릭 입력을 다시 허용
        if (endAlpha == 0)
        {
            fadeImage.raycastTarget = false;
            fadeImage.gameObject.SetActive(false);
        }

        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }
}
