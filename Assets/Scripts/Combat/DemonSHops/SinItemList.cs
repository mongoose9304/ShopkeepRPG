using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Basic stats of a Player or familiar
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
