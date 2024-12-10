using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// The class for the objects you can put one type of item and NPCs will approach to haggle for them/steal from them
/// </summary>
public class Pedestal : InteractableObject
{
    [Tooltip("Is this pedestal located in Hell")]
    public bool inHell;
    [Tooltip("The Item that has been placed on the pedestal")]
    public ItemData myItem;
    [Tooltip("The amount of the item placed on this pedestal")]
    public int amount;
    [Tooltip("The last item placed on this pedestal to quickly refill")]
    public ItemData myItemPrevious;
    [Tooltip("The amount of the last item placed on the pedestal")]
    public int amountPrevious;
    [Tooltip("Is this pedestal being used")]
    public bool inUse;
    [Tooltip("is a hot item placed?")]
    public bool hotItem;
    [Tooltip("is a cold item placed")]
    public bool coldItem;
    [Tooltip("REFERNCE to the Image that changes absed on the item placed oon the pedestal")]
    public Image myItemImage;
    [Tooltip("REFERENCE to the price text that changes when items are placed")]
    public TextMeshProUGUI basePriceText;
    [Tooltip("REFERENCE to the effect that activates when a hot item is placed down")]
    public GameObject hotEffect;
    [Tooltip("REFERENCE to the effect that activates when a cold item is placed down")]
    public GameObject coldEffect;
    /// <summary>
    /// The virtual function all interactbale objects will override to set thier specific functionality
    /// </summary>
    public override void Interact(GameObject interactingObject_ = null)
    {
        if (interactingObject_ == null)
            return;
      if(interactingObject_.TryGetComponent<StorePlayer>(out StorePlayer playa))
        {
            if (playa.isInMovingMode)
            {
              ShopManager.instance.PlayPopUpText("Exit Moving Mode first", playa.isPlayer2);
                return;
            }
            if(!playa.isPlayer2)
            {
                if(ShopManager.instance.OpenAMenu(PlayerManager.instance.GetPlayers()[0]))
                    ShopManager.instance.OpenPedestal(this);
            }
            else if (playa.isPlayer2)
            {
                if (ShopManager.instance.OpenAMenu(PlayerManager.instance.GetPlayers()[1], true))
                    ShopManager.instance.OpenPedestal(this,true);
            }

        }
    }
    /// <summary>
    /// Set an item on this pedestal and set up the visuals , if the item is null this will throw an error
    /// </summary>
    public virtual void SetItem(ItemData myItem_,int amount_)
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
                int x = Mathf.RoundToInt(myItem_.basePrice * amount_ * ShopManager.instance.GetHotItemMultiplier());
                basePriceText.text = (x).ToString();
                break;
            case 2://cold
                coldItem = true;
                coldEffect.SetActive(true);
                int y = Mathf.RoundToInt(myItem_.basePrice * amount_ * ShopManager.instance.GetColdItemMultiplier());
                basePriceText.text = y.ToString();
                break;
        }
    }
    /// <summary>
    /// Save the previous item 
    /// </summary>
    public void SetPreviousItem(ItemData myItem_, int amount_)
    {
        myItemPrevious = myItem_;
        amountPrevious = amount_;
    }
    /// <summary>
    /// Remove the item form this pedestal
    /// </summary>
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
    public virtual void SetInUse(bool inUse_)
    {
        ShopManager.instance.RemoveInteractableObject(this.gameObject);
        gameObject.SetActive(!inUse_);
        inUse = inUse_;
    }
    /// <summary>
    ///  If the item is sold remove the visuals and remove this pedestal from the customer manager
    /// </summary>
    public virtual void ItemSold()
    {
        ClearItem();
        CustomerManager.instance.CheckPedestalsforItems();
        SetInUse(false);
        coldEffect.SetActive(false);
        hotEffect.SetActive(false);
        coldItem = false;
        hotItem = false;
    }
    /// <summary>
    /// If the object is being destroyed prevent it from being interacted with
    /// </summary>
    public void ObjectBeingDestroyed()
    {
        if(ShopManager.instance)
        ShopManager.instance.RemoveInteractableObject(this.gameObject);
    }
    /// <summary>
    /// return the cost of the item after checking if it is hot/cold
    /// </summary>
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
