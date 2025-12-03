using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_BookCase_Right : Interaction_Obj
{
    // ... (변수 선언부는 기존과 동일) ...
    private CinemachineVirtualCamera targetCam;
    private CinemachineConfiner2D confiner;
    public GameObject FollowTarget;
    public float targetZoomSize = 3f;
    public float zoomDuration = 3f;

    Transform originalTarget;
    float originalSize;

    public bool isActive;
    public bool isSequence = false;
    public bool isPuzzleSolved = false;

    public Interaction_Candle_Stage2[] candles = null;

    [Header("Color Interaction Settings")]
    public SpriteRenderer[] fourObjects; // 색이 바뀔 4개의 오브젝트
    public float colorDisplayTime = 1.0f; // 색이 켜져 있는 시간 (예: 1초)
    public float intervalTime = 2.5f;     // 꺼진 후 다음 색이 켜질 때까지 대기 시간

    private readonly Color[] originalPalette = { Color.green, Color.white, Color.black, Color.blue };

    private List<Color> answerKey = new List<Color>(); // 정답 순서 저장
    private int playerCurrentIndex = 0; // 플레이어가 몇 번째 정답을 맞추고 있는지

    public override void Start()
    {
        base.Start();
        GameObject obj = GameObject.Find("Second");
        targetCam = obj.GetComponent<CinemachineVirtualCamera>();
        confiner = obj.GetComponent<CinemachineConfiner2D>();

        originalTarget = targetCam.Follow;
        originalSize = targetCam.m_Lens.OrthographicSize;
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        CloseBookCase();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        if(isPuzzleSolved)
        {
            return;
        }

        if (targetCam != null && isActive == false)
        {
            targetCam.Follow = FollowTarget.transform;

            StopAllCoroutines();
            StartCoroutine(SmoothZoom(targetZoomSize, zoomDuration));

            isSequence = false;
            if (!isPuzzleSolved)
            {
                StartCoroutine(UniqueSequenceRoutine());
            }

            isActive = true;
        }
        else if (isActive == true)
        {
            CloseBookCase();
        }
    }

    //책장 상호작용 취소
    void CloseBookCase()
    {
        targetCam.Follow = originalTarget;
        StopAllCoroutines();
        StartCoroutine(SmoothZoom(originalSize, 0));

        isActive = false;
        isSequence = false;

        confiner.InvalidateCache();

        if (!isPuzzleSolved)
        {
            if (candles == null) Debug.LogWarning("연결안됨");
            else
            {
                foreach (var candle in candles)
                {
                    candle.ResetCandle();
                }
            }
        }

        if (fourObjects == null) Debug.LogWarning("네개의 오브젝트 연결 안됨");
        else
        {
            foreach (var renderer in fourObjects)
            {
                renderer.color = Color.white;
            }
        }
    }

    public bool CheckPuzzleAnswer(Color inputColor)
    {
        if (answerKey.Count == 0 || playerCurrentIndex >= answerKey.Count)
            return false;

        if (inputColor == answerKey[playerCurrentIndex])
        {
            // 정답!
            playerCurrentIndex++; // 다음 단계로 이동


            if (playerCurrentIndex >= answerKey.Count)
            {
                Debug.Log("퍼즐 성공! 모든 촛불이 켜졌습니다.");

                isPuzzleSolved = true;

                // 아이템 드랍
                StopCoroutine("UniqueSequenceRoutine");

                CloseBookCase();
            }

            return true;
        }
        else
        {
            // 오답!
            Debug.Log($"틀렸습니다! 정답: {answerKey[playerCurrentIndex]}, 입력: {inputColor}");
            CloseBookCase(); // 상호작용 강제 종료 및 초기화
            return false;
        }
    }

    IEnumerator UniqueSequenceRoutine()
    {
        if (fourObjects == null || fourObjects.Length == 0) yield break;

        answerKey.Clear();
        playerCurrentIndex = 0;

        List<int> objIndices = new List<int>();
        List<int> colorIndices = new List<int>();


        for (int i = 0; i < fourObjects.Length; i++)
        {
            objIndices.Add(i);
            if (i < originalPalette.Length) colorIndices.Add(i);
        }

        ShuffleList(objIndices);
        ShuffleList(colorIndices);

        int loopCount = Mathf.Min(objIndices.Count, colorIndices.Count);
        for (int i = 0; i < loopCount; i++)
        {
            answerKey.Add(originalPalette[colorIndices[i]]);
        }

        isSequence = true;

        for (int i = 0; i < loopCount; i++)
        {
            SpriteRenderer currentObj = fourObjects[objIndices[i]];
            Color currentColor = originalPalette[colorIndices[i]];

            if (currentObj != null) currentObj.color = currentColor;
            yield return new WaitForSeconds(colorDisplayTime);

            if (currentObj != null) currentObj.color = Color.white;
            yield return new WaitForSeconds(intervalTime);
        }
    }
    void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 카메라 움직임 조정
    /// </summary>
    /// <param name="targetSize">카메라 사이즈 조정</param>
    /// <param name="duration">걸리는 시간</param>
    /// <returns></returns>
    IEnumerator SmoothZoom(float targetSize, float duration)
    {
        float startSize = targetCam.m_Lens.OrthographicSize;
        float time = 0;

        while (time < duration)
        {
            targetCam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            confiner.InvalidateCache();
            yield return null;
        }
        targetCam.m_Lens.OrthographicSize = targetSize;
    }
}