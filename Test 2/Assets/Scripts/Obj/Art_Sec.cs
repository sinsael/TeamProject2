using UnityEngine;

public class Art_Sec : MonoBehaviour
{
    public bool isCollected = false;
    public ItemData data = null;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected && collision.CompareTag("Player"))
        {
            Inventory.Instance.AddList(data);
            isCollected = true;
            gameObject.SetActive(false);
        }

    }
}
