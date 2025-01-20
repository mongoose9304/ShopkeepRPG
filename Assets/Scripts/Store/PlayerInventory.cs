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
/// The singleton that should persist between all scens to save the player's items and stats 
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
    [SerializeField] int woodTotal;
    [SerializeField] int stoneTotal;
    [SerializeField] int humanCashTotal;
    [SerializeField] int hellCashTotal;
    public StatBlock playerStats;
    public StatBlock familiarStats;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            LoadItems();
            LoadAllResources();
            LoadPlayerStats();
            LoadFamiliarStats();
            DontDestroyOnLoad(gameObject);
        }
        else if(instance!=this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Updates the master list to reflect the UI items, replacement NOT addition
    /// </summary>
    public void UpdateItems(List<InventorySlot> items_)
    {
        Debug.Log("UpdateItems");
        foreach (InventorySlot slot_ in items_)
        {
            if (slot_.myItem)
            {
              bool hasFoundItem = false;
                bool itemExists = false;
                foreach (InventoryItem masterItem_ in masterItemList)
                {

                    if (masterItem_.myItemName == slot_.myItem.itemName)
                    {
                        masterItem_.amount = slot_.amount;
                        hasFoundItem = true;
                        break;
                    }
                }
                foreach (ItemData item_ in allItems)
                {
                    if (item_.itemName == slot_.myItem.itemName)
                    {
                        itemExists = true;
                        break;
                    }
                }
                if (!hasFoundItem&&itemExists)
                {
                    InventoryItem itemX = new InventoryItem();
                    itemX.myItemName = slot_.myItem.itemName;
                    itemX.amount = slot_.amount;
                    masterItemList.Add(itemX);
                }
            }
        }
    }
    public void AddItemToInventory(string name_,int amount_)
    {
        bool hasFoundItem = false;
        bool itemExists = false;
        foreach (InventoryItem masterItem_ in masterItemList)
        {

            if (masterItem_.myItemName == name_)
            {
                masterItem_.amount += amount_;
                hasFoundItem = true;
                break;
            }
        }
        foreach(ItemData item_ in allItems)
        {
            if(item_.itemName==name_)
            {
                itemExists = true;
                break;
            }
        }
        if (!hasFoundItem&&itemExists)
        {
            InventoryItem itemX = new InventoryItem();
            itemX.myItemName = name_;
            itemX.amount = amount_;
            masterItemList.Add(itemX);
        }
    }
    public void ConsumeItem(string name_, int amount_)
    {
        foreach (InventoryItem masterItem_ in masterItemList)
        {

            if (masterItem_.myItemName == name_)
            {
                masterItem_.amount -= amount_;
                break;
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
    public void LoadPlayerStats()
    {
        if (!playerStats)
            return;
        if (PlayerPrefs.GetInt("PlayerLevel", 0) == 0)
            return;
        playerStats.Level= PlayerPrefs.GetInt("PlayerLevel", 0);
        playerStats.Vitality = PlayerPrefs.GetInt("PlayerVitality", 0);
        playerStats.Soul = PlayerPrefs.GetInt("PlayerSoul", 0);
        playerStats.PhysicalProwess = PlayerPrefs.GetInt("PlayerPhysicalProwess", 0);
        playerStats.MysticalProwess = PlayerPrefs.GetInt("PlayerMysticalProwess", 0);
        playerStats.PhysicalDefense = PlayerPrefs.GetInt("PlayerPhysicalDefense", 0);
        playerStats.MysticalDefense = PlayerPrefs.GetInt("PlayerMysticalDefense", 0);
        playerStats.Luck = PlayerPrefs.GetInt("PlayerLuck", 0);
        playerStats.savedExp = PlayerPrefs.GetInt("PlayerSavedExp", 0);
        playerStats.remainingSkillPoints = PlayerPrefs.GetInt("PlayerRemainingSkillPoints", 0);
        playerStats.totalSkillPoints = PlayerPrefs.GetInt("PlayerTotalSkillPoints", 0);
    }
    public void SavePlayerStats()
    {
        if (!playerStats)
            return;
        PlayerPrefs.SetInt("PlayerLevel", playerStats.Level);
        PlayerPrefs.SetInt("PlayerVitality", playerStats.Vitality);
        PlayerPrefs.SetInt("PlayerSoul", playerStats.Soul);
        PlayerPrefs.SetInt("PlayerPhysicalProwess", playerStats.PhysicalProwess);
        PlayerPrefs.SetInt("PlayerMysticalProwess", playerStats.MysticalProwess);
        PlayerPrefs.SetInt("PlayerPhysicalDefense", playerStats.PhysicalDefense);
        PlayerPrefs.SetInt("PlayerMysticalDefense", playerStats.MysticalDefense);
        PlayerPrefs.SetInt("PlayerLuck", playerStats.Luck);
        PlayerPrefs.SetInt("PlayerSavedExp", playerStats.savedExp);
        PlayerPrefs.SetInt("PlayerRemainingSkillPoints", playerStats.remainingSkillPoints);
        PlayerPrefs.SetInt("PlayerTotalSkillPoints", playerStats.totalSkillPoints);
    }
    public void LoadFamiliarStats()
    {
        if (!familiarStats)
            return;
        if (PlayerPrefs.GetInt("FamiliarLevel", 0) == 0)
            return;
        playerStats.Level = PlayerPrefs.GetInt("FamiliarLevel", 0);
        playerStats.Vitality = PlayerPrefs.GetInt("FamiliarVitality", 0);
        playerStats.Soul = PlayerPrefs.GetInt("FamiliarSoul", 0);
        playerStats.PhysicalProwess = PlayerPrefs.GetInt("FamiliarPhysicalProwess", 0);
        playerStats.MysticalProwess = PlayerPrefs.GetInt("FamiliarMysticalProwess", 0);
        playerStats.PhysicalDefense = PlayerPrefs.GetInt("FamiliarPhysicalDefense", 0);
        playerStats.MysticalDefense = PlayerPrefs.GetInt("FamiliarMysticalDefense", 0);
        playerStats.Luck = PlayerPrefs.GetInt("FamiliarLuck", 0);
        playerStats.savedExp = PlayerPrefs.GetInt("FamiliarSavedExp", 0);
        playerStats.remainingSkillPoints = PlayerPrefs.GetInt("FamiliarRemainingSkillPoints", 0);
        playerStats.totalSkillPoints = PlayerPrefs.GetInt("FamiliarTotalSkillPoints", 0);
    }
    public void SaveFamiliarStats()
    {
        if (!familiarStats)
            return;
        PlayerPrefs.SetInt("FamiliarLevel", familiarStats.Level);
        PlayerPrefs.SetInt("FamiliarVitality", familiarStats.Vitality);
        PlayerPrefs.SetInt("FamiliarSoul", familiarStats.Soul);
        PlayerPrefs.SetInt("FamiliarPhysicalProwess", familiarStats.PhysicalProwess);
        PlayerPrefs.SetInt("FamiliarMysticalProwess", familiarStats.MysticalProwess);
        PlayerPrefs.SetInt("FamiliarPhysicalDefense", familiarStats.PhysicalDefense);
        PlayerPrefs.SetInt("FamiliarMysticalDefense", familiarStats.MysticalDefense);
        PlayerPrefs.SetInt("FamiliarLuck", familiarStats.Luck);
        PlayerPrefs.SetInt("FamiliarSavedExp", familiarStats.savedExp);
        PlayerPrefs.SetInt("FamiliarRemainingSkillPoints", familiarStats.remainingSkillPoints);
        PlayerPrefs.SetInt("FamiliarTotalSkillPoints", familiarStats.totalSkillPoints);
    }
    public void LoadAllResources()
    {
        woodTotal = PlayerPrefs.GetInt("woodTotal", 0);
        stoneTotal = PlayerPrefs.GetInt("stoneTotal", 0);
        humanCashTotal = PlayerPrefs.GetInt("humanCashTotal", 0);
        hellCashTotal = PlayerPrefs.GetInt("hellCashTotal", 0);
    }
    public void SaveAllResources()
    {
        PlayerPrefs.SetInt("woodTotal", woodTotal);
        PlayerPrefs.SetInt("stoneTotal", stoneTotal);
        PlayerPrefs.SetInt("humanCashTotal", humanCashTotal);
        PlayerPrefs.SetInt("hellCashTotal", hellCashTotal);
    }
    public void AddWood(int amount_)
    {
        woodTotal += amount_;
    }
    public void AddStone(int amount_)
    {
        stoneTotal += amount_;
    }
    public void AddHumanCash(int amount_)
    {
        humanCashTotal += amount_;
    }
    public void AddHellCash(int amount_)
    {
        hellCashTotal += amount_;
    }
    public int GetWood() { return woodTotal; }
    public int GetStone() { return stoneTotal; }
    public int GetHumanCash() { return humanCashTotal; }
    public int GetHellCash() { return hellCashTotal; }
    public int GetItemAmount(string itemName_)
    {
        foreach(InventoryItem item_ in masterItemList)
        {
            if (item_.myItemName == itemName_)
                return item_.amount;
        }
        return 0;
    }
}
