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
        LoadItems();
    }
    public void UpdateItems(List<InventorySlot> items_)
    {
        Debug.Log("UpdateItems");
        foreach (InventorySlot slot_ in items_)
        {
            if (slot_.myItem)
            {
                foreach (InventoryItem masterItem_ in masterItemList)
                {

                    if (masterItem_.myItem.itemName == slot_.myItem.itemName)
                    {
                        masterItem_.amount = slot_.amount;
                        Debug.Log("FoundItem");
                    }
                }
            }
        }
    }
    public void SaveItems()
    {
        FileHandler.SaveToJSON(masterItemList,"PlayerInventory");
    }
    public void LoadItems()
    {
        List<InventoryItem> masterItemList_ = FileHandler.ReadListFromJSON<InventoryItem>("PlayerInventory");
        if(masterItemList_!=null)
        {
            if (masterItemList_.Count == 0)
                return;
            masterItemList = masterItemList_;
        }
    }
}
