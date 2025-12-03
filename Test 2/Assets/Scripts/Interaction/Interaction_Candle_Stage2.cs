using UnityEngine;

public class Interaction_Candle_Stage2 : Interaction_Obj
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] candleSprite;
    int count;

    public override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);
        count++;
        if(count >= candleSprite.Length)
        {
            count = 0;
        }
            spriteRenderer.sprite = candleSprite[count];
    }
}
