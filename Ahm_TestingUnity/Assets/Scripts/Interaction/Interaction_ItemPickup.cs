using UnityEngine;

public class Interaction_ItemPickup : Interaction_Obj
{
    public ItemData ItemData;
    //아이템 주을시 획득
    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        // AddList가 true(성공)를 반환했을 때만 안쪽 코드 실행
        if (Inventory.Instance.AddList(ItemData))
        {
            // 성공했으니 맵에서 삭제
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("가방이 꽉 차서 못 주웠어!");
        }
    }
}
