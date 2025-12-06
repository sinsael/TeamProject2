using UnityEngine;

public class Interaction_ArtCase : Interaction_Obj
{
    public SpriteRenderer art_Sr;
    public Sprite art;

    public ItemData item;
    public bool isCompleted = false;

    public override void Start()
    {
        base.Start();
        art_Sr = GetComponent<SpriteRenderer>();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);
        if (Inventory.Instance.HasItem(item))
        {
            art_Sr.sprite = art;
            isCompleted = true;
            Inventory.Instance.HasItem(item);
        }
    }

}
