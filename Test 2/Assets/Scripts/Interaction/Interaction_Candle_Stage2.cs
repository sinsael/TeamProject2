using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction_Candle_Stage2 : Interaction_Obj
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] candleSprite;
    [SerializeField]Interaction_BookCase_Right IBR;

    [Header("Candle Settings")]
    public int candleIndex; // 이 양초가 몇 번째 양초인지 (0, 1, 2, 3) 설정해줘야 함
    private bool isCandleOn = false;

    public override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        
        IBR = FindFirstObjectByType<Interaction_BookCase_Right>();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        // 퍼즐이 진행 중일 때만 작동
        if (IBR != null && IBR.isActive && !IBR.isPuzzleSolved)
        {
            base.OnInteract(PlayerInput);

            // [중요] 책장에게 상호작용 시도 알림 (성공 여부 반환)
            // TryInteractCandle 내부에서 오답이면 알아서 리셋을 돌려버림
            bool success = IBR.TryInteractCandle(candleIndex);

            if (success)
            {
                // 성공(정답 칸임): 상태 변경
                isCandleOn = !isCandleOn;
                UpdateVisual();
            }
            else
            {
                // 실패(오답 칸임): 켜지지 않음 (책장이 이미 전체 리셋 명령을 내렸을 것임)
                // 혹시 모르니 꺼짐 상태 확실히 하기
                ForceTurnOff();
            }
        }
    }

    public void ForceTurnOff()
    {
        isCandleOn = false;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (candleSprite.Length >= 2)
        {
            spriteRenderer.sprite = isCandleOn ? candleSprite[1] : candleSprite[0];
        }
    }
}
