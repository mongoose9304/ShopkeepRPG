using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalScreen : MonoBehaviour
{
    public InventorySlot currentPedestalSlot;
    public InventoryUI  inventoryUI;
    public InventorySlot currentInventorySlot;
    public Pedestal openPedestal;
    public void OpenMenu(Pedestal p_)
    {
        openPedestal = p_;
        if (p_.myItem != null && p_.amount > 0)
        {
            currentPedestalSlot.SetItem(p_.myItem, p_.amount);
            int x = 0;
            if (inventoryUI.GetSlotWithName(p_.myItem.itemName)!=null)
            x = inventoryUI.GetSlotWithName(p_.myItem.itemName).amount;
            currentInventorySlot.SetItem(p_.myItem, x);
        }
        else if (p_.myItem != null && p_.amount == 0)
        {
            currentPedestalSlot.SetItem(p_.myItem, p_.amount);
            currentInventorySlot.myItem = p_.myItem;
            currentInventorySlot.amount = inventoryUI.GetSlotWithName(p_.myItem.itemName).amount;
        }
        else
        {
            currentPedestalSlot.SetNullItem();
        }
        
    }
}
