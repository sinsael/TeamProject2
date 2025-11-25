using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;


public class Interaction_Valve : Interaction_Obj
{
    public GameObject block;
    public Vector3 vector;
    public int time;
    public bool valveOn = false;

    public override void OnInteract()
    {
        base.OnInteract();

        valveOn = true;

        block.SetActive(valveOn);

        transform.DORotate(vector,time, RotateMode.LocalAxisAdd);
    }
}
