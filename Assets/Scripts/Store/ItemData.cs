using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Accessory,
        Material,
        Treasure,
        Trash,
        consumable
    }

    [Range(-1, 1)]
    public int WarmFactor;
    [Range(-1, 1)]
    public int OccultFactor;
    [Range(-1, 1)]
    public int LivingFactor;
    [Range(-1, 1)]
    public int ViolentFactor;
    [Range(-1, 1)]
    public int GrossFactor;

    public string itemName;
    public string description;
    public int basePrice;
    public float consumeHealthValue;
    public float consumeManaValue;
    public Sprite itemSprite;
    public Color itemColor;
    public ItemType type;
}
