using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState
{
    GameStart,
    GamePause,
    GamePlay,
    GameOver,
    GameClear
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool isfirsteLoad = true;
    public GameState currentGameState { get; private set; }
    public GameObject GameStartUI;
    public GameObject GamePauseUI;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GamePlay)
            {
                ChangeGameState(GameState.GamePause);
            }
            else if (currentGameState == GameState.GamePause)
            {
                ChangeGameState(GameState.GamePlay);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeGameState(GameState.GameClear);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitAndFadeIn());
    }

    IEnumerator WaitAndFadeIn()
    {
        // 한 프레임(약 0.016초)을 쉽니다. 이 동안 FadeManager가 확실히 로딩됩니다.
        yield return null;

        // 안전 장치: 기다렸는데도 없으면 에러 로그 출력
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn();
            ChangeGameState(GameState.GamePlay);
        }
        else
        {
            Debug.LogError("FadeManager가 여전히 없습니다! DontDestroyOnLoad 설정을 확인해주세요.");
            // UI라도 끄기 위해 강제 변경
            ChangeGameState(GameState.GamePlay);
        }
    }

    public void OnPlayerDead()
    {
        // 상태는 변경하되 (입력 막기 용도), UI는 띄우지 않음
        ChangeGameState(GameState.GameOver);
    }


    public void ChangeGameState(GameState newState)
    {
        currentGameState = newState;

        CloseAllUI();

        // 2. 상태에 따라 필요한 처리(UI 켜기, 시간 정지 등)를 '한 번만' 수행합니다.
        switch (currentGameState)
        {
            case GameState.GameStart:
                if (GameStartUI != null) GameStartUI.SetActive(true);

                Time.timeScale = 0f; // 시간 정상화
                break;

            case GameState.GamePause:
                if (GamePauseUI != null) GamePauseUI.SetActive(true);
                Time.timeScale = 0f; // 게임 일시 정지
                break;

            case GameState.GamePlay:
                Time.timeScale = 1f; // 시간 정상화
                break;

            case GameState.GameOver:
                Time.timeScale = 1f;
                FadeManager.Instance.FadeOut(() =>
                {
                    isfirsteLoad = false;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                break;

            case GameState.GameClear:
                Time.timeScale = 1f;
                FadeManager.Instance.FadeOut(() =>
                {
                    int currentIndex = SceneManager.GetActiveScene().buildIndex;
                    int nextIndex = currentIndex + 1;
                    SceneManager.LoadScene(nextIndex);
                });
                break;

        }
    }

    private void CloseAllUI()
    {
        if (GameStartUI != null) GameStartUI.SetActive(false);
        if (GamePauseUI != null) GamePauseUI.SetActive(false);
    }
}
