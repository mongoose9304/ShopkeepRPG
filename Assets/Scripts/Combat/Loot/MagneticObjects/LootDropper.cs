using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    public List<LootTableItem> myTable=new List<LootTableItem>();
    public List<LootItem> itemsToDrop;
    public bool DropOnlyOneItem;
    bool hasFoundItem;
    public GameObject lootObject;
    GameObject temp;
    public bool dropNothing;
    public void DropItems()
    {
        if (dropNothing)
            return;
        PopulateLootDrop();
        if (itemsToDrop.Count == 0)
            return;

        foreach(LootItem item_ in itemsToDrop)
        {
            temp = LootManager.instance.GetWorldLootObject();
            temp.transform.position = transform.position;
            temp.transform.rotation = new Quaternion(0, 0, 0, 0);
            temp.GetComponent<LootWorldObject>().myItem = item_;
            temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0,0.25f), 4, Random.Range(0, 0.25f)), ForceMode.VelocityChange);
            temp.gameObject.SetActive(true);
        }


    }
    public void PopulateLootDrop()
    {
        if (DropOnlyOneItem)
        {
            AddLootItem(myTable[Random.Range(0,myTable.Count)].item);
        }
        else
        {


            itemsToDrop.Clear();
            foreach (LootTableItem tableItem in myTable)
            {
                for (int i = 0; i < tableItem.maxAmount; i++)
                {
                    if ((Random.Range(0, 100) * LootManager.instance.lootDropRateMultiplier) <= tableItem.chancesToSpawn)
                    {
                        AddLootItem(tableItem.item);
                    }
                }
            }
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
    //for setting up a loot table from another srcipt
    public void SetLootTable(LootTableItem[] lootTable)
    {
        myTable.Clear();
        foreach (LootTableItem tableItem in lootTable)
        {
            myTable.Add(tableItem);
        }
       
    }
    public void DropSpecificItem(LootItem item_)
    {
        temp = LootManager.instance.GetWorldLootObject();
        temp.transform.position = transform.position;
        temp.transform.rotation = new Quaternion(0, 0, 0, 0);
        temp.GetComponent<LootWorldObject>().myItem = item_;
        temp.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 0.25f), 4, Random.Range(0, 0.25f)), ForceMode.VelocityChange);
        temp.gameObject.SetActive(true);
    }
}
