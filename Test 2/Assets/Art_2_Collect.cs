using UnityEngine;

public class Art_2_Collect : MonoBehaviour
{
    public ItemData id;
    Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory.Instance.AddList(id);
        col.enabled = false;
    }
}
