using UnityEngine;

public class Interaction_ArtCase : Interaction_Obj
{
    public SpriteRenderer art_Sr;
    public Sprite art;

    public ItemData item; // 필요한 아이템
    public bool isCompleted = false; // 완료 여부

    public override void Start()
    {
        base.Start();
        art_Sr = GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);
        if (Inventory.Instance.HasItem(item))
        {
            art_Sr.sprite = art;
            isCompleted = true;
            Inventory.Instance.RemoveItem(item);
        }
    }

}
