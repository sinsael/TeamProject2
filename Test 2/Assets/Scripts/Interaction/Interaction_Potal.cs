using UnityEngine;
using System.Collections;

public class Interaction_Potal : Interaction_Obj
{
    public SpriteRenderer potal;
    public ItemData key;

    [Header("카메라 흔들림 설정")]
    [SerializeField] private bool shakeBoth = true; // ture 면 모든 카메라 흔들림, false 면 아이디로 지정된 카메라만 흔듦
    [SerializeField] private int shakeCameraId = 0; // 흔들리는 카메라 아이디 (0은 1P 카메라, 1은 2P 카메라)(둘다 흔들고 싶으면 위에 있는 shakeBoth 를 true 로)
                                                    // 하나만 흔들때는 ShakeOne 함수 사용 (Id , intensity, time)
    [SerializeField] private float shakeIntensity = 2f; // 흔들림의 세기
    [SerializeField] private float shakeTime = 1f; // 흔들림의 지속 시간

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);



        if (Inventory.Instance.HasItem(key))
        {
            potal.enabled = false;
            Inventory.Instance.RemoveItem(key);


            StartCoroutine(PotalActivationSequence());
        }
    }

    private IEnumerator PotalActivationSequence()
    {
        // 1. 카메라 흔들림 시작
        // 이 메서드(ShakeAll)가 코루틴을 시작하거나 직접 흔들림을 처리한다고 가정합니다.
        CameraShake.ShakeAll(shakeIntensity, shakeTime);
        // 2. 흔들림 시간만큼 대기
        // shakeTime이 0이 아닐 때만 대기합니다.
        if (shakeTime > 0)
        {
            yield return new WaitForSeconds(shakeTime);
        }

        // 3. (옵션) 흔들림이 완전히 끝난 후 추가 프레임 대기
        // 카메라가 제자리로 돌아오는 등의 미세한 딜레이를 위해 추가할 수 있습니다.
        // yield return null; 

        // 4. 게임 클리어 상태로 변경 (흔들림이 끝난 후 실행)
        GameManager.Instance.ChangeGameState(GameState.GameClear);
    }
}
