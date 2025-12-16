using UnityEngine;

public class Interaction_Obj : MonoBehaviour, IInteraction, IInteraction_circle
{
    protected SpriteRenderer sr;
    public Material highlightMaterial;
    Material originalMaterial;

    public virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;
        originalMaterial = sr.material;
    }

    // 접촉 시 호출
    public virtual void OnHitByRay()
    {
        Debug.Log("접촉");
        if (sr == null) return;
        
    }

    // 접촉 해제 시 호출
    public virtual void OnLeaveRay()
    {
        if (sr == null) return;

      
    }

    // 선택 시 호출
    public virtual void OnSelect()
    {
        if (sr == null) return;
        if (highlightMaterial != null)
        {
            sr.material = highlightMaterial;
        }
        Debug.Log(gameObject.name + " is selected");
    }

    // 선택 해제 시 호출
    public virtual void OnDeselect()
    {
        if (sr == null)
            return;

        if (highlightMaterial != null)
        {
            sr.material = originalMaterial;
        }
        Debug.Log(gameObject.name + " is deselected");
    }

    // 상호작용 시 호출
    public virtual void OnInteract(PlayerInputHandler PlayerInput)
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
