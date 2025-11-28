using UnityEngine;

public class Interaction_ItemPickup : Interaction_Obj
{
    public ItemData ItemData;
    //아이템 주을시 획득
    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
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
