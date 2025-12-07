using UnityEngine;

public class Interaction_Curtain : Interaction_Obj
{
    public enum CurtainState
    {
        Default,
        Folded
    }

    [SerializeField] private Sprite secondSprite;

    [SerializeField] private ItemData requiredItem;
    [SerializeField] private bool lockUntilHasItem = true;

    private SpriteRenderer srLocal;
    private Collider2D col;

    private bool unlocked;
    private CurtainState state = CurtainState.Default;

    public bool IsFolded
    {
        get { return state == CurtainState.Folded; }
    }

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
        if (state == CurtainState.Folded) return;
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
        if (state == CurtainState.Folded) return;
        if (lockUntilHasItem && !unlocked) return;

        srLocal.sprite = secondSprite;
        state = CurtainState.Folded;

        col.enabled = false;
    }
}
