using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Lever : Interaction_Obj
{
    public GameObject[] controlledObjects;

    private bool isActive = false;

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        // 상태에 따라 토글(Toggle) 실행
        if (isActive)
        {
            // 이미 활성화 상태였다면 -> 비활성화
            Deactivate();
            // 매니저에게 비활성화되었음을 알림
            LeverManager.Instance.RegisterDeactivation(this);
        }
        else
        {
            // 비활성화 상태였다면 -> 활성화
            Activate();
            // 매니저에게 활성화되었음을 알림
            LeverManager.Instance.RegisterActivation(this);
        }
    }

    // 오브젝트들을 활성화하고 상태 변경
    private void Activate()
    {
        isActive = true;
        foreach (var obj in controlledObjects)
        {
            if (obj != null) obj.SetActive(true);
        }
        // (필요하다면 여기에 레버 애니메이션/스프라이트 변경 코드 추가)
    }

    // 오브젝트들을 비활성화하고 상태 변경
    private void Deactivate()
    {
        isActive = false;
        foreach (var obj in controlledObjects)
        {
            if (obj != null) obj.SetActive(false);
        }
        // (필요하다면 여기에 레버 애니메이션/스프라이트 변경 코드 추가)
    }

    // LeverManager에 의해 강제로 비활성화될 때 호출됨
    public void ForceDeactivate()
    {
        // 매니저가 이미 리스트에서 제거했으므로, 
        // 여기서는 매니저에게 다시 알릴(RegisterDeactivation) 필요가 없습니다.
        Deactivate();
    }
}
