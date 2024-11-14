using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableInventoryUI : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public GameObject inventoryObject;
    public MoveableObjectUI moveableUI;
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
    public void AddItemToInventory(MoveableObject item_, int amount_)
    {
        bool hasFoundItem = false;
        foreach (InventorySlot slot_ in slots)
        {
            if (!slot_.myMoveableObject)
                continue;
            if (slot_.myMoveableObject.myName == item_.myName)
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
                if (slots[i].myMoveableObject == null)
                {
                    slots[i].SetMoveableItem(item_, amount_);
                    slots[i].gameObject.SetActive(true);
                    break;
                }
            }

        }
    }
    public void InventoryButtonClicked(InventorySlot slot_)
    {
        if (slot_.amount > 0)
        {
            moveableUI.PutItemAway();
            moveableUI.ChangeItem(slot_.myMoveableObject);
            slot_.UpdateAmount(slot_.amount - 1);
        }
    }
}
