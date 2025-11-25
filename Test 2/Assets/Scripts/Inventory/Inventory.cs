using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{ 
    int horizontal = 4;
    int vertical = 1;

    [SerializeField] private GameObject invenSlot;
    private List<ItemData> items = new List<ItemData>();
    private List<InvenSlot> Slots = new List<InvenSlot>();

    public ItemData testData;

    private void Awake()
    {
        FreshSlot();
    }

    void FreshSlot()
    {
        int index = 0;
        for (int i = 0; i < vertical; i++)
        {
            for (int j = 0; j < horizontal; j++)
            {
                InvenSlot slot = Instantiate(invenSlot, this.transform).GetComponent<InvenSlot>();
                Slots.Add(slot);
                if (index < items.Count)
                {
                    slot.SlotSetting(items[index]);
                }
                index++;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddList(testData);
        }
    }

    public void AddList(ItemData data)
    {
        foreach (InvenSlot slot in Slots)
        {
            if (slot.isEmpty)
            {
                slot.SlotSetting(data);
                items.Add(data);
                slot.OnItemForce += Puzzle;
                return;
            }
        }
    }

    public void Puzzle(ItemData data)
    {
        Debug.Log("as");
    }
}
