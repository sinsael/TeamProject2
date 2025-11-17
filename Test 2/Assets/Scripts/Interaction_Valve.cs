using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class Interaction_Valve : Interaction_Obj
{
    public GameObject block;
    public bool valveOn = false;

    public override void OnInteract()
    {
        base.OnInteract();

        valveOn = true;

        block.SetActive(valveOn);
    }
}
