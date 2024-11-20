using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryItem
{
    public string myItemName;
    public int amount;
}
[System.Serializable]
public class MoveableItem
{
    public string myItemName;
    public int amount;
}
[System.Serializable]
public class InventoryItemList
{
    public List<InventoryItem> myList = new List<InventoryItem>();
}

public class PlayerInventory : MonoBehaviour
{
    public  List<ItemData> allItems = new List<ItemData>();
    public List<InventoryItem> masterItemList = new List<InventoryItem>();
    public List<MoveableItem> masterMoveableItemList = new List<MoveableItem>();
    public static PlayerInventory instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            LoadItems();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UpdateItems(List<InventorySlot> items_)
    {
        Debug.Log("UpdateItems");
        foreach (InventorySlot slot_ in items_)
        {
            if (slot_.myItem)
            {
              bool hasFoundItem = false;
                foreach (InventoryItem masterItem_ in masterItemList)
                {

                    if (masterItem_.myItemName == slot_.myItem.itemName)
                    {
                        masterItem_.amount = slot_.amount;
                        hasFoundItem = true;
                        break;
                    }
                }
                if(!hasFoundItem)
                {
                    MoveableItem itemX = new MoveableItem();
                    itemX.myItemName = slot_.myItem.itemName;
                    itemX.amount = slot_.amount;
                    masterMoveableItemList.Add(itemX);
                }
            }
        }
    }
    public void UpdateMoveableItems(List<InventorySlot> items_)
    {
        foreach (InventorySlot slot_ in items_)
        {
            if (slot_.myMoveableObject)
            {
                bool hasFoundItem = false;
                foreach (MoveableItem masterItem_ in masterMoveableItemList)
                {

                    if (masterItem_.myItemName == slot_.myMoveableObject.myName)
                    {
                        masterItem_.amount = slot_.amount;
                        hasFoundItem = true;
                        break;
                    }
                }
                if (!hasFoundItem)
                {
                    MoveableItem itemX = new MoveableItem();
                    itemX.myItemName = slot_.myMoveableObject.myName;
                    itemX.amount = slot_.amount;
                    masterMoveableItemList.Add(itemX);
                }
            }
        }
    }
    public void SaveItems()
    {
        FileHandler.SaveToJSON(masterItemList,"PlayerInventory");
        SaveMoveableItems();
    }
    private void SaveMoveableItems()
    {
        FileHandler.SaveToJSON(masterMoveableItemList, "PlayerMoveableInventory");
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
        LoadMoveableItems();
    }
    private void LoadMoveableItems()
    {
        List<MoveableItem> masterItemList_ = FileHandler.ReadListFromJSON<MoveableItem>("PlayerMoveableInventory");
        if (masterItemList_ != null)
        {
            if (masterItemList_.Count == 0)
                return;
            masterMoveableItemList = masterItemList_;
        }
    }
    public ItemData GetItem(string name_)
    {
        foreach(ItemData data in allItems)
        {
            if (data.itemName == name_)
                return data;
        }
        return null;
    }
}
