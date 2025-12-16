using UnityEngine;

public class Interaction_FireWood : Interaction_Obj
{
    public GameObject FireWood;
    public GameObject vector;
    public ItemData itemData;

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        if (Inventory.Instance.HasItem(itemData))
        {
            // 장작나무 소환
            Instantiate(FireWood, vector.transform.position, Quaternion.identity);
            Inventory.Instance.RemoveItem(itemData); // 인벤토리에서 장작나무 아이템 제거
        }
    }
}
