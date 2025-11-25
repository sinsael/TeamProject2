using UnityEngine;
using UnityEngine.UI;
public class SanityUIManager : MonoBehaviour
{
    public static SanityUIManager instance;

    [Header("UI References")]
    // Image 대신 Slider로 변경되었습니다.
    public Slider player1SanitySlider;
    public Slider player2SanitySlider;

    [Header("Player References")]
    public Sanity player1Sanity;
    public Sanity player2Sanity;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        UpdateSanitySlider(player1SanitySlider, player1Sanity);

        if (player2Sanity != null)
        {
            UpdateSanitySlider(player2SanitySlider, player2Sanity);
        }
    }

    private void UpdateSanitySlider(Slider slider, Sanity targetSanity)
    {
        if (slider == null || targetSanity == null) return;

        // 1. 최대 정신력과 현재 정신력 가져오기
        float maxSanity = targetSanity.GetComponent<Entity_Stat>().GetSanity();
        float currentSanity = targetSanity.currentSan;

        // 2. 0 ~ 1 사이의 비율로 계산 (슬라이더 Max Value가 1이기 때문)
        // 만약 maxSanity가 0이 되는 버그가 있다면 1로 나눠지게 방어 코드 추가
        float ratio = (maxSanity > 0) ? (currentSanity / maxSanity) : 0;

        // 3. 슬라이더 값 적용 (부드럽게 움직이는 Lerp 적용)
        slider.value = Mathf.Lerp(slider.value, ratio, Time.deltaTime * 5f);

        // 부드러운 게 싫고 즉시 반응하길 원하면 아래 줄을 쓰세요.
        // slider.value = ratio;
    }
}
