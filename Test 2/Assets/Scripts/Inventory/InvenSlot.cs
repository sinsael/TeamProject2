using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class InvenSlot : MonoBehaviour, IPointerClickHandler
{
    public Action<ItemData> OnItemForce;

    [SerializeField] private Image image;
    public bool isEmpty { get; private set; } = true;
    ItemData newData;

    private void Awake()
    {
        if (image != null)
            image = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.gameObject + " Å¬¸¯µÊ!");
        this.OnItemForce.Invoke(newData);
    }

    public void SlotSetting(ItemData data)
    {
        newData = data;
        image.sprite = newData.itemSprite;
        isEmpty = false;
    }

    public void ClearSlot()
    {
        newData = null;
        image.sprite = null;
        isEmpty = true;
    }
}
