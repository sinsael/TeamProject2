using UnityEngine;

public class Interaction_BreakWall : Interaction_Obj
{
    public enum ActivateMode // 무슨 모드냐 (2P 가 활성화, 촛불 클릭 활성화)
    {
        BySecondPlayer, // 2P 상호작용 활성화 모드
        ByCandle        // 촛불 클릭 활성화 모드 
    }

    [SerializeField] private ActivateMode activateMode = ActivateMode.BySecondPlayer; // 기본은 2P 상호작용 모드
    
    [Header("벽이 부서졌을 때 드러날 숨겨진 아이템")]
    [SerializeField] private GameObject hiddenItem; // 벽 뒤 아이템 지정

    public int maxHitCount = 5;             // 타격 카운트 (벽 체력)
    public Sprite[] breakSprites;           // 스프라이트 배열
    public SpriteRenderer wallRenderer;     // 벽 렌더러

    private int currentCount = 0;           // 타격 받은 횟수    
    private bool isBreakableDiscovered = false; // 활성화 여부

    public override void Start()
    {
        base.Start();
        if (wallRenderer == null)
            wallRenderer = GetComponent<SpriteRenderer>();

        if (hiddenItem != null) // 아이템 비활성화 아니면 꺼두기
        {
            hiddenItem.SetActive(false);
        }
    }

    public override void OnInteract(PlayerInputHandler interactor) // 플레이어 정보 받아오기
    {
        if (interactor == null) 
            return;

        // 2P 벽을 활성화 하기 전
        if (!isBreakableDiscovered)
        {
            if (activateMode == ActivateMode.BySecondPlayer) // 2P 상호작용 모드 일 때
            {
                // 2P만 발견 가능
                if (interactor is Second_PlayerInputHandler)
                {
                    isBreakableDiscovered = true;
                    if (sr != null) sr.color = Color.yellow;
                    Debug.Log("2P 상호작용");
                }
                else
                {
                    Debug.Log("2P 가 먼가 알지도 모른다");
                }
            }
            else if (activateMode == ActivateMode.ByCandle) // 촛불 상호작용 모드 일 때
            {
                Debug.Log("검은 촛불이 먼가 수상하다");
            }

            return;
        }

        // 이미 활성화된 상태라면 1P / 2P 모두 파괴 가능
        currentCount++;

        int remaining = maxHitCount - currentCount;
        Debug.Log("벽 타격! (" + currentCount + "/" + maxHitCount + "), 남은 횟수: " + remaining);

        UpdateBreakSprite(); 

        if (currentCount >= maxHitCount)
        {
            Debug.Log("벽 완전히 파괴됨!");
            Destroy(gameObject);

            // 이 타이밍에 아이템을 드러내기
            if (hiddenItem != null)
            {
                // 부모(벽)가 사라져도 아이템이 살아있게 하려면 부모 분리
                hiddenItem.transform.SetParent(null);

                // 원하는 위치로 옮기고
                hiddenItem.transform.position = transform.position; // 벽 위치
                                                                    // 또는 미리 맞춰놨다면 생략

                // 활성화
                hiddenItem.SetActive(true);
            }
        }
    }


    public void ActivateByCandle() // 촛불 상호작용과 연결 (CandleActivator.cs)
    {
        if (isBreakableDiscovered) // 벽 활성화 시 무시
            return;

        if (activateMode != ActivateMode.ByCandle) // 벽이 촛불 방식이 아니라면 무시
        {
            return;
        }

        isBreakableDiscovered = true;
        if (sr != null) sr.color = Color.yellow; // 부모의 색깔 사용
        Debug.Log("촛불로 벽이 활성화되었다."); 
    }

    private void UpdateBreakSprite()
    {
        if (breakSprites == null || breakSprites.Length == 0)
            return;

        int index = Mathf.Clamp(currentCount, 0, breakSprites.Length - 1);
        wallRenderer.sprite = breakSprites[index];
    }
}
