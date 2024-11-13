using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableInventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();
    public InventorySlot heldSlot;
    public GameObject inventoryObject;
    private void Start()
    {
        LoadInventory();
    }
    public void LoadInventory()
    {
        int index = 0;
        foreach (InventorySlot slot_ in slots)
        {
            slot_.Clear();
        }
        foreach (MoveableItem item_ in PlayerInventory.instance.masterMoveableItemList)
        {
            if (item_.myItem)
            {
                if (item_.amount == 0)
                    continue;
                slots[index].SetMoveableItem(item_.myItem, item_.amount);
                slots[index].gameObject.SetActive(true);
                index += 1;
            }
        }
    }
    public void InventoryButtonClicked(InventorySlot slot_)
    {
        heldSlot.SetMoveableItem(slot_.myMoveableObject, 1);
        slot_.UpdateAmount(slot_.amount - 1);
    }
}
