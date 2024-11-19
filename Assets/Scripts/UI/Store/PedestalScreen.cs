using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Tools;

public class PedestalScreen : MonoBehaviour
{
    public InventorySlot currentPedestalSlot;
    public InventoryUI  inventoryUI;
    public InventorySlot currentInventorySlot;
    public InventorySlot previousItemSlot;
    public Pedestal openPedestal;
    public GameObject buttons;
    public GameObject refillButtons;
    public TextMeshProUGUI currentItemNameText;
    public TextMeshProUGUI previousItemNameText;
    public TextMeshProUGUI currentItemValue;
    [SerializeField] AudioClip placeItemAudio;
    [SerializeField] AudioClip takeItemAudio;
    public void OpenMenu(Pedestal p_)
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentPedestalSlot.gameObject);
        if (p_.myItemPrevious)
        {
            previousItemSlot.SetItem(p_.myItemPrevious, p_.amountPrevious);
            previousItemNameText.text = previousItemSlot.myItem.itemName;
        }
        else
        {
            previousItemSlot.SetNullItem();
        }
        openPedestal = p_;
        if (p_.myItem != null && p_.amount > 0)
        {
            currentPedestalSlot.SetItem(p_.myItem, p_.amount);
            int x = 0;
            if (inventoryUI.GetSlotWithName(p_.myItem.itemName)!=null)
            x = inventoryUI.GetSlotWithName(p_.myItem.itemName).amount;
            currentInventorySlot.SetItem(p_.myItem, x);
            SetButtonsActive(true);
            refillButtons.SetActive(false);

        }
        else if (p_.myItem != null && p_.amount == 0)
        {
            currentPedestalSlot.SetItem(p_.myItem, p_.amount);
            currentInventorySlot.myItem = p_.myItem;
            currentInventorySlot.amount = inventoryUI.GetSlotWithName(p_.myItem.itemName).amount;
            SetButtonsActive(true);
            refillButtons.SetActive(false);
        }
        else
        {
            currentPedestalSlot.SetNullItem();
            currentInventorySlot.SetNullItem();
            SetButtonsActive(false);
            if(previousItemSlot.myItem!=null)
            refillButtons.SetActive(true);
            else
                refillButtons.SetActive(false);
        }
        SetItemName();
        CalculateItemValue();
        
    }
    public void AddAmountOfCurrentItem(int amount_)
    {
       if (currentInventorySlot.amount >= amount_)
        {
            currentInventorySlot.UpdateAmount(currentInventorySlot.amount -= amount_);
            currentPedestalSlot.UpdateAmount(currentPedestalSlot.amount += amount_);
            CalculateItemValue();
        }

    }
    public void AddMaxAmountOfCurrentItem()
    {
        AddAmountOfCurrentItem(currentInventorySlot.amount);

    }
    public void AddHalfAmountOfCurrentItem()
    {
        if(currentInventorySlot.amount==1)
        {
            AddAmountOfCurrentItem(1);
            return;
        }
        AddAmountOfCurrentItem(currentInventorySlot.amount/2);

    }
    public void ClearButton()
    {
        PutItemBackInInventory();
        currentPedestalSlot.SetNullItem();
        currentInventorySlot.SetNullItem();
        SetItemName();
        SetButtonsActive(false);
        openPedestal.ClearItem();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentPedestalSlot.gameObject);
        CalculateItemValue();
        if (previousItemSlot.myItem != null)
            refillButtons.SetActive(true);
        else
            refillButtons.SetActive(false);
    }
    public void SetButtonsActive(bool isActive_=true)
    {
        buttons.SetActive(isActive_);
    }
    public void OpenInventorySection()
    {
        inventoryUI.OpenMenu(true,openPedestal.inHell);
        inventoryUI.SetClickFunctionIndex(1);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(inventoryUI.slots[0].gameObject);
        refillButtons.SetActive(false);
    }
    public void ChangeItem(ItemData data_,int amount_)
    {
        if (amount_ >0)
        {
            currentPedestalSlot.SetItem(data_, 1);
            currentInventorySlot.SetItem(data_, amount_ - 1);
            SetButtonsActive(true);
        }
        else
        {
            currentPedestalSlot.SetNullItem();
            currentInventorySlot.SetNullItem();
            SetButtonsActive(false);
        }
            SetItemName();
        CalculateItemValue();
    }
    public void RefillItem()
    {
        if(previousItemSlot.myItem)
        {
            int x = inventoryUI.GetSlotWithName(previousItemSlot.myItem.itemName).amount;
           if (x >=previousItemSlot.amount)
            {
                PutItemBackInInventory();
                currentPedestalSlot.SetItem(previousItemSlot.myItem, previousItemSlot.amount);
                currentInventorySlot.SetItem(previousItemSlot.myItem, inventoryUI.GetSlotWithName(previousItemSlot.myItem.itemName).amount - previousItemSlot.amount);
                SetItemName();
                CalculateItemValue();
                Save();
            }
        }
    }
    public void ResetSelectedItem()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(currentPedestalSlot.gameObject);
    }
    private void SetItemName()
    {
        if (currentPedestalSlot.myItem)
            currentItemNameText.text = currentInventorySlot.myItem.itemName;
        else
            currentItemNameText.text = "Empty";
    }
    public void Save()
    {
        if(currentPedestalSlot.myItem)
        {
            if (currentPedestalSlot.amount > 0)
            {
                openPedestal.SetItem(currentPedestalSlot.myItem, currentPedestalSlot.amount);
                openPedestal.SetPreviousItem(currentPedestalSlot.myItem, currentPedestalSlot.amount);
            }
            else
                openPedestal.ClearItem();
            UpdateInventoryAmount();

        }
        else
        {
            openPedestal.ClearItem();
        }
        ShopManager.instance.CloseMenu();
        CustomerManager.instance.CheckPedestalsforItems();
        MMSoundManager.Instance.PlaySound(placeItemAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    private void PutItemBackInInventory()
    {
        if (currentPedestalSlot.myItem)
        {
            if(inventoryUI.GetSlotWithName(currentPedestalSlot.myItem.itemName))
            inventoryUI.GetSlotWithName(currentPedestalSlot.myItem.itemName).UpdateAmount(currentInventorySlot.amount + currentPedestalSlot.amount);
            else
            {
                inventoryUI.AddItemToInventory(currentPedestalSlot.myItem, currentPedestalSlot.amount);
            }
            MMSoundManager.Instance.PlaySound(takeItemAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
    }
    private void UpdateInventoryAmount()
    {
        if (currentPedestalSlot.myItem)
        {
            inventoryUI.GetSlotWithName(currentPedestalSlot.myItem.itemName).UpdateAmount(currentInventorySlot.amount);
        }
    }
    private void CalculateItemValue()
    {
        if (currentPedestalSlot.myItem)
        {
            switch (ShopManager.instance.CheckIfItemIsHot(currentPedestalSlot.myItem, openPedestal.inHell))
            {
                case 0://normal
                    currentItemValue.text = (currentPedestalSlot.myItem.basePrice * currentPedestalSlot.amount).ToString();
                    break;
                case 1://hot
                    currentItemValue.text = (currentPedestalSlot.myItem.basePrice * currentPedestalSlot.amount*ShopManager.instance.GetHotItemMultiplier()).ToString();
                    break;
                case 2://cold
                    currentItemValue.text = (currentPedestalSlot.myItem.basePrice * currentPedestalSlot.amount * ShopManager.instance.GetColdItemMultiplier()).ToString();
                    break;
            }
            
        }
        else
        {
            currentItemValue.text = "Worthless";
        }
    }
    public void PlayAudio(string audio_)
    {
        if(ShopManager.instance)
        {
            ShopManager.instance.PlayUIAudio(audio_);
        }
    }
}
