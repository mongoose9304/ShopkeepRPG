using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObjectUI : MonoBehaviour
{
    public InventorySlot currentItemSlot;
    public MoveableInventoryUI invUI;
    public void OpenMenu()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentItemSlot.gameObject);
        invUI.inventoryObject.SetActive(false);
    }
    public void CloseMenu()
    {

    }
    public void PutItemAway()
    {
        if(currentItemSlot.myMoveableObject)
        {
            currentItemSlot.myMoveableObject = null;
            currentItemSlot.SetNullItem();
        }
    }
    public void OpenInventorySection()
    {
        invUI.inventoryObject.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(invUI.slots[0].gameObject);
    }
}
