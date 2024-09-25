using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A room where players can buy items based on what domain they are in. The shops can be robbed
/// </summary>
public class ShopRoom : BasicRoom
{
    public bool isBeingRobbed;
    [Tooltip("REFERNCE to the pedestals to store items")]
    public List<DemonShopPedestal> myPedestals = new List<DemonShopPedestal>();
    [Tooltip("REFERNCE to the chests behind the counter the player can rob")]
    public List<TreasureChest> myChests = new List<TreasureChest>();
    [Tooltip("REFERENCE to the spawn locations for guards that spawn when being robbed")]
    public List<Transform> guardSpawns = new List<Transform>();
    private void OnEnable()
    {
        SetUpPedestals();
    }

    public void SetUpPedestals()
    {
        for(int i=0;i<myPedestals.Count;i++)
        {
            myPedestals[i].myShop = this;
            myPedestals[i].SetItem(DemonShopManager.instance.currentSinItemList.itemTiers[myPedestals[i].itemTier].GetRandomItem(),4);
            myPedestals[i].ToggleVisibility(false);
        }
    }
    public void SetPedestalsInactive()
    {
        for (int i = 0; i < myPedestals.Count; i++)
        {
            myPedestals[i].ToggleVisibility(false);
        }
    }
    public void PurchaseItem(ItemData item_,Transform loc_)
    {
        if(item_.type==ItemData.ItemType.consumable)
        {
            if(CombatPlayerManager.instance.playerHotbar.AddItemToHotbar(item_))
            {
                return;
            }
            else
            {
                SpawnWorldItem(item_, loc_);
                return;
            }
        }
        else
        {
            SpawnWorldItem(item_, loc_);
        }
    }
    /// <summary>
    /// Used to spawn items the player doesnt need until after exiting the combat minigame such as upgrade materials
    /// </summary>
    public void SpawnWorldItem(ItemData item_, Transform loc_,int amount=1)
    {

     GameObject temp = LootManager.instance.GetWorldLootObject();
        temp.transform.position = loc_.position;
        temp.transform.rotation = new Quaternion(0, 0, 0, 0);
        LootItem lItem=new LootItem();
        lItem.name = item_.itemName;
        lItem.amount = amount;
        temp.GetComponent<LootWorldObject>().myItem = lItem;
        temp.SetActive(true);
    }
    public void RobTheShop()
    {
        isBeingRobbed = true;
        for (int i = 0; i < myPedestals.Count; i++)
        {
            myPedestals[i].SetBeingRobbed();
        }
        for (int i = 0; i < guardSpawns.Count; i++)
        {
            EnemyManager.instance.SpawnRandomEnemy(true, guardSpawns[i],null, DungeonManager.instance.GetEnemyLevel(),"Guards");
        }
    }
   
}
