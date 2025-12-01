using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 로드 완료 시 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindSanityComponents();
    }

    public void FindSanityComponents()
    {
        // 1P 찾기 (First_Player 클래스를 가진 오브젝트 검색)
        First_Player p1 = FindAnyObjectByType<First_Player>();
        if (p1 != null)
        {
            player1Sanity = p1.GetComponent<Sanity>();
        }

        // 2P 찾기 (Second_Player 클래스를 가진 오브젝트 검색)
        Second_Player p2 = FindAnyObjectByType<Second_Player>();
        if (p2 != null)
        {
            player2Sanity = p2.GetComponent<Sanity>();
        }
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
    }
}
