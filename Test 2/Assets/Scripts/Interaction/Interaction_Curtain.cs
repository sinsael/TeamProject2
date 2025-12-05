using UnityEngine;

public class Interaction_Curtain : Interaction_Obj
{
    [SerializeField] private Sprite secondSprite;

    [SerializeField] private ItemData requiredItem;
    [SerializeField] private bool lockUntilHasItem = true;

    private SpriteRenderer srLocal;
    private Collider2D col;

    private bool changed;
    private bool unlocked;

    public override void Start()
    {
        base.Start();

        srLocal = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        unlocked = !lockUntilHasItem;
        col.enabled = unlocked;
    }

    private void Update()
    {
        if (changed) return;
        if (unlocked) return;
        if (!lockUntilHasItem) return;

        if (Inventory.Instance.HasItem(requiredItem))
        {
            unlocked = true;
            col.enabled = true;
        }
    }

    public override void OnInteract(PlayerInputHandler interactor)
    {
        if (changed) return;
        if (lockUntilHasItem && !unlocked) return;

        srLocal.sprite = secondSprite;
        changed = true;

        col.enabled = false;
    }
}
