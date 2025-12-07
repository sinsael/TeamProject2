using UnityEngine;

public class Interaction_MaskOBJ : Interaction_Obj
{
    [Header("마스크 활성화")]
    [SerializeField] private ItemData triggerItem;  // 먹으면 활성화 되는 아이템 지정해주기
    [SerializeField] private GameObject targetMask; // 활성화 시킬 마스크 오브젝트 지정
    private bool latchOnce = true;
    private bool startHidden = true;

    private bool blockWhenCovered = true;

    [Header("가려짐으로 상호작용 차단")]
    [SerializeField] private Collider2D interactCollider; // 상호작용 하는 콜라이더
    [SerializeField] private Collider2D coverCollider;    // 그런 상호작용하는 콜라이더를 막는 콜라이더

    [Header("숨겨진 아이템 활성화")]
    [SerializeField] private GameObject hiddenItem; // 상호작용 시 나올 아이템
    [SerializeField] private Transform dropPoint;   // 나오는 위치

    private bool detachOnReveal = true;
    private bool revealOnce = true;
    private bool disableThisAfterReveal = true;

    private bool maskDone;
    private bool revealed;
    private bool lastCovered;

    private Bounds cachedSelfBounds;
    private bool hasCachedSelfBounds;
    private Vector3 cachedPos;

    public override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) currentcol = sr.color;

        if (interactCollider == null)
            interactCollider = GetComponent<Collider2D>();

        if (dropPoint == null)
            dropPoint = transform;

        if (targetMask != null && startHidden)
            targetMask.SetActive(false);

        if (hiddenItem != null)
            hiddenItem.SetActive(false);

        CacheSelfBounds();


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

            if (latchOnce)
                maskDone = true;
        }
    }

    private bool IsCovered()
    {
        if (interactCollider == null) return false;
        if (coverCollider == null) return false;

        if (!coverCollider.enabled) return false;
        if (!coverCollider.gameObject.activeInHierarchy) return false;

        Bounds selfB = GetSelfBoundsApprox();
        Bounds coverB = coverCollider.bounds;

        return selfB.size.sqrMagnitude > 0f && selfB.Intersects(coverB);
    }

    private void ApplyCoveredState(bool covered)
    {
        if (interactCollider != null)
        {
            if (covered)
                CacheSelfBounds();

            interactCollider.enabled = !covered;
        }

        if (covered)
        {
            OnLeaveRay();
            OnDeselect();
        }
    }

    private void CacheSelfBounds()
    {
        if (interactCollider == null)
            return;

        Bounds b = interactCollider.bounds;
        if (b.size.sqrMagnitude <= 0f)
            return;

        cachedSelfBounds = b;
        hasCachedSelfBounds = true;
        cachedPos = transform.position;
    }

    private Bounds GetSelfBoundsApprox()
    {
        if (interactCollider != null)
        {
            Bounds b = interactCollider.bounds;
            if (b.size.sqrMagnitude > 0f)
            {
                cachedSelfBounds = b;
                hasCachedSelfBounds = true;
                cachedPos = transform.position;
                return b;
            }
        }

        if (hasCachedSelfBounds)
        {
            Vector3 delta = transform.position - cachedPos;
            Bounds moved = cachedSelfBounds;
            moved.center += delta;
            cachedSelfBounds = moved;
            cachedPos = transform.position;
            return moved;
        }

        return new Bounds(transform.position, Vector3.zero);
    }

    public override void OnHitByRay()
    {
        if (sr == null) return;
        sr.color = Color.red;
    }

    public override void OnLeaveRay()
    {
        if (sr == null) return;
        sr.color = currentcol;
    }

    public override void OnSelect()
    {
        if (sr == null) return;
        sr.color = Color.green;
    }

    public override void OnDeselect()
    {
        if (sr == null) return;
        sr.color = currentcol;
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
        {
            Debug.LogWarning("숨겨진 아이템이 지정되지 않았습니다.");
            return;
        }

        if (detachOnReveal)
            hiddenItem.transform.SetParent(null);

        Vector3 pos = (dropPoint != null) ? dropPoint.position : transform.position;
        hiddenItem.transform.position = pos;

        hiddenItem.SetActive(true);

        revealed = true;

        if (disableThisAfterReveal && interactCollider != null)
            interactCollider.enabled = false;
    }
}
