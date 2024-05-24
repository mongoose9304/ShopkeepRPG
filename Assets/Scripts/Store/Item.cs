using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class ItemData : ScriptableObject{
    public enum ItemType
    {
        Weapon,
        Armor,
        Accessory,
        Material,
        Treasure,
        Trash
    }

    public string itemName;
    public string description;
    public int basePrice;
    public ItemType type;
}

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public void SetItem(ItemData item_){
        itemData = item_;
    }

    public ItemData GetItem(){
        return itemData; 
    }
   
}
