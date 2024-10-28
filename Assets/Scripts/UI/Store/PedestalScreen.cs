using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalScreen : MonoBehaviour
{
    public InventorySlot currentPedestalSlot;
    public InventoryUI  inventoryUI;
    public InventorySlot currentInventorySlot;
    public Pedestal openPedestal;
    public GameObject buttons;
    public void OpenMenu(Pedestal p_)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentPedestalSlot.gameObject);
        openPedestal = p_;
        if (p_.myItem != null && p_.amount > 0)
        {
            currentPedestalSlot.SetItem(p_.myItem, p_.amount);
            int x = 0;
            if (inventoryUI.GetSlotWithName(p_.myItem.itemName)!=null)
            x = inventoryUI.GetSlotWithName(p_.myItem.itemName).amount;
            currentInventorySlot.SetItem(p_.myItem, x);
            SetButtonsActive(true);
        }
        else if (p_.myItem != null && p_.amount == 0)
        {
            currentPedestalSlot.SetItem(p_.myItem, p_.amount);
            currentInventorySlot.myItem = p_.myItem;
            currentInventorySlot.amount = inventoryUI.GetSlotWithName(p_.myItem.itemName).amount;
            SetButtonsActive(true);
        }
        else
        {
            currentPedestalSlot.SetNullItem();
            currentInventorySlot.SetNullItem();
            SetButtonsActive(false);
        }
        
    }
    public void AddAmountOfCurrentItem(int amount_)
    {
       if (currentInventorySlot.amount >= amount_)
        {
            currentInventorySlot.UpdateAmount(currentInventorySlot.amount -= amount_);
            currentPedestalSlot.UpdateAmount(currentPedestalSlot.amount += amount_);
        }

    }
    public void AddMaxAmountOfCurrentItem()
    {
        AddAmountOfCurrentItem(currentInventorySlot.amount);

    }
    public void AddHalfAmountOfCurrentItem()
    {
        AddAmountOfCurrentItem(currentInventorySlot.amount/2);

    }
    public void ClearButton()
    {
        currentPedestalSlot.SetNullItem();
        currentInventorySlot.SetNullItem();
        SetButtonsActive(false);
    }
    public void SetButtonsActive(bool isActive_=true)
    {
        buttons.SetActive(isActive_);
    }
    public void OpenInventorySection()
    {
        inventoryUI.OpenMenu(true);
        inventoryUI.SetClickFunctionIndex(1);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(inventoryUI.slots[0].gameObject);
    }
    public void ChangeItem(ItemData data_,int amount_)
    {
        currentPedestalSlot.SetItem(data_, 1);
        currentInventorySlot.SetItem(data_, amount_ - 1);
    }
    public void ResetSelectedItem()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentPedestalSlot.gameObject);
    }
}
