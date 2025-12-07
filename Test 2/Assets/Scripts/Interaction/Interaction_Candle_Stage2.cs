using UnityEngine;

public class Interaction_Candle_Stage2 : Interaction_Obj
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] candleSprite = null;
    [SerializeField] Interaction_BookCase_Right IBR;
    [SerializeField] Collider2D col;
    public Color myCandleColor = Color.white;

    public override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (IBR == null)
            IBR = FindFirstObjectByType<Interaction_BookCase_Right>();


        ResetCandle();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        if (IBR != null && IBR.isActive && IBR.isSequence)
        {
            // 2. 책장에게 "이 색깔 맞아?" 라고 물어봄 (채점 요청)
            bool isCorrect = IBR.CheckPuzzleAnswer(myCandleColor);

            if (isCorrect)
            {
                // 정답이면 불을 켬
                base.OnInteract(PlayerInput); // 효과음 등
                LightOn();
            }
            else
            {
                // 틀리면 책장이 알아서 닫히므로 촛불은 할 게 없음 (로그만 출력)
                Debug.Log("틀렸습니다! 초기화됩니다.");
            }
        }
    }
    // 모든 양초 초기화
    void LightOn()
    {
        if (candleSprite != null && candleSprite.Length > 1)
        {
            spriteRenderer.sprite = candleSprite[1]; // 켜진 이미지
            col.enabled = false; // 상호작용 콜라이더 비활성화

        }
    }

    // 불 끄기 (초기화)
    public void ResetCandle()
    {
        if (candleSprite != null && candleSprite.Length > 0)
        {
            spriteRenderer.sprite = candleSprite[0]; // 꺼진 이미지
        }
    }
}