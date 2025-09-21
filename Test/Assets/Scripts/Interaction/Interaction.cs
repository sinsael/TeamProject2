
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public Transform ObjCheck;
    public float ObjCheckRadius;

    public LayerMask WhatIsObj;

    public Collider2D[] ObjColliders;

    private HashSet<IInteraction> detectedInteractions = new HashSet<IInteraction>();

    public Collider2D[] GetDetectedColliders()
    {
        return ObjColliders = Physics2D.OverlapCircleAll(ObjCheck.position, ObjCheckRadius, WhatIsObj);
    }

    public void UpdateObjDetected()
    {
        GetDetectedColliders();

        HashSet<IInteraction> currentFrameInteractions = new HashSet<IInteraction>();

        foreach (var collider in ObjColliders)
        {
            if (collider.TryGetComponent<IInteraction>(out var interactionComponent))
            {
                currentFrameInteractions.Add(interactionComponent);
            }
        }

        detectedInteractions.RemoveWhere(interaction =>
        {
            if (!currentFrameInteractions.Contains(interaction))
            {
                interaction?.OnLeaveRay();
                return true;
            }
            return false;
        });

        foreach (var interactionComp in currentFrameInteractions)
        {
            if (detectedInteractions.Add(interactionComp))
            {
                interactionComp?.OnHitByRay();
            }
        }

    }

    public void Interact()
    {
        foreach (var interactionComp in GetDetectedColliders())
        {
            IInteraction interaction = interactionComp.GetComponent<IInteraction>();
            interaction?.OnInteract();
        }
    }
}
