using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData/Interaction_Run")]
public class Interaction_Runes : ItemData
{
    public override void Use()
    {

        Debug.Log("∑È¿‘¥œµ¢ Ω««‡!!!!!!");


        Inventory.Instance.RemoveItem(this);
    }
}
