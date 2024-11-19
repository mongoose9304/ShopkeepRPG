using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class BarginBinSlot
{
    public int amount;
    public ItemData myItem;
    public int discountedCost;
}

public class BarginBin : InteractableObject
{
    public bool inHell;
    public List<BarginBinSlot> binSlots = new List<BarginBinSlot>();
    public List<BarginBinSlot> binSlotsPrevious = new List<BarginBinSlot>();
    public List<BarginBinSlot> binSlotsWithItems = new List<BarginBinSlot>();
    public List<Image> binImages = new List<Image>();
    public List<GameObject> hotEffects = new List<GameObject>();
    public List<GameObject> coldEffects = new List<GameObject>();
    public float itemDiscount;
    public TextMeshProUGUI discountText;
    public bool inUse;
    public override void Interact(GameObject interactingObject_ = null)
    {
        ShopManager.instance.OpenBarginBin(this);
    }
    public void SetSlot(int index,ItemData item_,int amount_)
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
    public void SetBinDiscountAmount(float amount_)
    {
        itemDiscount = amount_;
        itemDiscount = Mathf.Round(itemDiscount * 100.0f) * 0.01f;
        discountText.text = Mathf.Round(itemDiscount * 100.0f).ToString();
    }
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
    public void SellItem(BarginBinSlot slot_)
    {
        ClearSlot(binSlots.IndexOf(slot_));
        UpdateSlotsWithItems();
        ApplyDiscountToAllItems();
        CustomerManager.instance.CheckBarginBinsForItems();
    }
    public void ObjectBeingDestroyed()
    {
        if (ShopManager.instance)
            ShopManager.instance.RemoveInteractableObject(this.gameObject);
    }
}
