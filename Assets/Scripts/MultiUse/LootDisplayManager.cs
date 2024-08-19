using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class LootDisplayManager : MonoBehaviour
{
    public static LootDisplayManager instance;
   [SerializeField] List<LootItem> itemsToDisplay = new List<LootItem>();
    private bool allItemsDisplayed;
   [SerializeField] private int currentItem;
    public int maxUIBackgrounds;
    int currentUIBackground;
    [SerializeField] float maxTimeBetweenAdds;
    [SerializeField] float maxObjectsCollectedDisplayTime;
    float currentTimeBetweenAdds;
    public float maxResourceDisplayTime;
    public float alteredResourceDisplayTime;
    float currentResourceDisplayTime;
    public MMMiniObjectPooler pooler;
    public List<AcquiredLootDisplay> resourceLootList = new List<AcquiredLootDisplay>();
    [SerializeField] List<int> resourcesAcquired = new List<int>();
    [SerializeField] List<int> resourcesInventory = new List<int>();
    [SerializeField] List<Sprite> resourcesSprites = new List<Sprite>();
    [SerializeField] private GameObject playerFireworkObject;
    [SerializeField] private GameObject playerLossObject;
    [SerializeField] private GameObject playerFamiliarHolderObject;
    [SerializeField] UnityEvent startVictoryEvent;
    [SerializeField] UnityEvent endVictoryEvent;
    private bool isPlaying;
    private bool hasLost = false;
    private void Awake()
    {
       // StartVictoryScreen();
        instance = this;
    }
    private void Update()
    {
        if (!isPlaying)
            return;
        if (allItemsDisplayed)
        {
            currentResourceDisplayTime -= Time.deltaTime;
            if(currentResourceDisplayTime<=0)
            {
                if(hasLost)
                    playerLossObject.SetActive(true);
                else
                    playerFireworkObject.SetActive(true);
                endVictoryEvent.Invoke();
                isPlaying = false;
            }
            return;
        }
        currentTimeBetweenAdds -= Time.deltaTime;
        if(currentTimeBetweenAdds<=0)
        {
            currentTimeBetweenAdds = maxTimeBetweenAdds;
            DisplayItem();
        }
    }
    private void DisplayItem()
    {
        if(itemsToDisplay.Count==0)
        {
            allItemsDisplayed = true;
            return;
        }

        AddUILootObject(itemsToDisplay[currentItem], false);
        currentItem += 1;
        if(currentItem>=itemsToDisplay.Count)
        {
            allItemsDisplayed = true;
            currentResourceDisplayTime = alteredResourceDisplayTime;
            DisplayLootedResouces();
        }
    }
    public void AddUILootObject(LootItem item_, bool isNew = false)
    {
        currentUIBackground += 1;
        if (currentUIBackground > maxUIBackgrounds)
            currentUIBackground = 0;
        pooler.GetPooledGameObject().GetComponent<LootUIObject>().CreateUIObject(item_.amount, item_.name, isNew, currentUIBackground);
        // scrollRect.normalizedPosition = new Vector2(0, 1);


    }
    public void AddItems(List<LootItem> items_)
    {
        itemsToDisplay.Clear();
        foreach (LootItem item in items_)
        {
            itemsToDisplay.Add(item);
        }
    }
    public void AddResources(List<int> resourcesAcquired_, List<int> resourcesInventory_,List<Sprite> resourceSprites_)
    {
        resourcesAcquired.Clear();
        resourcesInventory.Clear();
        resourcesSprites.Clear();
        foreach (int item in resourcesAcquired_)
        {
            resourcesAcquired.Add(item);
        }
        foreach (int item in resourcesInventory_)
        {
            resourcesInventory.Add(item);
        }
        foreach (Sprite item in resourceSprites_)
        {
            resourcesSprites.Add(item);
        }
    }
    public void DisplayLootedResouces()
    {
        foreach(AcquiredLootDisplay loot in resourceLootList)
        {
            loot.gameObject.SetActive(false);
        }
        for(int i=0;i< resourcesAcquired.Count;i++)
        {
            resourceLootList[i].gameObject.SetActive(true);
            resourceLootList[i].StartCountEffect(resourcesInventory[i], resourcesAcquired[i], resourcesSprites[i],alteredResourceDisplayTime,hasLost);
        }
    }
    public void StartVictoryScreen(bool hasLost_=false)
    {
        hasLost = hasLost_;
        isPlaying = true;
        playerFamiliarHolderObject.SetActive(true);
        startVictoryEvent.Invoke();
        if(itemsToDisplay.Count> maxObjectsCollectedDisplayTime*2)
        maxTimeBetweenAdds = maxObjectsCollectedDisplayTime/ itemsToDisplay.Count;
        else
        {
            maxTimeBetweenAdds = 0.4f;
        }
        if(resourcesAcquired[0]==0)
        {
            alteredResourceDisplayTime = 0.1f;
        }
        else
        {
            alteredResourceDisplayTime = maxResourceDisplayTime;
        }
    }
}
