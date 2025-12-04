using UnityEngine;

public class WallObject : MonoBehaviour
{
    public Collider2D Collider;
    public ItemData art;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider = GetComponent<Collider2D>();

        Collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Inventory.Instance.HasItem(art))
        {
            Collider.enabled = true;
        }
        else
        {
            Collider.enabled = false;
        }
    }
}
