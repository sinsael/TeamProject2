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

    public virtual void OnHitByRay()
    {
        if (sr == null) return;
        if (highlightMaterial != null)
        {
            sr.material = highlightMaterial;
        }
    }

    public virtual void OnLeaveRay()
    {
        if (sr == null) return;

        if (highlightMaterial != null)
        {
            sr.material = originalMaterial;
        }
    }

    public virtual void OnSelect()
    {
        if (sr == null) return;

        Debug.Log(gameObject.name + " is selected");
    }

    public virtual void OnDeselect()
    {
        if (sr == null)
            return;
        Debug.Log(gameObject.name + " is deselected");
    }

    public virtual void OnInteract(PlayerInputHandler PlayerInput)
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
