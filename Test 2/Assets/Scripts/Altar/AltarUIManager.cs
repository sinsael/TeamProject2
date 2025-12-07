using System;
using UnityEngine;

public class AltarUIManager : MonoBehaviour
{
    public static AltarUIManager Instance;

    [SerializeField] private AltarSlot[] slots;

    private AltarSlot selectedSlot;
    private AltarSlot pickedInsertedSlot;

    public event Action<bool> OnAltarCompleteChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NotifyAltarState();
    }

    public bool IsAltarComplete()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsInserted())
                return false;
        }
        return true;
    }

    private void NotifyAltarState()
    {
        bool complete = IsAltarComplete();
        if (OnAltarCompleteChanged != null)
            OnAltarCompleteChanged(complete);
    }

    public void OnClickSlot(AltarSlot slot)
    {
        ItemData invSelected = Inventory.Instance.GetSelectedItem();

        if (invSelected != null)
        {
            CancelPickMode();

            if (slot.CanAccept(invSelected))
            {
                Inventory.Instance.RemoveItem(invSelected);
                Inventory.Instance.DeselectCurrent();
                slot.Insert();
                NotifyAltarState();
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

    public bool TryReturnPickedToInventory()
    {
        if (pickedInsertedSlot == null)
            return false;

        ItemData data = pickedInsertedSlot.GetRequiredItem();

        bool ok = Inventory.Instance.AddList(data);
        if (!ok)
            return false;

        pickedInsertedSlot.RemoveFromAltar();
        pickedInsertedSlot.SetHighlight(false);
        pickedInsertedSlot = null;

        if (selectedSlot != null)
        {
            selectedSlot.SetHighlight(false);
            selectedSlot = null;
        }

        NotifyAltarState();
        return true;
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
