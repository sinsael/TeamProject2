using UnityEngine;

public class Interaction_BookCase : Interaction_Obj
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprite = null;
    public int totalcount; // 총 갯수
    public int count = 0; // 현재 갯수
    public int dropcount; // 드랍 카운트

    public bool drop = false; // 드랍 여부

    public GameObject DropItem; // 드랍 아이템
    
    public GameObject dropPosition; // 드랍 위치


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
            // 아이템 드랍
            Instantiate(DropItem, dropPosition.transform.position, Quaternion.identity);
            drop = true;
            return;
        }

        count++;

        if (count > sprite.Length)
        {
            count = 0;
        }

        spriteRenderer.sprite = sprite[count];

    }

}
