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

    public GameObject dropPosition;


    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Start()
    {
        base.Start();
        spriteRenderer.sprite = sprite[0];
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
            Instantiate(DropItem, dropPosition.transform.position, Quaternion.identity);
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
