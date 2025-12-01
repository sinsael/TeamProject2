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
    public override void Start()
    {
        base.Start();

        GameObject obj = GameObject.Find("Second");

        targetCam = obj.GetComponent<CinemachineVirtualCamera>();
        confiner = obj.GetComponent<CinemachineConfiner2D>();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        if (targetCam != null)
        {
            targetCam.Follow = FollowTarget.transform;

            // 코루틴 시작 (부드러운 줌)
            StartCoroutine(SmoothZoom(targetZoomSize, zoomDuration));
        }
    }

    IEnumerator SmoothZoom(float targetSize, float duration)
    {
        float startSize = targetCam.m_Lens.OrthographicSize;
        float time = 0;

        while (time < duration)
        {
            // Lerp를 이용해 서서히 값 변경
            targetCam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, time / duration);
            time += Time.deltaTime;
            confiner.InvalidateCache();
            yield return null;
        }

        // 마지막에 정확한 값으로 고정
        targetCam.m_Lens.OrthographicSize = targetSize;
    }
}
