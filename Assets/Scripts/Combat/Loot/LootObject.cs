using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootObject : MonoBehaviour
{
    protected List<LootItem> currentItems = new List<LootItem>();
    bool hasFoundItem;


    public void InitializeOject(List<LootTableItem> table_)
    {
        foreach(LootTableItem item_ in table_)
        {
            for(int i=0;i<item_.maxAmount;i++)
            {
                if(Random.Range(0,100)>item_.chancesToSpawn)
                {
                    AddLootItem(item_.item);
                }
            }
        }
    }
    public void AddLootItem(LootItem item_)
    {
        hasFoundItem = false;
        foreach (LootItem item in currentItems)
        {
            if (item.name == item_.name)
            {
                item.amount += item_.amount;
                hasFoundItem = true;
                break;
            }
        }
        if (!hasFoundItem)
            currentItems.Add(item_);
    }
}
