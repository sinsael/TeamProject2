using UnityEngine;

public class WallParticle : MonoBehaviour
{
    public static WallParticle instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private ParticleSystem hitParticlePrefab;
    [SerializeField] private ParticleSystem breakParticlePrefab;

    private void Awake()
    {
        instance = this;
    }

    public void PlayHit(Vector3 position)
    {
        Spawn(hitParticlePrefab, position);
    }

    public void PlayBreak(Vector3 position)
    {
        Spawn(breakParticlePrefab, position);
    }

    private void Spawn(ParticleSystem prefab, Vector3 position)
    {
        if (prefab == null) return;

        ParticleSystem ps = Instantiate(prefab, position, Quaternion.identity);
        ps.Play();

        // 파티클이 끝나면 삭제 (프리팹은 Looping 꺼져있어야 함)
        ParticleSystem.MainModule main = ps.main;
        float life = main.duration + main.startDelay.constantMax + main.startLifetime.constantMax;

        Destroy(ps.gameObject, life + 0.1f);
    }
}
