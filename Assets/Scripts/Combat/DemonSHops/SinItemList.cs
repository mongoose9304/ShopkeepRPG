using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A list of all the items that can be bought in a domain's shop. They are sorted into tiers 
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SinItemList", order = 1)]
public class SinItemList : ScriptableObject
{
    public List<SinTieredPurchaseableItemList> itemTiers;
    public string sinID;
}
[System.Serializable]
public class SinTieredPurchaseableItemList
{
    public List<ItemData> myItems;
    public ItemData GetRandomItem()
    {
        return myItems[Random.Range(0, myItems.Count)];
    }
}
