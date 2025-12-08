using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    public bool isSequence = false;
    public bool isPuzzleSolved = false;

    public Interaction_Candle_Stage2[] candles = null;

    [Header("오브젝트 상호작용색")]
    public Light2D targetLight; // 변경: 배열이 아니라 단일 Light2D
    public float colorDisplayTime = 1.0f; // 색이 켜져 있는 시간 (예: 1초)
    public float intervalTime = 2.5f;     // 꺼진 후 다음 색이 켜질 때까지 대기 시간

    private readonly Color[] originalPalette = { Color.green, Color.white, Color.black, Color.blue };

    private List<Color> answerKey = new List<Color>(); // 정답 순서 저장
    private int playerCurrentIndex = 0; // 플레이어가 몇 번째 정답을 맞추고 있는지

    [Header("드랍 아이템")]
    [SerializeField] GameObject dropPostion;
    [SerializeField] GameObject dropItem;
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

        // 조명 색상 초기화 (기본 흰색으로 복귀)
        if (targetLight != null)
        {
            targetLight.color = Color.white;
        }
    }

    public bool CheckPuzzleAnswer(Color inputColor)
    {
        if (answerKey.Count == 0 || playerCurrentIndex >= answerKey.Count)
            return false;

        if (inputColor == answerKey[playerCurrentIndex])
        {
            playerCurrentIndex++; // 다음 단계로 이동


            if (playerCurrentIndex >= answerKey.Count)
            {
                Debug.Log("퍼즐 성공! 모든 촛불이 켜졌습니다.");

                isPuzzleSolved = true;

                // 아이템 드랍
                Instantiate(dropItem, dropPostion.transform.position, Quaternion.identity);

                StopCoroutine("UniqueSequenceRoutine");

                CloseBookCase();
            }

            return true;
        }
        else
        {
            Debug.Log($"틀렸습니다! 정답: {answerKey[playerCurrentIndex]}, 입력: {inputColor}");
            CloseBookCase(); // 상호작용 강제 종료 및 초기화
            return false;
        }
    }

    IEnumerator UniqueSequenceRoutine()
    {
        if (targetLight == null) yield break;

        answerKey.Clear();
        playerCurrentIndex = 0;

        // 1. 색상 인덱스(0,1,2,3)를 리스트에 담고 섞습니다.
        List<int> colorIndices = new List<int>();
        for (int i = 0; i < originalPalette.Length; i++)
        {
            colorIndices.Add(i);
        }
        ShuffleList(colorIndices);

        // 2. 섞인 순서대로 정답 키를 생성합니다.
        foreach (int index in colorIndices)
        {
            answerKey.Add(originalPalette[index]);
        }

        isSequence = true;

        // 3. 하나의 조명(targetLight)에서 색을 순차적으로 보여줍니다.
        foreach (int index in colorIndices)
        {
            // 색상 변경
            targetLight.enabled = true;
            targetLight.color = originalPalette[index];

            // 지정된 시간만큼 보여줌
            yield return new WaitForSeconds(colorDisplayTime);

            targetLight.enabled = false;

            // 다음 색이 나오기 전 대기
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