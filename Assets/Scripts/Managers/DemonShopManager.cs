using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class DemonShopManager : MonoBehaviour
{
    public static DemonShopManager instance;
    public List<SinItemList> mySinPurchaseableItems;
    public SinItemList currentSinItemList;

    private void Awake()
    {
        instance = this;
    }
    public void SwitchSinSrops(string name_)
    {
        foreach (SinItemList list_ in mySinPurchaseableItems)
        {
            if (name_ == list_.sinID)
            {
                currentSinItemList = list_;
            }
        }

    }
}
