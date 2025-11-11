using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public InventorySlot[] uiSlots;
    public GameObject inventoryItemPrefab;

    private void Start()
    {
        inventoryManager.OnInventoryChanged += UpdateUI;

        InitializeSlots();

        UpdateUI();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].GetComponent<InventorySlot>().Initialize(this, i);
        }
    }

    private void OnDestroy()
    {
        inventoryManager.OnInventoryChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (i < inventoryManager.inventoryData.Count)
            {
                Item item = inventoryManager.inventoryData[i];
                InventoryItem itemScript;

                if (uiSlots[i].transform.childCount == 0)
                {
                    GameObject itemObj = Instantiate(inventoryItemPrefab, uiSlots[i].transform);
                    itemScript = itemObj.GetComponent<InventoryItem>();
                }
                else
                {
                    itemScript = uiSlots[i].transform.GetChild(0).GetComponent<InventoryItem>();
                }

                itemScript.Initialize(item, i, this);
            }
            else if (uiSlots[i].transform.childCount > 0)
            {
                Destroy(uiSlots[i].transform.GetChild(0).gameObject);
            }
        }
    }

    public void OnSlotClicked(int index)
    {
        // 클릭된 인덱스를 InventoryManager에게 그대로 전달
        inventoryManager.SetSelectedIndex(index);
    }
}
