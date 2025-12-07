using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData/BookItem")]
public class BookData : ItemData
{
    public override void Use()
    {
        base.Use();
        UIManager.Instance.SetActiveBookUI();
    }
}
