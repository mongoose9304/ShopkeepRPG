using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
/// <summary>
/// Container for information for each slot an item can be placed in the bargin bin 
/// </summary>
public class BarginBinSlot
{
    public int amount;
    public ItemData myItem;
    public int discountedCost;
}
/// <summary>
/// Objects where the player can put many items at a discounted cost  
/// </summary>
public class BarginBin : InteractableObject
{
    [Tooltip("Is this bin in the hell section")]
    public bool inHell;
    [Tooltip("REFERENCE to all the bin slots in this object")]
    public List<BarginBinSlot> binSlots = new List<BarginBinSlot>();
    [Tooltip("REFERENCE to all the previous bin slots in this object. Used to refill items quickly")]
    public List<BarginBinSlot> binSlotsPrevious = new List<BarginBinSlot>();
    [Tooltip("All the slots with items in them for NPCs to look through")]
    public List<BarginBinSlot> binSlotsWithItems = new List<BarginBinSlot>();
    [Tooltip("REFERENCE to all the images that are displayed when an item is placed in that slot")]
    public List<Image> binImages = new List<Image>();
    [Tooltip("REFERENCE to all the effects to show if an item is HOT")]
    public List<GameObject> hotEffects = new List<GameObject>();
    [Tooltip("REFERENCE to all the effects to show if an item is Cold")]
    public List<GameObject> coldEffects = new List<GameObject>();
    [Tooltip("The current discount % on all items 0-1")]
    public float itemDiscount;
    [Tooltip("REFERENCE to UI text showing the discount value")]
    public TextMeshProUGUI discountText;
    [Tooltip("Is this bin currently in use")]
    public bool inUse;
    public override void Interact(GameObject interactingObject_ = null)
    {
        if (interactingObject_.TryGetComponent<StorePlayer>(out StorePlayer playa))
        {
            if (!playa.isPlayer2)
            {
                if (ShopManager.instance.OpenAMenu(PlayerManager.instance.GetPlayers()[0]))
                    ShopManager.instance.OpenBarginBin(this);
            }
            else if (playa.isPlayer2)
            {
                if (ShopManager.instance.OpenAMenu(PlayerManager.instance.GetPlayers()[1], true))
                    ShopManager.instance.OpenBarginBin(this, true);
            }

        }
    }
    /// <summary>
    /// Set a specifc slot with an item and amount
    /// </summary>
    public virtual void SetSlot(int index,ItemData item_,int amount_)
    {
        binSlots[index].myItem = item_;
        binSlots[index].amount = amount_;
        binImages[index].sprite = item_.itemSprite;
        binImages[index].gameObject.SetActive(true);
        switch(ShopManager.instance.CheckIfItemIsHot(item_,inHell))
        {
            case 0:
                hotEffects[index].SetActive(false);
                coldEffects[index].SetActive(false);
                break;
            case 1:
                hotEffects[index].SetActive(true);
                break;
            case 2:
                coldEffects[index].SetActive(true);
                break;
        }
    }
    /// <summary>
    /// Save the item that was placed here previously to refill items easily
    /// </summary>
    public void SetPreviousSlot(int index, ItemData item_, int amount_)
    {
        binSlotsPrevious[index].myItem = item_;
        binSlotsPrevious[index].amount = amount_;
    }
    public void ClearPreviousSlots()
    {
        foreach (BarginBinSlot slot_ in binSlotsPrevious)
        {
            slot_.amount = 0;
            slot_.myItem = null;
        }
    }
    public void ClearSlot(int index)
    {
        binSlots[index].myItem = null;
        binSlots[index].amount = 0;
        binImages[index].sprite = null;
        binImages[index].gameObject.SetActive(false);
        hotEffects[index].SetActive(false);
        coldEffects[index].SetActive(false);
    }
    /// <summary>
    /// Create a list with all the items with items in them
    /// </summary>
    public void UpdateSlotsWithItems()
    {
        binSlotsWithItems.Clear();
        foreach(BarginBinSlot slot_ in binSlots)
        {
            if(slot_.myItem&&slot_.amount>0)
            {
                binSlotsWithItems.Add(slot_);
            }
        }
    }
    /// <summary>
    /// Change the item discount
    /// </summary>
    public void SetBinDiscountAmount(float amount_)
    {
        itemDiscount = amount_;
        itemDiscount = Mathf.Round(itemDiscount * 100.0f) * 0.01f;
        discountText.text = Mathf.Round(itemDiscount * 100.0f).ToString();
    }
    /// <summary>
    /// Calculate the price for all ietsm placed in the bin
    /// </summary>
    public void ApplyDiscountToAllItems()
    {
        foreach (BarginBinSlot slot_ in binSlotsWithItems)
        {
            if (slot_.myItem && slot_.amount > 0)
            {
                slot_.discountedCost = Mathf.RoundToInt(slot_.myItem.basePrice * slot_.amount * (1 - itemDiscount));
            }
        }
        discountText.text = Mathf.Round(itemDiscount * 100.0f).ToString();
    }
    /// <summary>
    /// Take an item out of the bin, the player earns the money from this sale at the cash register 
    /// </summary>
    public void SellItem(BarginBinSlot slot_)
    {
        ClearSlot(binSlots.IndexOf(slot_));
        UpdateSlotsWithItems();
        ApplyDiscountToAllItems();
        CustomerManager.instance.CheckBarginBinsForItems();
    }
    /// <summary>
    /// Called when this object is being destroyed to prevnt issues with interacting with destroyed objects
    /// </summary>
    public void ObjectBeingDestroyed()
    {
        if (ShopManager.instance)
            ShopManager.instance.RemoveInteractableObject(this.gameObject);
    }
}
