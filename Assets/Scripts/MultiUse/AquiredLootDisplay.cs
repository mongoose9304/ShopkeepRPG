using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquiredLootDisplay : MonoBehaviour
{
   [SerializeField] List<LootItem> itemsToDisplay = new List<LootItem>();
    private bool allItemsDisplayed;
   [SerializeField] private int currentItem;
    public int maxUIBackgrounds;
    int currentUIBackground;
    public float maxTimeBetweenAdds;
    public float currentTimeBetweenAdds;
    public MMMiniObjectPooler pooler;
    private void Update()
    {
        if (allItemsDisplayed)
            return;
        currentTimeBetweenAdds -= Time.deltaTime;
        if(currentTimeBetweenAdds<=0)
        {
            currentTimeBetweenAdds = maxTimeBetweenAdds;
            DisplayItem();
        }
    }
    private void DisplayItem()
    {
        AddUILootObject(itemsToDisplay[currentItem], false);
        currentItem += 1;
        if(currentItem>=itemsToDisplay.Count)
        {
            allItemsDisplayed = true;
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
        foreach(LootItem item in items_)
        {
            itemsToDisplay.Add(item);
        }
    }
}
