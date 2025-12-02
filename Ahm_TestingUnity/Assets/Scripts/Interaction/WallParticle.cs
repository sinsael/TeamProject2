using UnityEngine;

public class BreakVfxSpawner : MonoBehaviour
{
    [Header("파티클 프리펩")]
    [SerializeField] private ParticleSystem debrisPrefab;

    public void SpawnDebris(Vector2 pos)
    {
        if (debrisPrefab == null) return;

        ParticleSystem ps = Instantiate(debrisPrefab, pos, Quaternion.identity);
        ps.Play();

        float life = ps.main.duration + ps.main.startLifetime.constantMax;
        Destroy(ps.gameObject, life + 0.2f);
    }
}
