using UnityEngine;

public class Interaction_Obj : MonoBehaviour, IInteraction, IInteraction_circle
{
    protected SpriteRenderer sr;
    protected Color currentcol;
    protected bool material = false;

    public virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;
        currentcol = sr.color;
    }

    public virtual void OnHitByRay()
    {
        if (sr == null) return;

        sr.color = Color.red;
    }

    public virtual void OnLeaveRay()
    {
        if (sr == null) return;

        sr.color = currentcol;
    }

    public virtual void OnSelect()
    {
        if (sr == null) return;

        sr.color = Color.green;
        Debug.Log(gameObject.name + " is selected");
    }

    public virtual void OnDeselect()
    {
        if (sr == null)
            return;
        sr.color = currentcol;
        Debug.Log(gameObject.name + " is deselected");
    }

    public virtual void OnInteract(PlayerInputHandler PlayerInput)
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
