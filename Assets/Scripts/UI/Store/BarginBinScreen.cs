using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BarginBinScreen : MonoBehaviour
{
    // This ones the big displayed Icon in the middle
    public InventorySlot currentlySelectedSlot;
    public InventoryUI inventoryUI;
    // This ones a mirror of how much of the item you have in your inventory
    public InventorySlot currentInventorySlot;
    public BarginBin openBarginBin;
    public GameObject buttons;
    public GameObject inventoryObject;
    public GameObject addingButtons;
    public TextMeshProUGUI currentItemNameText;
    public TextMeshProUGUI currentItemValue;
    public List<InventorySlot> slots = new List<InventorySlot>();
    int currentSlotIndex;
    public void OpenMenu(BarginBin bin_)
    {
        openBarginBin = bin_;
        LoadInventory(bin_);
        SetButtonsActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(slots[0].gameObject);
    }
    public void LoadInventory(BarginBin bin_)
    {
        int index = 0;
        foreach (InventorySlot slot_ in slots)
        {
            slot_.SetNullItem();
            slot_.gameObject.SetActive(true);
        }
        foreach (BarginBinSlot item_ in bin_.binSlots)
        {
            if (item_.myItem)
            {
                slots[index].SetItem(item_.myItem, item_.amount);
                slots[index].gameObject.SetActive(true);
                index += 1;
            }
        }
    }
    public void SetButtonsActive(bool isActive_ = true)
    {
        buttons.SetActive(isActive_);
        inventoryObject.SetActive(!isActive_);
    }
    public void BarginSlotSelected(InventorySlot slot_)
    {
        currentSlotIndex = slots.IndexOf(slot_);
        buttons.SetActive(true);
        if(slot_.myItem)
        {
            currentlySelectedSlot.SetItem(slot_.myItem,slot_.amount);
            int x = 0;
            if (inventoryUI.GetSlotWithName(slot_.myItem.itemName) != null)
                x = inventoryUI.GetSlotWithName(slot_.myItem.itemName).amount;
            currentInventorySlot.SetItem(slot_.myItem, x);
            addingButtons.SetActive(true);
        }
        else
        {
            currentlySelectedSlot.SetNullItem();
            currentInventorySlot.SetNullItem();
            addingButtons.SetActive(false);
        }
        inventoryObject.SetActive(false);
        SetItemName();
        CalculateItemValue();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentlySelectedSlot.gameObject);
    }
    public void OpenInventorySection()
    {
        inventoryUI.OpenMenu(true);
        inventoryUI.SetClickFunctionIndex(2);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(inventoryUI.slots[0].gameObject);
    }
    public void ChangeItem(ItemData data_, int amount_)
    {
        if (amount_ > 0)
        {
            currentlySelectedSlot.SetItem(data_, 1);
            currentInventorySlot.SetItem(data_, amount_ - 1);
            addingButtons.SetActive(true);
        }
        else
        {
            currentlySelectedSlot.SetNullItem();
            currentInventorySlot.SetNullItem();
            addingButtons.SetActive(false);
        }
        SetItemName();
        CalculateItemValue();
    }
    private void SetItemName()
    {
        if (currentlySelectedSlot.myItem)
            currentItemNameText.text = currentInventorySlot.myItem.itemName;
        else
            currentItemNameText.text = "Empty";
    }
    private void CalculateItemValue()
    {
        if (currentlySelectedSlot.myItem)
        {
            currentItemValue.text = (currentlySelectedSlot.myItem.basePrice * currentlySelectedSlot.amount).ToString();
        }
        else
        {
            currentItemValue.text = "Worthless";
        }
    }
    public void ResetSelectedItem()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentlySelectedSlot.gameObject);
    }
    public void Save()
    {
        if (currentlySelectedSlot.myItem)
        {
            if (currentlySelectedSlot.amount > 0)
            {
                slots[currentSlotIndex].SetItem(currentlySelectedSlot.myItem, currentlySelectedSlot.amount);
                openBarginBin.SetSlot(currentSlotIndex, currentlySelectedSlot.myItem, currentlySelectedSlot.amount);
            }
            else
            {
                slots[currentSlotIndex].SetNullItem();
                openBarginBin.ClearSlot(currentSlotIndex);
            }
            UpdateInventoryAmount();
        }
        else
        {
            slots[currentSlotIndex].SetNullItem();
        }
        inventoryObject.SetActive(true);
        SetButtonsActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(slots[currentSlotIndex].gameObject);
    }
    public void AddAmountOfCurrentItem(int amount_)
    {
        if (currentInventorySlot.amount >= amount_)
        {
            currentInventorySlot.UpdateAmount(currentInventorySlot.amount -= amount_);
            currentlySelectedSlot.UpdateAmount(currentlySelectedSlot.amount += amount_);
            CalculateItemValue();
        }

    }
    public void AddMaxAmountOfCurrentItem()
    {
        AddAmountOfCurrentItem(currentInventorySlot.amount);

    }
    public void AddHalfAmountOfCurrentItem()
    {
        if (currentInventorySlot.amount == 1)
        {
            AddAmountOfCurrentItem(1);
            return;
        }
        AddAmountOfCurrentItem(currentInventorySlot.amount / 2);

    }
    public void ClearButton()
    {
        PutItemBackInInventory();
        currentlySelectedSlot.SetNullItem();
        currentInventorySlot.SetNullItem();
        SetItemName();
        SetButtonsActive(false);
        slots[currentSlotIndex].SetNullItem();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(slots[currentSlotIndex].gameObject);
        CalculateItemValue();
        openBarginBin.ClearSlot(currentSlotIndex);
    }
    private void PutItemBackInInventory()
    {
        if (currentlySelectedSlot.myItem)
            inventoryUI.GetSlotWithName(currentlySelectedSlot.myItem.itemName).UpdateAmount(currentInventorySlot.amount + currentlySelectedSlot.amount);
    }
    private void UpdateInventoryAmount()
    {
        if (currentlySelectedSlot.myItem)
        {
            inventoryUI.GetSlotWithName(currentlySelectedSlot.myItem.itemName).UpdateAmount(currentInventorySlot.amount);
        }
    }
}
