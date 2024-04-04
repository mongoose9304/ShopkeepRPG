using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public string name;
    public int amount;
}
[System.Serializable]
public class LootTableItem
{
    public LootItem item;
    public int maxAmount;
    public float chancesToSpawn;
}
public class LootManager : MonoBehaviour
{
    public static LootManager LootManager_;
  [SerializeField] List<LootItem> currentLootItems = new List<LootItem>();
    bool hasFoundItem;
    private void Start()
    {
        LootManager_ = this;
    }
    public void AddLootItem(LootItem item_)
    {
        hasFoundItem = false;
        foreach(LootItem item in currentLootItems)
        {
            if(item.name==item_.name)
            {
                item.amount += item_.amount;
                hasFoundItem = true;
                break;
            }
        }
        if (!hasFoundItem)
            currentLootItems.Add(item_);
    }
}
