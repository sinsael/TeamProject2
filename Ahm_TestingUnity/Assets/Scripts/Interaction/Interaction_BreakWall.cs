using UnityEngine;

public class Interaction_BreakWall : Interaction_Obj
{
    public enum ActivateMode // 무슨 모드냐 (2P 가 활성화, 촛불 클릭 활성화)
    {
        BySecondPlayer, // 2P 상호작용 활성화 모드
        ByCandle        // 인벤토리의 
    }

    [Header("첫 발견자 모드 (2P, 촛불)")]
    [SerializeField] private ActivateMode activateMode = ActivateMode.BySecondPlayer; // 기본은 2P 상호작용 모드
    
    [Header("벽이 부서졌을 때 드러날 숨겨진 아이템")]
    [SerializeField] private GameObject hiddenItem; // 벽 뒤 아이템 지정

    [Header("발견 시 표시되는 스프라이트")]
    [SerializeField] private Sprite discoveredSprite; // 발견되었을 때 먼저 보여줄 이미지 1장

    [Header("벽 타격 관련 설정")]
    public int maxHitCount = 5;             // 타격 카운트 (벽 체력)
    public Sprite[] breakSprites;           // 스프라이트 배열

    [Header("벽 파괴 시 파티클")]
    [SerializeField] private BreakVfxSpawner vfx;
    [SerializeField] private Transform vfxPoint; // optional

    private bool requireBookPile = true; // 요걸 트루로 만들어서 책 더미로 기믹 활성화 필요하게 만들기
    private static bool wallGimmickUnlocked = false;      // 책 더미로 기믹 활성화 여부

    private SpriteRenderer wallRenderer;     // 벽 렌더러

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

        // 시작 시에는 발견 전이므로 숨김 처리
        if (!isBreakableDiscovered)
        {
            if (sr != null) sr.enabled = false;
            if (wallRenderer != null) wallRenderer.enabled = false;
        }

    }

    public override void OnInteract(PlayerInputHandler interactor) // 플레이어 정보 받아오기
    {
        if (interactor == null) 
            return;

        if (isBroken) // 이미 파괴된 상태라면 무시
        {
            return;
        }

        if (requireBookPile && !wallGimmickUnlocked)
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
                    ShowDiscoveredVisual();
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

    // 촛불(아이템) 선택 후 벽 상호작용으로 벽 활성화 
    public void ActivateByCandle()
    {
        if (isBreakableDiscovered)
            return;

        if (activateMode != ActivateMode.ByCandle)
            return;

        isBreakableDiscovered = true;

        // 촛불로 발견해도 반드시 "발견 스프라이트"를 먼저 보여줘야 함
        ShowDiscoveredVisual();

        Debug.Log("촛불 선택으로 벽 발견");
    }

    private void ShowDiscoveredVisual()
    {
        if (sr != null) sr.enabled = true;
        if (wallRenderer != null) wallRenderer.enabled = true;

        // 발견되면 먼저 '발견 스프라이트'로 교체
        if (wallRenderer != null && discoveredSprite != null)
        {
            wallRenderer.sprite = discoveredSprite;
        }

        // 발견 연출 색(원하면 제거 가능)
        if (sr != null) sr.color = Color.yellow;
    }

    // 타격 시 벽 스프라이트 진행
    private void UpdateBreakSprite()
    {
        if (breakSprites == null || breakSprites.Length == 0)
            return;

        if (wallRenderer == null)
            return;

        // 1타: breakSprites[0], 2타: breakSprites[1] ... 되도록 -1
        int index = Mathf.Clamp(currentCount - 1, 0, breakSprites.Length - 1);
        wallRenderer.sprite = breakSprites[index];
    }


    //벽 때리기
    private void HitWall()
    {
        currentCount++;

        int remaining = maxHitCount - currentCount;
        Debug.Log("벽 타격! (" + currentCount + "/" + maxHitCount + "), 남은 횟수: " + remaining);

        CameraShake.instance.ShakeCamera(2f, 0.2f);

        Vector3 p = (vfxPoint != null) ? vfxPoint.position : transform.position;
        if (vfx != null) vfx.SpawnDebris(p);

        UpdateBreakSprite();

        if (currentCount >= maxHitCount)
        {
            Debug.Log("벽 완전히 파괴됨!");

            isBroken = true;

            if (col != null)
                col.enabled = false;

            // 벽이 파괴될 때 아이템 드러내기
            if (hiddenItem != null)
            {
                hiddenItem.transform.SetParent(null);
                hiddenItem.transform.position = transform.position;
                hiddenItem.SetActive(true);
            }
        }
    }

    // 책 더미로 기믹 활성화
    public static void UnlockWallGimmickByBookPile()
    {
        wallGimmickUnlocked = true;
    }
}
