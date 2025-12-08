using UnityEngine;

public class Interaction_BookPile : Interaction_Obj
{
    [Header("상호작용 시 획득하는 아이템")]
    [SerializeField] private ItemData pickItem;

    private bool isUsed = false;

    public override void OnInteract(PlayerInputHandler interactor)
    {
        if (interactor == null)
            return;

        if (isUsed)
            return;

        if (pickItem == null)
        {
            Debug.LogWarning("획득할 아이템 선택하기");
            return;
        }

        bool added = Inventory.Instance.AddList(pickItem);
        if (!added)
        {
            Debug.Log("인벤토리가 가득 차서 아이템을 획득할 수 없습니다.");
            return;
        }

        isUsed = true;

        Interaction_BreakWall.UnlockWallGimmickByBookPile();
        Debug.Log("책 더미를 조사했다. 벽 기믹이 활성화되었다.");
    }
}
