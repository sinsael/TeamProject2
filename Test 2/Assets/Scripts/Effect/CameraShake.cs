using UnityEngine;
using Cinemachine;
using System.Security.Cryptography.X509Certificates;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance { get; private set; }

    private static readonly System.Collections.Generic.List<CameraShake> all = new(); 
    private static readonly System.Collections.Generic.Dictionary<int, CameraShake> byID = new();

    private CinemachineVirtualCamera Vcam;
    private CinemachineBasicMultiChannelPerlin perlin;

    private int CameraID = 0;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntencity;

    private void Awake()
    {
        instance = this;
        Vcam = GetComponent<CinemachineVirtualCamera>();
        perlin = Vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        all.Add(this);
        byID[CameraID] = this;

        all.Add(this);
        byID[CameraID] = this;
    }

    public static void ShakeAll(float intensity, float time)
    {
        for (int i = 0; i < all.Count; i++)
            all[i].ShakeLocal(intensity, time);
    }

    public static void ShakeOne(int id, float intensity, float time)
    {
        if (byID.ContainsKey(id))
            byID[id].ShakeLocal(intensity, time);
    }
    public void ShakeCamera(float intencity, float time)
    {
        ShakeLocal(intencity, time);
    }

    public void ShakeLocal(float intencity, float time)
    {
        perlin.m_AmplitudeGain = intencity;

        startingIntencity = intencity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer <= 0f)
            return;

        shakeTimer -= Time.deltaTime;

        float t = 1f - (shakeTimer / shakeTimerTotal);
        perlin.m_AmplitudeGain = Mathf.Lerp(startingIntencity, 0f, t);

        if (shakeTimer <= 0f)
            perlin.m_AmplitudeGain = 0f;
    }

}