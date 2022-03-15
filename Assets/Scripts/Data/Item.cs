using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item{
    public string itemName;
    public string description;
    public int quantity;
    public Raritys rarity;
    public string iconSpriteName;
    public Item(string name = null, string _description = null, int _durability = 1, Raritys _rarity = Raritys.COMMON, string _icon = null){
        itemName = name;
        description = _description;
        quantity = _durability;
        rarity = _rarity;
        iconSpriteName = _icon;
    }
}
public enum Raritys{
    COMMON,
    UNCOMMON,
    RARE,
    VERY_RARE,
    UNIQUE
}
