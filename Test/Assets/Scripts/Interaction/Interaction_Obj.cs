using UnityEngine;

public class Interaction_Obj : MonoBehaviour, IInteraction
{
    SpriteRenderer sr;
    Color currentcol;
    public bool material = false;

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

    public virtual void OnInteract()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
