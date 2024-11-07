using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pedestal : InteractableObject
{
    public ItemData myItem;
    public int amount;
    public ItemData myItemPrevious;
    public int amountPrevious;
    public Image myItemImage;
    public bool inUse;
    public TextMeshProUGUI basePriceText;
 
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
    }
   
}
