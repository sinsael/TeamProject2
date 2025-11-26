using UnityEngine;
using UnityEngine.EventSystems; // IPointerClickHandler 사용

public class Interactor_Candle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Interaction_BreakWall targetWall; // 활성화할 벽
    [SerializeField] private GameObject flameObject;            // 촛불 불꽃 오브젝트(이거 쓸일 있을까1

    private bool isLit = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLit)
            return;

        isLit = true;

        // 촛불 불꽃 켜기 (이거 쓸일 있을까2)
        if (flameObject != null)
            flameObject.SetActive(true);

        // 벽 활성화
        if (targetWall != null)
            targetWall.ActivateByCandle();

        Debug.Log("촛불 클릭됨 벽 활성화");
    }
}
