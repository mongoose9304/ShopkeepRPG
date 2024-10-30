using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryItem
{
    public ItemData myItem;
    public int amount;
}
public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> masterItemList = new List<InventoryItem>();
    public static PlayerInventory instance;
    private void Awake()
    {
        instance = this;
    }
}
