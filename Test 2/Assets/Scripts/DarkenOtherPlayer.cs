using UnityEngine;

public class DarkenOtherPlayer : MonoBehaviour
{
    public Renderer player2Renderer; // P2의 SkinnedMeshRenderer
    public Color darkenColor = new Color(0.5f, 0.5f, 0.5f);

    private Color originalColor;
    private Material p2Material;

    void Start()
    {
        // P2의 재질(Material)을 미리 캐싱해 둡니다.
        p2Material = player2Renderer.material;
        originalColor = p2Material.color;
    }

    // 이 카메라가 렌더링을 시작하기 '직전'에 호출됨
    void OnPreRender()
    {
        if (p2Material != null)
        {
            // P2의 색상을 어둡게 바꿈
            p2Material.color = darkenColor;
        }
    }

    // 렌더링이 끝난 '직후'에 호출됨
    void OnPostRender()
    {
        if (p2Material != null)
        {
            // 다른 카메라가 볼 수 있도록 P2의 색상을 원상 복구
            p2Material.color = originalColor;
        }
    }
}