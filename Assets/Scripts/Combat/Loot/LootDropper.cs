using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public List<LootTableItem> myTable=new List<LootTableItem>();
    public List<LootItem> itemsToDrop;
    bool hasFoundItem;
    public GameObject lootObject;
    GameObject temp;
    public void DropItems(float rateMultiplier=1.0f)
    {
        itemsToDrop.Clear();
        foreach(LootTableItem tableItem in myTable)
        {
            for(int i=0;i<tableItem.maxAmount;i++)
            {
                if((Random.Range(0,100)*rateMultiplier)<=tableItem.chancesToSpawn)
                {
                    AddLootItem(tableItem.item);
                }
            }
        }
        if (itemsToDrop.Count == 0)
            return;

        foreach(LootItem item_ in itemsToDrop)
        {
           temp= Instantiate(lootObject, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            temp.GetComponent<LootWorldObject>().myItem = item_;
        }


    }
    public void AddLootItem(LootItem item_)
    {
        hasFoundItem = false;
        foreach (LootItem item in itemsToDrop)
        {
            if (item.name == item_.name)
            {
                item.amount += item_.amount;
                hasFoundItem = true;
                break;
            }
        }
        if (!hasFoundItem)
        {
            LootItem x = new LootItem();
            x.amount = item_.amount;
            x.name = item_.name;
            itemsToDrop.Add(x);
        }
    }
}
