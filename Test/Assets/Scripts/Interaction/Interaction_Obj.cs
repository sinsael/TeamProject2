using UnityEngine;

public class Interaction_Obj : MonoBehaviour, IInteraction
{
    SpriteRenderer sr;
    Color currentcol;
    protected bool material = false;

    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentcol = sr.color;
    }

    public virtual void OnHitByRay()
    {
        if(!material)
        {
            material = true;
            sr.color = Color.red;
        }
    }

    public virtual void OnLeaveRay()
    {
        if(material)
        {
            material = false;
            sr.color = currentcol;
        }
    }

    public void OnSelect()
    {
        sr.color = Color.green;
        Debug.Log(gameObject.name + " is selected");
    }

    public void OnDeselect()
    {
        sr.color = currentcol;
        Debug.Log(gameObject.name + " is deselected");
    }

    public virtual void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
