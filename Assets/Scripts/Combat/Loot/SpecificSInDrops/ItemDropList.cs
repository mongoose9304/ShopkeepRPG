using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemDropList", order = 1)]
[System.Serializable]
public class TieredItemList
{
    public List<LootTableItem> myTable=new List<LootTableItem>();
}

public class ItemDropList : ScriptableObject
{
    public List<TieredItemList> myTable = new List<TieredItemList>();
    public string itemListName;
}
