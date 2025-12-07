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
    public Light2D targetLight; 
    public float colorDisplayTime = 1.0f;
    public float intervalTime = 2.5f;   

    private readonly Color[] originalPalette = { Color.green, Color.white, new Color(0.2f, 0f, 0.4f), Color.blue };

    private List<Color> answerKey = new List<Color>();
    private int playerCurrentIndex = 0; 

    [Header("드랍 아이템")]
    [SerializeField] GameObject dropPostion;
    [SerializeField] GameObject dropItem;
    public override void Start()
    {
        base.Start();
        GameObject obj = GameObject.Find("Second");
        targetCam = obj.GetComponent<CinemachineVirtualCamera>();
        confiner = obj.GetComponent<CinemachineConfiner2D>();

        targetLight.enabled = false;

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
                foreach (var candle in candles)
                {
                    candle.ResetCandle();
                }
        }

        if (targetLight != null)
        {
            targetLight.enabled = false;
        }
    }

    public bool CheckPuzzleAnswer(Color inputColor)
    {
        if (answerKey.Count == 0 || playerCurrentIndex >= answerKey.Count)
            return false;

        if (inputColor == answerKey[playerCurrentIndex])
        {
            playerCurrentIndex++;


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
            CloseBookCase(); 
            return false;
        }
    }

    IEnumerator UniqueSequenceRoutine()
    {
        if (targetLight == null) yield break;

        answerKey.Clear();
        playerCurrentIndex = 0;

        List<int> colorIndices = new List<int>();
        for (int i = 0; i < originalPalette.Length; i++)
        {
            colorIndices.Add(i);
        }
        ShuffleList(colorIndices);

        foreach (int index in colorIndices)
        {
            answerKey.Add(originalPalette[index]);
        }

        isSequence = true;

        yield return new WaitForSeconds(0.5f);

        foreach (int index in colorIndices)
        {
            targetLight.color = originalPalette[index];
            targetLight.enabled = true;
            yield return new WaitForSeconds(colorDisplayTime);

            targetLight.enabled = false;

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