using UnityEngine;

public class Interaction_BookCase : Interaction_Obj
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprite = null;
    public int totalcount;
    public int count = 0;
    public int dropcount;

    public bool drop = false;

    public GameObject DropItem;

    public Vector2 dropPosition;


    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        if(drop)
        {
            return;
        }

        if(count == dropcount)
        {
            dropPosition = transform.position;
            Instantiate(DropItem, dropPosition, Quaternion.identity);
            drop = true;
            return;
        }

        count++;

        if (count >= sprite.Length)
        {
            count = 0;
        }

        spriteRenderer.sprite = sprite[count];

    }

}
