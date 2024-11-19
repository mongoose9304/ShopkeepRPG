using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pedestal : InteractableObject
{
    public bool inHell;
    public ItemData myItem;
    public int amount;
    public ItemData myItemPrevious;
    public int amountPrevious;
    public Image myItemImage;
    public bool inUse;
    public TextMeshProUGUI basePriceText;
    public GameObject hotEffect;
    public GameObject coldEffect;
    public bool hotItem;
    public bool coldItem;
    /// <summary>
    /// The virtual function all interactbale objects will override to set thier specific functionality
    /// </summary>
    public override void Interact(GameObject interactingObject_ = null)
    {
        ShopManager.instance.OpenPedestal(this);
    }
    public void SetItem(ItemData myItem_,int amount_)
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
        switch (ShopManager.instance.CheckIfItemIsHot(myItem,inHell))
        {
            case 0://normal
                break;
            case 1://hot
                hotItem = true;
                hotEffect.SetActive(true);
                basePriceText.text = (myItem_.basePrice * amount_*ShopManager.instance.GetHotItemMultiplier()).ToString();
                break;
            case 2://cold
                coldItem = true;
                coldEffect.SetActive(true);
                basePriceText.text = (myItem_.basePrice * amount_*ShopManager.instance.GetColdItemMultiplier()).ToString();
                break;
        }
    }
    public void SetPreviousItem(ItemData myItem_, int amount_)
    {
        myItemPrevious = myItem_;
        amountPrevious = amount_;
    }
    public void ClearItem()
    {
        myItemImage.gameObject.SetActive(false); ;
        myItem = null;
        amount = 0;
        myItemImage.sprite = null;
        basePriceText.gameObject.SetActive(false);
        coldItem = false;
        hotItem = false;
        coldEffect.SetActive(false);
        hotEffect.SetActive(false);
    }
    public void SetInUse(bool inUse_)
    {
        ShopManager.instance.RemoveInteractableObject(this.gameObject);
        gameObject.SetActive(!inUse_);
        inUse = inUse_;
    }
    public void ItemSold()
    {
        ClearItem();
        CustomerManager.instance.CheckPedestalsforItems();
        SetInUse(false);
        coldEffect.SetActive(false);
        hotEffect.SetActive(false);
        coldItem = false;
        hotItem = false;
    }
    public void ObjectBeingDestroyed()
    {
        if(ShopManager.instance)
        ShopManager.instance.RemoveInteractableObject(this.gameObject);
    }
    public float GetItemCost()
    {
        if (hotItem)
        {
            return myItem.basePrice * amount*ShopManager.instance.GetHotItemMultiplier();
        }
        else if (coldItem)
        {
            return myItem.basePrice * amount*ShopManager.instance.GetColdItemMultiplier();
        }
        else
        {
            return myItem.basePrice * amount;
        }
    }

}
