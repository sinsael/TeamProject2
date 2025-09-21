
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("상호작용 오브젝트 감지")]
    public Transform ObjCheck;
    public float ObjCheckRadius;

    public LayerMask WhatIsObj;

    public Collider2D[] ObjColliders;

    private HashSet<IInteraction> detectedInteractions = new HashSet<IInteraction>();

    [Space]
    [Header("상호작용 가능 범위")]
    public Transform interactionCheck; 
    public float interactionRadius;    
    public LayerMask interactableLayer; 

    private IInteraction currentTarget; 
    private IInteraction previousTarget; 

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

    public void FindBestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactionCheck.position, interactionRadius, interactableLayer);

        float closestDist = float.MaxValue;
        IInteraction bestTarget = null;

        foreach (var col in colliders)
        {
            float dist = Vector2.Distance(interactionCheck.position, col.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                bestTarget = col.GetComponent<IInteraction>();
            }
        }

        currentTarget = bestTarget;
    }

    public void HandleTargetChange()
    {
        if (previousTarget != currentTarget)
        {
            previousTarget?.OnDeselect(); // 이전 타겟이 있었다면 선택 해제
            currentTarget?.OnSelect(); // 새 타겟이 있다면 선택

            previousTarget = currentTarget;
        }
    }

    public void Interact()
    {
        if (currentTarget != null)
        {
            currentTarget.OnInteract();
        }
    }
}
