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
    bool isActive;
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

        if(originalTarget == null) return;
        targetCam.Follow = originalTarget;
        StopAllCoroutines();
        StartCoroutine(SmoothZoom(originalSize, 0));
        isActive = false;
        confiner.InvalidateCache();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        if (targetCam != null && isActive == false)
        {
            originalTarget = targetCam.Follow;
            originalSize = targetCam.m_Lens.OrthographicSize;

            targetCam.Follow = FollowTarget.transform;

            // ???? ???? (?ех??? ??)
            StartCoroutine(SmoothZoom(targetZoomSize, zoomDuration));
            isActive = true;
        }
        else if (isActive == true)
        {
            if (originalTarget == null) return;
            targetCam.Follow = originalTarget;
            StopAllCoroutines();
            StartCoroutine(SmoothZoom(originalSize, 0));
            isActive = false;
            confiner.InvalidateCache();
        }
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
}
