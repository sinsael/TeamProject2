using UnityEngine;

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


            CameraShake.ShakeAll(shakeIntensity, shakeTime);

        }
    }
}
