using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltarSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image highlightImage;

    private ItemData data;
    public bool isEmpty { get; private set; } = true;

    public ItemData GetItemData()
    {
        return data;
    }

    private void Awake()
    {
        if (iconImage == null)
        {
            if (transform.childCount > 0)
            {
                iconImage = transform.GetChild(0).GetComponent<Image>();
            }
        }

        if (highlightImage != null)
        {
            highlightImage.enabled = false;
        }

        ClearSlotVisual();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AltarUIManager mgr = GetComponentInParent<AltarUIManager>();
        if (mgr != null)
        {
            mgr.OnClickSlot(this);
        }
    }

    public void SetHighlight(bool isOn)
    {
        if (highlightImage == null) return;

        highlightImage.enabled = isOn;
        if (isOn)
        {
            highlightImage.transform.SetAsLastSibling();
        }
    }

    public void SetItem(ItemData newData)
    {
        data = newData;

        if (iconImage != null)
        {
            iconImage.sprite = (data != null) ? data.itemSprite : null;
            iconImage.preserveAspect = true;
            SetAlpha((data != null) ? 1f : 0f);
        }

        isEmpty = (data == null);
    }

    public void ClearSlot()
    {
        data = null;
        ClearSlotVisual();
        isEmpty = true;
        SetHighlight(false);
    }

    private void ClearSlotVisual()
    {
        if (iconImage == null) return;

        iconImage.sprite = null;
        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (iconImage == null) return;

        Color c = iconImage.color;
        c.a = alpha;
        iconImage.color = c;
    }
}
