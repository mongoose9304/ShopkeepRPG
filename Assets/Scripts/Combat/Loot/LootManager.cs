using MoreMountains.Feedbacks;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
[System.Serializable]
public class LootItem
{
    public string name;
    public int amount;
    public bool isNew;
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
    public LootItem testItem;
    [SerializeField] private float cashMultiplier = 1;
  [SerializeField] List<LootItem> currentLootItems = new List<LootItem>();
    bool hasFoundItem;
    public MMMiniObjectPooler pooler;
    [SerializeField] MMMiniObjectPooler worldObjectPool;
    public ScrollRect scrollRect;
    public int maxUIBackgrounds;
    int currentUIBackground;
    public GameObject lootCollectionUIObject;
    public float maxTimeCollectionUIWillBeOut = 6.0f;
    public float currentTimeCollectionUIWillBeOut;

   public MMF_Player[] demonCashPickUpFeedBacks;
    public int demonCurrentCash;
    public TextMeshProUGUI demonCurrentCashText;
    public MMF_Player[] regularCashPickUpFeedBacks;
    public int regularCurrentCash;
    public TextMeshProUGUI regularCurrentCashText;
    public MMF_Player[] resourcePickUpFeedBacks;
    public int currentResource;
    public TextMeshProUGUI currentResourceText;
    public List<LootItem> AquiredLootItems =new List<LootItem>();
    public List<LootItem> WaitingLootItemPool =new List<LootItem>();
    public float maxDelayBetweenPopUps;
    float currentDelayBetweenPopUps;
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
        if(WaitingLootItemPool.Count>0)
        {
            currentDelayBetweenPopUps -= Time.deltaTime;
            if(currentDelayBetweenPopUps<=0)
            {
                currentTimeCollectionUIWillBeOut=maxTimeCollectionUIWillBeOut;
                currentDelayBetweenPopUps = maxDelayBetweenPopUps;
                DisplayUILootObject(WaitingLootItemPool[0]);
                WaitingLootItemPool.RemoveAt(0);
               
            }
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
        AquiredLootItems.Add(item_);
    }
    public void AddUILootObject(LootItem item_,bool isNew=false)
    {
        item_.isNew = isNew;
        WaitingLootItemPool.Add(item_);
       // scrollRect.normalizedPosition = new Vector2(0, 1);
        BringlootCollectionUIObjectOut();
        currentTimeCollectionUIWillBeOut = maxTimeCollectionUIWillBeOut;
       
        
    }
    private void DisplayUILootObject(LootItem item_)
    {
        currentUIBackground += 1;
        if (currentUIBackground > maxUIBackgrounds)
            currentUIBackground = 0;
        pooler.GetPooledGameObject().GetComponent<LootUIObject>().CreateUIObject(item_.amount, item_.name, item_.isNew, currentUIBackground);
    }
    public void AddDemonMoney(int money_)
    {
        demonCurrentCash += Mathf.RoundToInt(money_ * cashMultiplier);
        demonCurrentCashText.text = demonCurrentCash.ToString("#,#");
           foreach(MMF_Player player_ in demonCashPickUpFeedBacks)
        {
            player_.PlayFeedbacks();
        }
    }
    public void AddResource(int resource_)
    {
        currentResource += resource_;
        currentResourceText.text = currentResource.ToString("#,#");
        foreach (MMF_Player player_ in resourcePickUpFeedBacks)
        {
            player_.PlayFeedbacks();
        }
    }
    public void AddRegularMoney(int money_)
    {
       
        regularCurrentCash+=Mathf.RoundToInt(money_ * cashMultiplier);
        regularCurrentCashText.text = regularCurrentCash.ToString("#,#");
        foreach (MMF_Player player_ in regularCashPickUpFeedBacks)
        {
            player_.PlayFeedbacks();
        }
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
        pooler.ResetAllObjects();


    }
    public GameObject GetWorldLootObject()
    {
        return worldObjectPool.GetPooledGameObject();
    }
    public void AddToCashMultiplier(float amount_)
    {
        cashMultiplier += amount_;
    }
    public void AddDebugUIItem()
    {
        AddUILootObject(testItem);
    }

}
