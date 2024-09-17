using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Basic stats of a Player or familiar
/// </summary>
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SinItemList", order = 1)]
public class SinItemList : MonoBehaviour
{
    public List<SinTieredPurchaseableItemList> itemTiers;
    public string sinID;
}
[System.Serializable]
public class SinTieredPurchaseableItemList
{
    public string sinID;
    public List<ItemData> myItems;
}
