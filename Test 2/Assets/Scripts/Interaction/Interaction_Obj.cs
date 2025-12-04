using UnityEngine;

public class Interaction_Obj : MonoBehaviour, IInteraction, IInteraction_circle
{
    protected SpriteRenderer sr;
    protected Color currentcol;
    protected bool material = false;

    public virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        currentcol = sr.color;
    }

    public virtual void OnHitByRay()
    {
            sr.color = Color.red;
    }

    public virtual void OnLeaveRay()
    {
            sr.color = currentcol;
    }

    public virtual void OnSelect()
    {
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
