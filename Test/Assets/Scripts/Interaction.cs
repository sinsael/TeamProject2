using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaction : MonoBehaviour
{
    public bool material = false;

    public virtual void OnHitByRay()
    {
        if(!material)
        {
            material = true;
            Debug.Log("Hit by ray");
        }
    }

    public virtual void OnLeaveRay()
    {
        if(material)
        {
            material = false;
            Debug.Log("Leave ray");
        }
    }
}
