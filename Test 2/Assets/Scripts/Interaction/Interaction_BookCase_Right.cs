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


    // ... (Start, OnInteract, OnDeselect 등 기존 함수 동일) ...
    public override void Start()
    {
        base.Start();
        GameObject obj = GameObject.Find("Second");
        targetCam = obj.GetComponent<CinemachineVirtualCamera>();
        confiner = obj.GetComponent<CinemachineConfiner2D>();
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
    }

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