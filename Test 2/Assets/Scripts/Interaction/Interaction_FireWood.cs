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
            Instantiate(FireWood, vector.transform.position, Quaternion.identity);
            Inventory.Instance.RemoveItem(itemData);
        }
    }
}
