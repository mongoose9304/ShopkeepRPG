using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutPedestal : Pedestal
{
    public ItemData tutHotItem;
    public ItemData tutColdItem;
    public int tutState;
    public override void SetItem(ItemData myItem_, int amount_)
    {
        myItem = myItem_;
        amount = amount_;
        myItemImage.sprite = myItem.itemSprite;
        myItemImage.gameObject.SetActive(true);
        basePriceText.text = (myItem_.basePrice * amount_).ToString();
        basePriceText.gameObject.SetActive(true);
        hotEffect.SetActive(false);
        hotItem = false;
        coldItem = false;
        coldEffect.SetActive(false);
        if(myItem.itemName==tutHotItem.itemName)
        {
            hotItem = true;
            hotEffect.SetActive(true);
            basePriceText.text = (myItem_.basePrice * amount_ * ShopManager.instance.GetHotItemMultiplier()).ToString();
        }
        else if(myItem.itemName == tutColdItem.itemName)
        {
            coldItem = true;
            coldEffect.SetActive(true);
            basePriceText.text = (myItem_.basePrice * amount_ * ShopManager.instance.GetColdItemMultiplier()).ToString();
        }
        if (tutState > 0)
        {
            if(tutState==10)
            {
                if(myItem.itemName == tutHotItem.itemName)
                {
                    ShopTutorialManager.instance_.SetTutorialState(tutState);
                    tutState = 0;
                }
                return;
            }
            ShopTutorialManager.instance_.SetTutorialState(tutState);
            tutState = 0;
        }
    }
    public override void ItemSold()
    {
        ClearItem();
        SetInUse(false);
        coldEffect.SetActive(false);
        hotEffect.SetActive(false);
        coldItem = false;
        hotItem = false;
    }
    public override void SetInUse(bool inUse_)
    {
        ShopManager.instance.RemoveInteractableObject(this.gameObject);
        gameObject.SetActive(!inUse_);
        inUse = inUse_;
    }
}
