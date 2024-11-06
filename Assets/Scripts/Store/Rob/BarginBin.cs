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
    public List<BarginBinSlot> binSlots = new List<BarginBinSlot>();
    public List<BarginBinSlot> binSlotsWithItems = new List<BarginBinSlot>();
    public List<Image> binImages = new List<Image>();
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
    }
    public void ClearSlot(int index)
    {
        binSlots[index].myItem = null;
        binSlots[index].amount = 0;
        binImages[index].sprite = null;
        binImages[index].gameObject.SetActive(false);
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
    }
    public void SellItem(BarginBinSlot slot_)
    {
        ClearSlot(binSlots.IndexOf(slot_));
        UpdateSlotsWithItems();
        ApplyDiscountToAllItems();
        CustomerManager.instance.CheckBarginBinsForItems();
    }
}
