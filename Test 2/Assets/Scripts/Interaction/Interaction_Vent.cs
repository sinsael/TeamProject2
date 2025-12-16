using UnityEngine;

public class Interaction_Vent : Interaction_Obj // 다음 스테이지로 넘어가기 위한 환풍구
{
    [SerializeField] private Collider2D ventCollider;

    private AltarUIManager altar;

    private void Awake()
    {
        if (ventCollider == null)
            ventCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        altar = AltarUIManager.Instance;
        //if (altar == null)
        //    altar = FindObjectOfType<AltarUIManager>(true);

        if (altar == null)
        {
            if (ventCollider != null)
                ventCollider.enabled = false;
            return;
        }

        altar.OnAltarCompleteChanged += HandleAltarCompleteChanged;
        HandleAltarCompleteChanged(altar.IsAltarComplete());
    }

    private void OnDisable()
    {
        if (altar != null)
            altar.OnAltarCompleteChanged -= HandleAltarCompleteChanged;
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
