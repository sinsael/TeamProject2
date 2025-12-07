using UnityEngine;

public class Chandelier : MonoBehaviour
{
    Collider2D col;
    Rigidbody2D rb;
    FireWood FireActive;
    public bool isFallen = false;
    public GameObject Key;
    public GameObject KeyPosition;
    private void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        col.isTrigger = true;
    }

    private void Update()
    {
        if (isFallen) return;

        if (FireActive == null)
        {
            FireActive = FindAnyObjectByType<FireWood>();

            if (FireActive == null) return;
        }

        if (FireActive.Active)
        {
            rb.gravityScale = 1f;
            col.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFallen) return;

        if(collision.gameObject.layer.Equals(7))
        {
            Instantiate(Key, KeyPosition.transform.position, Quaternion.identity);
            isFallen = true;
        }
    }
}
