using UnityEngine;

public class OutLineChange : MonoBehaviour
{
    [Header("감지 대상")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float radius = 1.5f;

    [Header("머티리얼")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material nearMaterial;

    private SpriteRenderer sr;
    private bool isNear = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && normalMaterial == null)
        {
            normalMaterial = sr.sharedMaterial;
        }
    }

    private void Update()
    {
        bool nowNear = Physics2D.OverlapCircle(transform.position, radius, playerLayer) != null;

        if (nowNear == isNear)
            return;

        isNear = nowNear;

        if (sr == null)
            return;

        // 가까이 오면 nearMaterial, 멀어지면 normalMaterial
        sr.sharedMaterial = isNear ? nearMaterial : normalMaterial;
    }
}
