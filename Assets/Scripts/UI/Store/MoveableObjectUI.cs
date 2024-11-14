using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveableObjectUI : MonoBehaviour
{
    public InventorySlot currentItemSlot;
    public MoveableInventoryUI invUI;
    public TextMeshProUGUI currentItemNameText;
    public void OpenMenu()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentItemSlot.gameObject);
        invUI.inventoryObject.SetActive(false);
        if(ShopManager.instance.GetPlayerHeldMoveableItem()!=null)
        {
            currentItemSlot.SetMoveableItem(ShopManager.instance.GetPlayerHeldMoveableItem(), 1);
            Debug.Log("FOundHeldItem");
        }
        else
        {
            currentItemSlot.SetNullItem();
        }
        if (currentItemSlot.myMoveableObject)
        {
            currentItemNameText.text = currentItemSlot.myMoveableObject.myName;
        }
        else
        {
            currentItemNameText.text = "";
        }
    }
    public void CloseMenu()
    {

    }
    public void PutItemAway()
    {
        if(currentItemSlot.myMoveableObject)
        {
            invUI.AddItemToInventory(currentItemSlot.myMoveableObject, 1);
            currentItemSlot.myMoveableObject = null;
            currentItemSlot.SetNullItem();
        }
    }
    public void SaveItem()
    {
        ShopManager.instance.SetPlayerHeldMoveableItem(currentItemSlot.myMoveableObject);
    }
    public void OpenInventorySection()
    {
        invUI.inventoryObject.SetActive(true);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(invUI.slots[0].gameObject);
    }
    public void ChangeItem(MoveableObject obj)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentItemSlot.gameObject);
        currentItemSlot.SetMoveableItem(obj, 1);
        invUI.inventoryObject.SetActive(false);
    }
}
