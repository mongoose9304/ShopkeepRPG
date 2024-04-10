using MoreMountains.Feedbacks;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public ScrollRect scrollRect;
    public int maxUIBackgrounds;
    int currentUIBackground;
    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
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
                AddUILootObject(item_, false);
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
        currentUIBackground += 1;
        if (currentUIBackground > maxUIBackgrounds)
            currentUIBackground = 0;
        pooler.GetPooledGameObject().GetComponent<LootUIObject>().CreateUIObject(item_.amount, item_.name, isNew,currentUIBackground);
        scrollRect.normalizedPosition = new Vector2(0, 1);
        
    }
    
}
