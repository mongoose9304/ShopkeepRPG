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
        }
    }
}
