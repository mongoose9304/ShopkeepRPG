using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BarginBinSlot
{
    public int amount;
    public ItemData myItem;
}
public class BarginBin : InteractableObject
{
    public List<BarginBinSlot> binSlots = new List<BarginBinSlot>();
    public override void Interact(GameObject interactingObject_ = null)
    {
        ShopManager.instance.OpenBarginBin(this);
    }
}
