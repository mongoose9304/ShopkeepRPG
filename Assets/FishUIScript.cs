using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishUIScript : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public GameObject inventoryObject;
    public FishStorage storage;
    [SerializeField] int clickFunctionIndex;

    //TownInventory stuff
    public TextMeshProUGUI selectedItemNameText;
    public TextMeshProUGUI selectedItemNameDescription;
    public GameObject selectedItemDisplay;
    public Image selectedItemImage;


    public void Activate()
    {
        LoadInventory();
        OpenMenu(true);
    }
    private void LoadInventory()
    {
        int index = 0;
        foreach (InventorySlot slot_ in slots)
        {
            slot_.Clear();
        }

        Debug.Log("Loading Inventory");

        foreach (Fish fish_ in storage.allFish)
        {
            slots[index].SetFish(fish_.GetName(), fish_.size);
            slots[index].gameObject.SetActive(true);
            index += 1;
        }
    }

    private InventorySlot GetSlotWithName(string name_)
    {
        foreach (InventorySlot slot_ in slots)
        {
            if (!slot_.myItem)
                continue;
            if (slot_.myItem.itemName == name_)
                return slot_;
        }
        return null;
    }
    public void OpenMenu(bool open_ = true)
    {
        inventoryObject.SetActive(open_);
    }

    public void SetClickFunctionIndex(int index_)
    {
        clickFunctionIndex = index_;
    }
    public void AddItemToInventory(ItemData item_, int amount_)
    {
        if (item_ == null)
        {
            return;
        }
        bool hasFoundItem = false;
        foreach (InventorySlot slot_ in slots)
        {
            if (!slot_.myItem)
                continue;
            if (slot_.myItem.itemName == item_.itemName)
            {
                slot_.UpdateAmount(slot_.amount + amount_);
                hasFoundItem = true;
                Debug.Log("ReturnedItem");
            }
        }
        if (!hasFoundItem)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].myItem == null)
                {
                    slots[i].SetItem(item_, amount_);
                    slots[i].gameObject.SetActive(true);
                    break;
                }
            }

        }
    }
    public void ClearAllSlots()
    {
        foreach (InventorySlot slot_ in slots)
        {
            slot_.Clear();
        }
    }
    public void SetDisplayedItem(InventorySlot slot_)
    {
        if (selectedItemDisplay == null)
            return;
        if (slot_.myItem == null)
            return;
        selectedItemNameText.text = slot_.myItem.itemName;
        selectedItemNameDescription.text = slot_.myItem.description;
        selectedItemImage.sprite = slot_.myItem.itemSprite;
        selectedItemImage.color = slot_.myItem.itemColor;
        selectedItemDisplay.SetActive(true);
    }
}
