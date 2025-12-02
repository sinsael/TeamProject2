using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineShakeNoise : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;

    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine routine;

    private void Awake()
    {
        if (vcam == null) vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null) noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float duration, float amplitude, float frequency)
    {
        if (noise == null) return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShakeRoutine(duration, amplitude, frequency));
    }

    private IEnumerator ShakeRoutine(float duration, float amplitude, float frequency)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
        routine = null;
    }
}
