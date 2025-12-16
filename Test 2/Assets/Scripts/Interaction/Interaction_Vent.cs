using UnityEngine;

// 다음 스테이지로 넘어가기 위한 환풍구
public class Interaction_Vent : Interaction_Obj
{
    [SerializeField] private Collider2D ventCollider;

    private AltarUIManager altar;
    private bool CompletEvent;

    private void Awake()
    {
        // 콜라이더가 자식에 있을 수도 있어서 자식까지 포함해서 찾기
        if (ventCollider == null)
            ventCollider = GetComponent<Collider2D>();

        if (ventCollider != null)
            ventCollider.enabled = false;
    }

    public override void Start()
    {
        base.Start();

        TrySubscribeAltar();
    }


    private void OnDisable()
    {
        UnsubscribeAltar();
    }

    private void TrySubscribeAltar()
    {
        altar = AltarUIManager.Instance;
        if (altar == null)
            return;

        if (CompletEvent)
            return;

        altar.OnAltarCompleteChanged += HandleAltarCompleteChanged;
        CompletEvent = true;

        HandleAltarCompleteChanged(altar.IsAltarComplete());
    }

    private void UnsubscribeAltar()
    {
        if (!CompletEvent)
            return;

        if (altar != null)
            altar.OnAltarCompleteChanged -= HandleAltarCompleteChanged;

        CompletEvent = false;
        altar = null;
    }

    private void HandleAltarCompleteChanged(bool complete)
    {
        if (ventCollider != null)
            ventCollider.enabled = complete;
    }

    public override void OnInteract(PlayerInputHandler playerInput)
    {
        base.OnInteract(playerInput);
        GameManager.Instance.ChangeGameState(GameState.GameClear);
    }
}
