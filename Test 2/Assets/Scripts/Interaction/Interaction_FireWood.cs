using UnityEngine;

public class Interaction_FireWood : Interaction_Obj
{
    public GameObject FireWood;
    public GameObject vector;

    public override void OnInteract(PlayerInputHandler PlayerInput)
    {
        base.OnInteract(PlayerInput);

        Instantiate(FireWood, vector.transform.position, Quaternion.identity);
    }
}
