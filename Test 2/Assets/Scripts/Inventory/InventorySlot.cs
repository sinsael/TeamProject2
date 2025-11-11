using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    private InventoryUI uiController;
    private int mySlotIndex;

    public void Initialize(InventoryUI controller, int index)
    {
        uiController = controller;
        mySlotIndex = index;
    }

    private void Start()
    {
        uiController = GetComponentInParent<InventoryUI>();
    }

    public bool HasItem()
    {
        return transform.childCount > 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!HasItem())
        {
            if (uiController != null)
            {
                uiController.OnSlotClicked(mySlotIndex);
            }
        }

    }
}