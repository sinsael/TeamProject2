using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;
  
    //아이템 사용법
    public virtual void Use()
    {
        Debug.Log("실행");
    }
}
