using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction_Candle_Stage2 : Interaction_Obj
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] candleSprite;
    [SerializeField] Interaction_BookCase_Right IBR;

    [Header("Candle Settings")]
    public int candleIndex;
    private bool isCandleOn = false;

    public override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (IBR == null)
            IBR = FindFirstObjectByType<Interaction_BookCase_Right>();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {

        isCandleOn = true;
        UpdateVisual();
    }

    public void ForceTurnOff()
    {
        isCandleOn = false;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (candleSprite != null && candleSprite.Length >= 2)
        {
            spriteRenderer.sprite = isCandleOn ? candleSprite[1] : candleSprite[0];
        }
    }
}