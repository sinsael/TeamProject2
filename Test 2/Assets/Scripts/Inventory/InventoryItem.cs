using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]public Item item;
    [HideInInspector]public Image image;

    private int mySlotIndex;
    private InventoryUI uiController;
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Initialize(Item newItem, int index, InventoryUI controller)
    {
        item = newItem;
        image.sprite = item.image;
        mySlotIndex = index;
        uiController = controller;
    }

    // 2. 클릭 시 예외 대신, UI 컨트롤러에게 알리도록 수정
    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiController != null)
        {
            // "UI컨트롤러야! 나(mySlotIndex) 클릭됐어!"
            uiController.OnSlotClicked(mySlotIndex);
        }
    }
}
