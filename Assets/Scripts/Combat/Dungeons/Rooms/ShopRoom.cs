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
            myPedestals[i].SetItem(DemonShopManager.instance.currentSinItemList.itemTiers[myPedestals[i].itemTier].GetRandomItem());
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
   
}
