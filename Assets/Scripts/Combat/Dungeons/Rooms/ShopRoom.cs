using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoom : BasicRoom
{
    public List<DemonShopPedestal> myPedestals = new List<DemonShopPedestal>();

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
   
}
