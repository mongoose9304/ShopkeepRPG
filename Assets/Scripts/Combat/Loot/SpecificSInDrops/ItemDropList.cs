using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemDropList", order = 1)]
[System.Serializable]
public class TieredItemList
{
    public List<LootTableItem> myTable=new List<LootTableItem>();
    public LootTableItem GetRandomItem()
    {
        return myTable[Random.Range(0, myTable.Count)];
    }
}

public class ItemDropList : ScriptableObject
{
    public List<TieredItemList> myTable = new List<TieredItemList>();
    public string itemListName;
}
