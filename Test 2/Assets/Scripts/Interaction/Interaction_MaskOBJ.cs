using UnityEngine;

public class Interaction_MaskOBJ : Interaction_Obj
{
    [Header("Mask Activation")]
    [SerializeField] private ItemData triggerItem;
    [SerializeField] private GameObject targetMask;
    [SerializeField] private bool latchOnce = true;
    [SerializeField] private bool startHidden = true;

    [Header("Block Interaction When Covered")]
    [SerializeField] private bool blockWhenCovered = true;
    [SerializeField] private Collider2D interactCollider;
    [SerializeField] private Collider2D coverCollider;
    [SerializeField] private Collider2D coverCheckCollider;

    [Header("Hidden Item Reveal")]
    [SerializeField] private GameObject hiddenItem;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private bool detachOnReveal = true;
    [SerializeField] private bool revealOnce = true;
    [SerializeField] private bool disableThisAfterReveal = true;

    private bool maskDone;
    private bool revealed;
    private bool lastCovered;

    public override void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (interactCollider == null)
            interactCollider = GetComponent<Collider2D>();

        if (dropPoint == null)
            dropPoint = transform;

        if (targetMask != null && startHidden)
            targetMask.SetActive(false);

        if (hiddenItem != null)
            hiddenItem.SetActive(false);

        lastCovered = false;
        if (blockWhenCovered)
        {
            lastCovered = IsCovered();
            ApplyCoveredState(lastCovered);
        }
    }

    private void Update()
    {
        UpdateMaskActivation();

        if (!blockWhenCovered)
            return;

        bool covered = IsCovered();
        if (covered != lastCovered)
        {
            lastCovered = covered;
            ApplyCoveredState(covered);
        }
    }

    private void UpdateMaskActivation()
    {
        if (maskDone) return;
        if (triggerItem == null) return;
        if (targetMask == null) return;
        if (Inventory.Instance == null) return;

        if (Inventory.Instance.HasItem(triggerItem))
        {
            targetMask.SetActive(true);
            if (latchOnce) maskDone = true;
        }
    }

    private bool IsCovered()
    {
        if (coverCollider == null) return false;
        if (coverCheckCollider == null) return false;

        if (!coverCollider.enabled) return false;
        if (!coverCollider.gameObject.activeInHierarchy) return false;

        ColliderDistance2D d = coverCheckCollider.Distance(coverCollider);
        return d.isOverlapped;
    }

    private void ApplyCoveredState(bool covered)
    {
        if (interactCollider != null)
            interactCollider.enabled = !covered;

        if (covered)
        {
            OnLeaveRay();
            OnDeselect();
        }
    }

    public override void OnHitByRay()
    {
        if (sr == null) return;
        sr.color = Color.red;
    }

    public override void OnLeaveRay()
    {
        if (sr == null) return;
    }

    public override void OnSelect()
    {
        if (sr == null) return;
        sr.color = Color.green;
    }

    public override void OnDeselect()
    {
        if (sr == null) return;
    }

    public override void OnInteract(PlayerInputHandler interactor)
    {
        if (interactor == null)
            return;

        if (revealOnce && revealed)
            return;

        if (blockWhenCovered && IsCovered())
            return;

        RevealHiddenItem();
    }

    private void RevealHiddenItem()
    {
        if (hiddenItem == null)
            return;

        if (detachOnReveal)
            hiddenItem.transform.SetParent(null);

        Vector3 pos = dropPoint != null ? dropPoint.position : transform.position;
        hiddenItem.transform.position = pos;
        hiddenItem.SetActive(true);

        revealed = true;

        if (disableThisAfterReveal)
        {
            if (interactCollider != null)
                interactCollider.enabled = false;

            blockWhenCovered = false;
            enabled = false;
        }
    }
}
