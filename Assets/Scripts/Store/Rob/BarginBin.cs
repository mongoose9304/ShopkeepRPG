using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class BarginBinSlot
{
    public int amount;
    public ItemData myItem;
}
public class BarginBin : InteractableObject
{
    public List<BarginBinSlot> binSlots = new List<BarginBinSlot>();
    public List<Image> binImages = new List<Image>();
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
}
