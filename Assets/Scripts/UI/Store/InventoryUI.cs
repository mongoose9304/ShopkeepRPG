using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public GameObject inventoryObject;
    public PedestalScreen pedScreen;
    public BarginBinScreen barginBinScreen;
    [SerializeField] int clickFunctionIndex;
    private void Start()
    {
        LoadInventory();
    }
    public void LoadInventory()
    {
        int index = 0;
        foreach(InventorySlot slot_ in slots)
        {
            slot_.Clear();
        }
        foreach (InventoryItem item_ in PlayerInventory.instance.masterItemList)
        {
            if(item_.myItem)
            {
                if (item_.amount == 0)
                    continue;
                slots[index].SetItem(item_.myItem, item_.amount);
                slots[index].gameObject.SetActive(true);
                index += 1;
            }
        }
    }
    public InventorySlot GetSlotWithName(string name_)
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
    public void OpenMenu(bool open_=true)
    {
        inventoryObject.SetActive(open_);
    }
    public void InventoryButtonClicked(InventorySlot slot_)
    {
        switch(clickFunctionIndex)
        {
            case 1:
                pedScreen.ChangeItem(slot_.myItem, slot_.amount);
                pedScreen.ResetSelectedItem();
                OpenMenu(false);
                break;
            case 2:
                barginBinScreen.ChangeItem(slot_.myItem, slot_.amount);
                barginBinScreen.ResetSelectedItem();
                OpenMenu(false);
                break;
            default:
                break;
        }
    }
    public void SetClickFunctionIndex(int index_)
    {
        clickFunctionIndex = index_;
    }
    public void AddItemToInventory(ItemData item_,int amount_)
    {
        bool hasFoundItem = false;
        foreach (InventorySlot slot_ in slots)
        {
            if (!slot_.myItem)
                continue;
            if(slot_.myItem.itemName==item_.itemName)
            {
                slot_.UpdateAmount(slot_.amount + amount_);
                hasFoundItem = true;
                Debug.Log("ReturnedItem");
            }
        }
        if(!hasFoundItem)
        {
            for(int i=0;i<slots.Count;i++)
            {
                if(slots[i].myItem==null)
                {
                    slots[i].SetItem(item_, amount_);
                    slots[i].gameObject.SetActive(true);
                    break;
                }
            }

        }
    }
}
