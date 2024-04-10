using MoreMountains.Feedbacks;

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
    public static LootManager instance;
  [SerializeField] List<LootItem> currentLootItems = new List<LootItem>();
    bool hasFoundItem;
    public MMMiniObjectPooler pooler;

    private void Start()
    {
        instance = this;
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
                AddUILootObject(item, false);
                break;
            }
        }
        if (!hasFoundItem)
        {
             LootItem x=new LootItem();
            x.amount = item_.amount;
            x.name = item_.name;
            currentLootItems.Add(x);
            AddUILootObject(x, true);
        }
    }
    public void AddUILootObject(LootItem item_,bool isNew=false)
    {
        pooler.GetPooledGameObject().GetComponent<LootUIObject>().CreateUIObject(item_.amount, item_.name, isNew);
    }
}
