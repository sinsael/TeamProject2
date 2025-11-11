using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> inventoryData = new List<Item>();
    public int selectedIndex { get; private set; } = -1;

    public event Action OnInventoryChanged;

    public void AddItem(Item item)
    {
        inventoryData.Add(item);
        OnInventoryChanged?.Invoke();
    }

    public void SetSelectedIndex(int index)
    {
        if (index < 0 || index >= inventoryData.Count)
        {
            return;
        }

        if (index == selectedIndex)
        {
            selectedIndex = -1;
            Debug.Log("선택 해제됨");
        }
        else
        {
            selectedIndex = index;
            // 3. 바로 Item의 이름에 접근합니다.
            Debug.Log(inventoryData[index].name + " 선택됨");
        }
    }

    public Item GetSelectedItem(bool use)
    {
        if (selectedIndex == -1) return null;

        // 4. 리스트에서 아이템을 바로 가져옵니다.
        Item item = inventoryData[selectedIndex];

        if (use)
        {
            // 5. '사용'은 곧 '인벤토리에서 제거'를 의미합니다.
            inventoryData.RemoveAt(selectedIndex);
            selectedIndex = -1; // 아이템이 사라졌으니 선택 해제
            OnInventoryChanged?.Invoke(); // UI 새로고침
        }
        return item; // 방금 사용(제거)한 아이템을 반환
    }
}
