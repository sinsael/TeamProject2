using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    int horizontal = 4;
    int vertical = 1;

    [SerializeField] private GameObject invenSlot;
    private List<ItemData> items = new List<ItemData>();
    private List<InvenSlot> Slots = new List<InvenSlot>();

    public ItemData testData;

    private InvenSlot selectedSlot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        if (transform.parent != null)
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        FreshSlot();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearAllItems();
    }

    public void ClearAllItems()
    {
        items.Clear();

        foreach (InvenSlot slot in Slots)
        {
            slot.ClearSlot();
        }

        DeselectCurrent();

        Debug.Log("씬 이동으로 인해 인벤토리가 초기화되었습니다.");
    }

    /// <summary>
    /// 아이템 추가
    /// </summary>
    /// <param name="data"></param>
    /// <returns>아이템 데이터값 넣으면 됨</returns>
    public bool AddList(ItemData data) // void -> bool 변경
    {
        foreach (InvenSlot slot in Slots)
        {
            if (slot.isEmpty)
            {
                slot.SlotSetting(data);
                items.Add(data);
                return true; // 성공! (들어갔음)
            }
        }

        // 반복문을 다 돌았는데 빈 슬롯이 없다면?
        Debug.Log("인벤토리가 가득 찼습니다.");
        return false; // 실패! (꽉 찼음)
    }
    // 아이템 빼기
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
    //두번째 클릭일시 실행 및 다른 슬롯 선택시 해제
    public void OnClickSlot(InvenSlot slot)
    {
        // 빈 슬롯을 클릭했다면? (선택을 풀거나 무시)
        if (slot.isEmpty)
        {
            DeselectCurrent(); // 빈 곳 누르면 선택 해제
            return;
        }

        // 1. 이미 선택된 슬롯을 '또' 클릭했는가? -> 아이템 사용!
        if (selectedSlot == slot)
        {
            ItemData data = slot.GetItemData();
            if (data != null)
            {
                data.Use(); // 아이템 사용 (삭제는 아이템 내부 or RemoveItem으로 처리)

                // (선택사항) 사용 후 선택 해제하려면 아래 주석 해제
                // DeselectCurrent(); 
            }
        }
        // 2. 다른 슬롯을 클릭했거나, 처음 클릭인가? -> 단순 선택
        else
        {
            // 기존 선택된 게 있다면 끄기
            DeselectCurrent();

            // 새로 클릭한 슬롯 선택
            selectedSlot = slot;
            selectedSlot.SetHighlight(true); // 테두리 켜기

            Debug.Log($"[{slot.GetItemData().itemName}] 선택됨");
        }
    }

    //하이라이트 끄기
    public void DeselectCurrent()
    {
        if (selectedSlot != null)
        {
            selectedSlot.SetHighlight(false); // 기존 슬롯 테두리 끄기
            selectedSlot = null;
        }
    }
    // 아이템 리스트 제거
    public void RemoveItem(ItemData data)
    {
        // 1. 데이터 리스트에서 제거
        if (items.Contains(data))
        {
            items.Remove(data);
        }

        // 2. 해당 아이템을 보여주고 있던 슬롯 UI 찾아서 비우기
        // (간단하게 구현하기 위해 전체 슬롯을 돌며 찾습니다)
        foreach (InvenSlot slot in Slots)
        {
            if (slot.GetItemData() == data)
            {
                slot.ClearSlot();
                return; // 하나 지웠으면 종료
            }
        }
    }

    // 어떤 아이템을 가지고 있는지 여부

    public bool HasItem(ItemData data)
    {
        return items.Contains(data);
    }

    // 선택된 아이템 데이터 공개 (벽파괴에서 쓸거임(BreakWall.cs))
    public ItemData GetSelectedItem()
    {
        if (selectedSlot == null)
            return null;

        return selectedSlot.GetItemData();
    }
}
