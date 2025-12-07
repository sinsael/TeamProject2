using UnityEngine;
using UnityEngine.Rendering; // URP 렌더링 이벤트를 쓰기 위해 필수

public class DarkenOtherPlayer : MonoBehaviour
{
    public Renderer player2Renderer; // P2의 SkinnedMeshRenderer
    public Color darkenColor = new Color(0.5f, 0.5f, 0.5f);

    private Color originalColor;
    private Material p2Material;
    private Camera attachedCamera;

    void Awake()
    {
        attachedCamera = GetComponent<Camera>();

        // 머티리얼 캐싱 및 원본 색상 저장
        if (player2Renderer != null)
        {
            p2Material = player2Renderer.material;

            // URP 쉐이더 종류에 따라 프로퍼티 이름이 다를 수 있음 (_BaseColor 또는 _Color)
            if (p2Material.HasProperty("_BaseColor"))
                originalColor = p2Material.GetColor("_BaseColor");
            else
                originalColor = p2Material.color;
        }
    }

    void OnEnable()
    {
        // 렌더링 시작/끝 이벤트 구독
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDisable()
    {
        // 이벤트 구독 해제 (메모리 누수 방지)
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    // 렌더링 시작 전 (OnPreRender 대체)
    void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        // 현재 렌더링 중인 카메라가 이 스크립트가 붙은 카메라일 때만 실행
        if (camera == attachedCamera && p2Material != null)
        {
            if (p2Material.HasProperty("_BaseColor"))
                p2Material.SetColor("_BaseColor", darkenColor);
            else
                p2Material.color = darkenColor;
        }
    }

    void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        // 색상 원상복구
        if (camera == attachedCamera && p2Material != null)
        {
            if (p2Material.HasProperty("_BaseColor"))
                p2Material.SetColor("_BaseColor", originalColor);
            else
                p2Material.color = originalColor;
        }
    }
}