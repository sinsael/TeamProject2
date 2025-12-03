using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltarUIManager : MonoBehaviour
{
    [SerializeField] private AltarSlot[] slots;

    private AltarSlot selectedSlot;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    private void Awake()
    {
        if (slots == null || slots.Length == 0)
        {
            slots = GetComponentsInChildren<AltarSlot>(true);
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            raycaster = canvas.GetComponent<GraphicRaycaster>();
        }

        eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (selectedSlot == null) return;
        if (!Input.GetMouseButtonDown(0)) return;

        if (IsClickOnAnyAltarSlot())
        {
            return;
        }

        DeselectCurrent();
    }

    private bool IsClickOnAnyAltarSlot()
    {
        if (raycaster == null || eventSystem == null) return false;

        PointerEventData ped = new PointerEventData(eventSystem);
        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(ped, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.GetComponentInParent<AltarSlot>() != null)
            {
                return true;
            }
        }

        return false;
    }

    public void OnClickSlot(AltarSlot slot)
    {
        if (slot == null) return;

        if (selectedSlot == slot)
        {
            // 나중에 여기서 "인벤 선택 아이템을 제단에 박기" 처리
            return;
        }

        DeselectCurrent();
        selectedSlot = slot;
        selectedSlot.SetHighlight(true);
    }

    public void DeselectCurrent()
    {
        if (selectedSlot == null) return;

        selectedSlot.SetHighlight(false);
        selectedSlot = null;
    }
}
