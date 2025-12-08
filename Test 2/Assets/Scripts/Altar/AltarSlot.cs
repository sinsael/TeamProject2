using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltarSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Refs")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image highlightImage;

    [Header("Rule")]
    [SerializeField] private ItemData requiredItem;

    [Header("Lock Visual")]
    [Range(0f, 1f)][SerializeField] private float lockToGray = 0.75f;
    [Range(0f, 1f)][SerializeField] private float lockBrightness = 0.55f;

    private bool inserted;
    private Color normalColor = Color.white;

    private void Awake()
    {
        if (iconImage != null)
        {
            normalColor = iconImage.color;

            // 제단은 아이템 이미지가 미리 보여야 하니 기본 스프라이트 세팅
            if (requiredItem != null && iconImage.sprite == null)
            {
                iconImage.sprite = requiredItem.itemSprite;
                iconImage.preserveAspect = true;
            }
        }

        if (highlightImage != null)
        {
            highlightImage.enabled = false;
        }

        ApplyVisual();
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

    public ItemData GetRequiredItem()
    {
        return requiredItem;
    }

    public bool IsInserted()
    {
        return inserted;
    }

    public bool CanAccept(ItemData selected)
    {
        if (inserted) return false;
        if (selected == null) return false;
        return selected == requiredItem;
    }

    public void Insert()
    {
        inserted = true;
        ApplyVisual();
    }

    public void RemoveFromAltar()
    {
        inserted = false;
        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (iconImage == null) return;

        if (inserted)
        {
            iconImage.color = normalColor;
            return;
        }

        // 잠김 상태는 채도 낮추고 어둡게
        Color c = normalColor;
        float gray = (c.r + c.g + c.b) / 3f;

        float r = Mathf.Lerp(c.r, gray, lockToGray) * lockBrightness;
        float g = Mathf.Lerp(c.g, gray, lockToGray) * lockBrightness;
        float b = Mathf.Lerp(c.b, gray, lockToGray) * lockBrightness;

        iconImage.color = new Color(r, g, b, c.a);
    }
}
