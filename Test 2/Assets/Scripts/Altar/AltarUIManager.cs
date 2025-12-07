using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AltarUIManager : MonoBehaviour
{
    public static AltarUIManager Instance;

    [SerializeField] private AltarSlot[] slots;

    private AltarSlot selectedSlot;
    private AltarSlot pickedInsertedSlot;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    private void Awake()
    {
        Instance = this;

        if (slots == null || slots.Length == 0)
        {
            slots = GetComponentsInChildren<AltarSlot>(true);
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null) raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    public void DeselectCurrent()
    {
        CancelPickMode();
    }

    public bool HasPickedInsertedSlot()
    {
        return pickedInsertedSlot != null;
    }

    public bool TryReturnPickedToInventory()
    {
        if (pickedInsertedSlot == null) return false;
        if (Inventory.Instance == null) return false;

        ItemData data = pickedInsertedSlot.GetRequiredItem();
        if (data == null) return false;

        bool ok = Inventory.Instance.AddList(data);
        if (!ok)
        {
            return false;
        }

        pickedInsertedSlot.RemoveFromAltar();
        pickedInsertedSlot.SetHighlight(false);
        pickedInsertedSlot = null;

        if (selectedSlot != null)
        {
            selectedSlot.SetHighlight(false);
            selectedSlot = null;
        }

        return true;
    }

    public void OnClickSlot(AltarSlot slot)
    {
        if (slot == null) return;

        ItemData invSelected = null;
        if (Inventory.Instance != null)
        {
            invSelected = Inventory.Instance.GetSelectedItem();
        }

        if (invSelected != null)
        {
            CancelPickMode();

            if (slot.CanAccept(invSelected))
            {
                Inventory.Instance.RemoveItem(invSelected);
                Inventory.Instance.DeselectCurrent();
                slot.Insert();
            }
            return;
        }

        if (slot.IsInserted())
        {
            if (pickedInsertedSlot == slot)
            {
                slot.SetHighlight(false);
                pickedInsertedSlot = null;
                selectedSlot = null;
                return;
            }

            CancelPickMode();

            pickedInsertedSlot = slot;
            selectedSlot = slot;
            selectedSlot.SetHighlight(true);
            return;
        }

        CancelPickMode();
        selectedSlot = slot;
        selectedSlot.SetHighlight(true);
    }

    private void CancelPickMode()
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetHighlight(false);
            selectedSlot = null;
        }

        pickedInsertedSlot = null;
    }
}
