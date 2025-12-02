using Cinemachine;
using UnityEngine;
using System.Collections;

public class Interaction_BookCase_Right : Interaction_Obj
{
    private CinemachineVirtualCamera targetCam;
    private CinemachineConfiner2D confiner;
    public GameObject FollowTarget;
    public float targetZoomSize = 3f;
    public float zoomDuration = 3f;

    Transform originalTarget;
    float originalSize;
    public bool isActive;

    [Header("Puzzle Settings")]
    public SpriteRenderer[] bookRenderers; // 색이 바뀔 책 4개 (에디터에서 할당)
    public Color bookOnColor = Color.cyan; // 불 들어왔을 때 책 색깔
    public Color bookOffColor = Color.white; // 기본 책 색깔

    // 정답 패턴 (책이 켜져있는지 여부)
    private bool[] targetPattern;
    // 현재 양초 상태 (양초가 켜져있는지 여부)
    private bool[] currentCandleState;

    public bool isPuzzleSolved = false; // 퍼즐 클리어 여부

    public Interaction_Candle_Stage2[] candles;

    public override void Start()
    {
        base.Start();

        GameObject obj = GameObject.Find("Second");

        targetCam = obj.GetComponent<CinemachineVirtualCamera>();
        confiner = obj.GetComponent<CinemachineConfiner2D>();

        targetPattern = new bool[4];
        currentCandleState = new bool[4];
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        CloseBookCase();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        if (targetCam != null && isActive == false)
        {
            originalTarget = targetCam.Follow;
            originalSize = targetCam.m_Lens.OrthographicSize;
            targetCam.Follow = FollowTarget.transform;

            StopAllCoroutines();
            StartCoroutine(SmoothZoom(targetZoomSize, zoomDuration));

            isActive = true;

            if (!isPuzzleSolved)
            {
                StartPuzzle();
            }
        }
        else if (isActive == true)
        {
            CloseBookCase();
        }
    }

    void CloseBookCase()
    {
        if (!isActive) return;
        if (originalTarget != null) targetCam.Follow = originalTarget;
        StopAllCoroutines();
        StartCoroutine(SmoothZoom(originalSize, 0));
        isActive = false;
        if (confiner != null) confiner.InvalidateCache();

        ResetPuzzleState(); // 나가면 초기화
    }


    IEnumerator SmoothZoom(float targetSize, float duration)
    {
        float startSize = targetCam.m_Lens.OrthographicSize;
        float time = 0;

        while (time < duration)
        {
            // Lerp?? ????? ?????? ?? ????
            targetCam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            confiner.InvalidateCache();
            yield return null;
        }

        // ???????? ????? ?????? ????
        targetCam.m_Lens.OrthographicSize = targetSize;
    }

    void StartPuzzle()
    {
        // 1. 기존 상태 초기화
        ResetPuzzleState();

        // 2. 새로운 랜덤 패턴 생성
        GenerateRandomPattern();

        // 3. 2.5초 간격으로 불 켜기 시작
        StartCoroutine(ShowPatternRoutine());
    }

    void GenerateRandomPattern()
    {
        for (int i = 0; i < 4; i++)
        {
            // 50% 확률로 정답 설정
            targetPattern[i] = Random.value > 0.5f;
        }

        // (예외 처리) 만약 랜덤인데 하나도 안 켜지면 재미없으니 하나는 강제로 켬
        if (!targetPattern[0] && !targetPattern[1] && !targetPattern[2] && !targetPattern[3])
        {
            targetPattern[Random.Range(0, 4)] = true;
        }
    }

    IEnumerator ShowPatternRoutine()
    {
        // 보여주기 전 잠깐 대기 (줌인 시간 고려)
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < 4; i++)
        {
            // 정답인 책만 불을 켬
            if (targetPattern[i])
            {
                if (bookRenderers[i] != null)
                    bookRenderers[i].color = bookOnColor;

                // 불이 켜지고 2.5초 대기 (다음 불 켜질 때까지)
                yield return new WaitForSeconds(2.5f);
            }
        }
    }
    public bool TryInteractCandle(int index)
    {
        if (isPuzzleSolved) return false;

        // [오답 체크] 켜져서는 안 되는(책이 꺼진) 위치의 양초를 건드렸는가?
        if (targetPattern[index] == false)
        {
            Debug.Log("틀렸습니다! 퍼즐이 초기화됩니다.");
            // 퍼즐 리셋 및 재시작
            StartPuzzle();
            return false; // 양초에게 "켜지지 마"라고 신호 줌
        }

        // [정답 체크] 맞는 양초를 건드림
        currentCandleState[index] = !currentCandleState[index]; // 켜짐/꺼짐 토글 (원하면 켜기만 가능하게 수정 가능)

        CheckWinCondition();
        return true; // 양초에게 "상태 바꿔도 돼"라고 신호 줌
    }

    void CheckWinCondition()
    {
        for (int i = 0; i < 4; i++)
        {
            // 정답 패턴과 현재 양초 상태가 다르면 아직 성공 아님
            // (꺼져야 할 곳은 위에서 이미 걸러졌으니, 켜져야 할 곳이 다 켜졌는지 확인하는 셈)
            if (targetPattern[i] != currentCandleState[i])
                return;
        }

        Debug.Log("퍼즐 성공!");
        isPuzzleSolved = true;
        // 보상 지급 로직...
    }

    void ResetPuzzleState()
    {
        StopAllCoroutines(); // 진행 중이던 패턴 보여주기 중단

        // 데이터 초기화
        for (int i = 0; i < 4; i++) currentCandleState[i] = false;

        // 책(문제) 시각 효과 끄기
        for (int i = 0; i < bookRenderers.Length; i++)
        {
            if (bookRenderers[i] != null) bookRenderers[i].color = bookOffColor;
        }

        // 양초(입력) 시각 효과 끄기 (양초 스크립트 제어)
        if (candles != null)
        {
            foreach (var candle in candles)
            {
                if (candle != null) candle.ForceTurnOff();
            }
        }
    }

    // 2. 책 시각효과 리셋
    void ResetPuzzleVisuals()
    {
        for (int i = 0; i < bookRenderers.Length; i++)
        {
            if (bookRenderers[i] != null)
                bookRenderers[i].color = bookOffColor;
        }
    }

    // 3. 양초가 켜지거나 꺼질 때 호출되는 함수 (양초 스크립트에서 호출)
    public void UpdateCandleState(int candleIndex, bool isOn)
    {
        if (isPuzzleSolved) return;

        currentCandleState[candleIndex] = isOn;
        CheckSolution();
    }

    // 4. 정답 확인
    void CheckSolution()
    {
        for (int i = 0; i < 4; i++)
        {
            if (targetPattern[i] != currentCandleState[i])
                return;
        }

        isPuzzleSolved = true;

    }
}
