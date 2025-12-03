
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [Header("첫번쨰 원형 레이캐스트 오브젝트 감지")]
    public Transform ObjCheck; 
    public float ObjCheckRadius; // 감 반경

    public LayerMask WhatIsObj; // 감지할 레이어

    public Collider2D[] ObjColliders; // 감지된 콜라이더들

    private HashSet<IInteraction_circle> detectedInteractions_circle = new HashSet<IInteraction_circle>(); // 현재 감지된 상호작용 컴포넌트들

    [Space]
    [Header("상호작용 가능 오브젝트")]
    public Transform interactionCheck; 
    public float interactionRadius;    
    public LayerMask interactableLayer; 

    private IInteraction currentTarget; 
    private IInteraction previousTarget; 

    public Collider2D[] GetDetectedColliders()
    {
        return ObjColliders = Physics2D.OverlapCircleAll(ObjCheck.position, ObjCheckRadius, WhatIsObj);
    }

    // 감지된 오브젝트 업데이트
    public void UpdateObjDetected()
    {
        GetDetectedColliders();

        HashSet<IInteraction_circle> currentFrameInteractions = new HashSet<IInteraction_circle>();

        // 현재 프레임에 감지된 상호작용 컴포넌트들 수집
        foreach (var collider in ObjColliders)
        {
            if (collider.gameObject == this.gameObject)
            {
                continue;
            }

            if (collider.TryGetComponent<IInteraction_circle>(out var interactionComponent_circle))
            {
                currentFrameInteractions.Add(interactionComponent_circle);
            }
        }

        // 이전에 감지되었지만 현재 프레임에 없는 컴포넌트들 처리
        detectedInteractions_circle.RemoveWhere(interaction_circle =>
        {
            if (!currentFrameInteractions.Contains(interaction_circle))
            {
                interaction_circle?.OnLeaveRay();
                return true;
            }
            return false;
        });

        // 현재 프레임에 새로 감지된 컴포넌트들 처리
        foreach (var interactionComp_circle in currentFrameInteractions)
        {
            if (detectedInteractions_circle.Add(interactionComp_circle))
            {
                interactionComp_circle?.OnHitByRay();
            }
        }

    }

    // 가장 가까운 상호작용 오브젝트 찾기
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

    // 타겟 변경 처리
    public void HandleTargetChange()
    {
        if (previousTarget != currentTarget)
        {
            previousTarget?.OnDeselect(); // 이전 타겟에서 벗어남 처리
            currentTarget?.OnSelect(); // 새 타겟 선택 처리

            previousTarget = currentTarget;
        }
    }

    // 상호작용 실행
    public void Interact()
    {
        if (currentTarget != null)
        {
            currentTarget.OnInteract();
        }
    }
}
