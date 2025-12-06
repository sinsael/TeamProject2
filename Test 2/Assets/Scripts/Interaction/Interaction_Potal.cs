using UnityEngine;

public class Interaction_Potal : Interaction_Obj
{
    public SpriteRenderer potal;
    public ItemData key;
    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);
        if(Inventory.Instance.HasItem(key))
        {
            potal.enabled = false;
            Inventory.Instance.RemoveItem(key);
        }
    }
}
