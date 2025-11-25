using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemSprite;
    public ItemType itemType;
    public enum ItemType
    {
        Book,
        Wood
    }
}
