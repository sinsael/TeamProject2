using UnityEngine;
using Cinemachine;

public class WallBreakShakeImpulse : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulse;

    public void Play()
    {
        if (impulse == null) return;
        impulse.GenerateImpulse();
    }
}
