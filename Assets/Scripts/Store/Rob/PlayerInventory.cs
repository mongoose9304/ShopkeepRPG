using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
/// <summary>
/// A container for an item and the amount held
/// </summary>
public class InventoryItem
{
    public string myItemName;
    public int amount;
}
[System.Serializable]
/// <summary>
/// A container for an item and the amount held. Moveable items are saved here to keep them seperate from the normal items
/// </summary>
public class MoveableItem
{
    public string myItemName;
    public int amount;
}
[System.Serializable]
/// <summary>
/// Stores a list of inventory items
/// </summary>
public class InventoryItemList
{
    public List<InventoryItem> myList = new List<InventoryItem>();
}
/// <summary>
/// The singleton that should persist between all scens to save the player's items
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    [Tooltip("All the items in the game. Used to look up items and load them later")]
    public  List<ItemData> allItems = new List<ItemData>();
    [Tooltip("the items the player currently has")]
    public List<InventoryItem> masterItemList = new List<InventoryItem>();
    [Tooltip("the moveable items the player has")]
    public List<MoveableItem> masterMoveableItemList = new List<MoveableItem>();
    [Tooltip("The singeton instance")]
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
    /// <summary>
    /// Add all the UI items to the master list
    /// </summary>
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
    /// <summary>
    /// all all the ui moveable objects to the master list
    /// </summary>
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
    /// <summary>
    /// Save the items to a Json
    /// </summary>
    public void SaveItems()
    {
        FileHandler.SaveToJSON(masterItemList,"PlayerInventory");
        SaveMoveableItems();
    }
    /// <summary>
    /// Save the moveable to a Json
    /// </summary>
    private void SaveMoveableItems()
    {
        FileHandler.SaveToJSON(masterMoveableItemList, "PlayerMoveableInventory");
    }
    /// <summary>
    /// Load all items from a Json, if none are found use whats in the inspector
    /// </summary>
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
    /// <summary>
    /// Load all moveable items from a Json, if none are found use whats in the inspector
    /// </summary>
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
    /// <summary>
    /// get Temdata from an items name by searching the item index of all items
    /// </summary>
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
