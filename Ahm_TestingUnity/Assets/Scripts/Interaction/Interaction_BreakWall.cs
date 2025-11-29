using UnityEngine;

public class Interaction_BreakWall : Interaction_Obj
{
    public enum ActivateMode // 무슨 모드냐 (2P 가 활성화, 촛불 클릭 활성화)
    {
        BySecondPlayer, // 2P 상호작용 활성화 모드
        ByCandle        // 인벤토리의 
    }

    [SerializeField] private ActivateMode activateMode = ActivateMode.BySecondPlayer; // 기본은 2P 상호작용 모드
    
    [Header("벽이 부서졌을 때 드러날 숨겨진 아이템")]
    [SerializeField] private GameObject hiddenItem; // 벽 뒤 아이템 지정


    public int maxHitCount = 5;             // 타격 카운트 (벽 체력)
    public Sprite[] breakSprites;           // 스프라이트 배열
    public SpriteRenderer wallRenderer;     // 벽 렌더러

    private int currentCount = 0;           // 타격 받은 횟수    
    private bool isBreakableDiscovered = false; // 활성화 여부
    private bool isBroken = false;          // 이미 파괴됨
    private BoxCollider2D col;

    public override void Start()
    {
        base.Start();

        wallRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();

        if (hiddenItem != null) // 아이템 비활성화 아니면 꺼두기
            hiddenItem.SetActive(false);
 
    }

    public override void OnInteract(PlayerInputHandler interactor) // 플레이어 정보 받아오기
    {
        if (interactor == null) 
            return;

        if (isBroken) // 이미 파괴된 상태라면 무시
        {
            return;
        }

        // 2P 벽을 활성화 하기 전
        if (!isBreakableDiscovered)
        {
            if (activateMode == ActivateMode.BySecondPlayer) // 2P 상호작용 모드 일 때
            {
                // 2P만 발견 가능
                if (interactor is Second_PlayerInputHandler)
                {
                    isBreakableDiscovered = true;
                    sr.enabled = true;
                    wallRenderer.enabled = true;
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
                // 인벤토리에서 현재 선택된 아이템 가져오기
                ItemData selectedItem = null;

                if (Inventory.Instance != null)
                {
                    selectedItem = Inventory.Instance.GetSelectedItem();
                }

                // 선택된 아이템이 블랙 촛불인지 확인
                if (selectedItem is Interaction_BlackCandle)
                {
                    // 블랙 촛불이 선택된 상태이므로 벽 활성화
                    ActivateByCandle();
                }
                else
                {
                    Debug.Log("블랙 촛불이 선택되어 있어야 벽을 활성화할 수 있습니다.");
                }

                // 촛불 모드는 여기서 처리 끝
                return;
            }

            return;
        }

        if (interactor is First_PlayerInputHandler) // 1P 가 상호작용 할 때만 타격
        {
            HitWall();
        }
    }


    public void ActivateByCandle()
    {
        if (isBreakableDiscovered)
            return;

        if (activateMode != ActivateMode.ByCandle)
            return;

        isBreakableDiscovered = true;

        if (sr != null)
        {
            sr.enabled = true;
            sr.color = Color.yellow;
        }

        if (wallRenderer != null)
        {
            wallRenderer.enabled = true;
        }

        Debug.Log("촛불 사용으로 벽 활성화");
    }


    private void UpdateBreakSprite()
    {
        if (breakSprites == null || breakSprites.Length == 0)
            return;

        int index = Mathf.Clamp(currentCount, 0, breakSprites.Length - 1);
        wallRenderer.sprite = breakSprites[index];
    }

    private void HitWall()
    {
        currentCount++;

        int remaining = maxHitCount - currentCount;
        Debug.Log("벽 타격! (" + currentCount + "/" + maxHitCount + "), 남은 횟수: " + remaining);

        UpdateBreakSprite();

        if (currentCount >= maxHitCount)
        {
            Debug.Log("벽 완전히 파괴됨!");
            isBroken = true;
            col.enabled = false;

            // 벽이 파괴될때 아이템 드러내기
            if (hiddenItem != null)
            {
                // 부모(벽)가 사라져도 아이템이 살아있게 하려면 부모 분리
                hiddenItem.transform.SetParent(null);

                // 벽 한가운데 놔두고
                hiddenItem.transform.position = transform.position;                                                   

                // 활성화 시키기
                hiddenItem.SetActive(true);
            }
        }
    }
}
