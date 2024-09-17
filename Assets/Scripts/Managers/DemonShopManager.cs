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
}
