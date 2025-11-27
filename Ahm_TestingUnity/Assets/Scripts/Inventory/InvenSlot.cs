using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class InvenSlot : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Image image;
    [SerializeField] private Image highlightImage;
    public bool isEmpty { get; private set; } = true;
    ItemData newData;

    public ItemData GetItemData() => newData;

    private void Awake()
    {
        if (image != null)
            image = transform.GetChild(0).GetComponent<Image>();

        ClearSlot();

        highlightImage.enabled = false;
    }

    // 아이템 슬롯 클릭
    public void OnPointerClick(PointerEventData eventData)
    {
        Inventory.Instance.OnClickSlot(this);
    }

    // 하이라이트 세팅
    public void SetHighlight(bool isOn)
    {
        if (highlightImage != null)
        {
            highlightImage.enabled = isOn;

            // [핵심] 켜질 때마다 "나를 맨 앞으로 보내줘!" 라고 명령
            if (isOn)
            {
                highlightImage.transform.SetAsLastSibling();
            }
        }
    }

    // 아이템 이미지 값
    public void SlotSetting(ItemData data)
    {
        newData = data;
        image.sprite = newData.itemSprite;

        SetAlpha(1);

        isEmpty = false;
    }

    // 아이템 슬롯 비우기
    public void ClearSlot()
    {
        newData = null;
        image.sprite = null;

        SetAlpha(0);

        isEmpty = true;

        SetHighlight(false);
    }

    private void SetAlpha(float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
