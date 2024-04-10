using MoreMountains.Feedbacks;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
    public GameObject lootCollectionUIObject;
    public float maxTimeCollectionUIWillBeOut = 6.0f;
    public float currentTimeCollectionUIWillBeOut;

    private void Start()
    {
        instance = this;
    }
    private void Update()
    {
       // scrollRect.normalizedPosition = new Vector2(0, 1);
        scrollRect.DOVerticalNormalizedPos(1, 1);

        currentTimeCollectionUIWillBeOut -= Time.deltaTime;
        if(currentTimeCollectionUIWillBeOut<=0)
        {
            PutAwaylootCollectionUIObject();
        }
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
       // scrollRect.normalizedPosition = new Vector2(0, 1);
        BringlootCollectionUIObjectOut();
        currentTimeCollectionUIWillBeOut = maxTimeCollectionUIWillBeOut;
        
    }
    public void BringlootCollectionUIObjectOut()
    {
        //1303.8
        //1003
         lootCollectionUIObject.transform.DOLocalMoveX(1003,1);
      
       // Debug.Log("Loot Pos "+lootCollectionUIObject.transform.localPosition.x);
    }
    public void PutAwaylootCollectionUIObject()
    {
        //1303.8
        //1003
        lootCollectionUIObject.transform.DOLocalMoveX(1303.8f, 1);
        
      
    }


}
