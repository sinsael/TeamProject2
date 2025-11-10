using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "CreateItem/Item")]
public class Item : ScriptableObject
{
    [Header("")]
    public TileBase tilebase;
    public Sprite image;
    public ItemType itemType;
    public Vector2Int Range = new Vector2Int(1,1);
    public bool isStackable;
}
