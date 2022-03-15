using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Drops/Items")]
public class ItemContainer : ScriptableObject
{
    public string itemName;
    public string description;
    public int durability;
    public Sprite sprite;
}
