using UnityEngine;

public class FireWood : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite sprite;
    public bool Active;

    [SerializeField] Interaction_ArtCase First_Active;
    [SerializeField] Interaction_ArtCase Second_Active;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(First_Active.isCompleted && Second_Active.isCompleted)
        {
            sr.sprite = sprite;
            Active = true;
        }
    }
}
