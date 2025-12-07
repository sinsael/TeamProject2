using UnityEngine;

public class FireWood : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite sprite;
    public bool Active;

    Interaction_ArtCase First_Active;
    Interaction_ArtCase Second_Active;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        GameObject obj1 = GameObject.Find("ArtCase_Left");
        GameObject obj2 = GameObject.Find("ArtCase_Right");

        First_Active = obj1.GetComponent<Interaction_ArtCase>();
        Second_Active = obj2.GetComponent<Interaction_ArtCase>();
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
