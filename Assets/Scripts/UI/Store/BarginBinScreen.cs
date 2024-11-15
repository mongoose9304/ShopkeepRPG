using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoreMountains.Tools;

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
    public GameObject refillButtons;
    public TextMeshProUGUI currentItemNameText;
    public TextMeshProUGUI currentItemValue;
    public List<InventorySlot> slots = new List<InventorySlot>();
    int currentSlotIndex;
    public TextMeshProUGUI discountUIText;
    public Slider discountSlider;
    [SerializeField] AudioClip placeItemAudio;
    [SerializeField] AudioClip takeItemAudio;
    [SerializeField] private float maxTimebetweenSliderAudios;
    [SerializeField] private float currentTimebetweenSliderAudios;
    private void Update()
    {
        if (currentTimebetweenSliderAudios > 0)
            currentTimebetweenSliderAudios -= Time.deltaTime;
    }
    public void OpenMenu(BarginBin bin_)
    {
        openBarginBin = bin_;
        LoadInventory(bin_);
        SetButtonsActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(slots[0].gameObject);
        discountSlider.value = openBarginBin.itemDiscount;
        SetDiscountAmount(openBarginBin.itemDiscount);
        refillButtons.SetActive(true);
        currentTimebetweenSliderAudios = 0;

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
        refillButtons.SetActive(false);
        if (slot_.myItem)
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
    public void CloseMenu()
    {
        if(openBarginBin)
        {
            openBarginBin.ApplyDiscountToAllItems();
            CustomerManager.instance.CheckBarginBinsForItems();
        }
        openBarginBin = null;
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
            // currentItemValue.text = (Mathf.RoundToInt(currentlySelectedSlot.myItem.basePrice * currentlySelectedSlot.amount / (1-openBarginBin.itemDiscount))).ToString();
            int x = Mathf.RoundToInt(currentlySelectedSlot.myItem.basePrice * currentlySelectedSlot.amount * (1 - openBarginBin.itemDiscount));
            currentItemValue.text = x.ToString();
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
                openBarginBin.SetPreviousSlot(currentSlotIndex, currentlySelectedSlot.myItem, currentlySelectedSlot.amount);
            }
            else
            {
                slots[currentSlotIndex].SetNullItem();
                openBarginBin.ClearSlot(currentSlotIndex);
            }
            UpdateInventoryAmount();
            openBarginBin.UpdateSlotsWithItems();
            MMSoundManager.Instance.PlaySound(placeItemAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
        else
        {
            slots[currentSlotIndex].SetNullItem();
        }
        inventoryObject.SetActive(true);
        SetButtonsActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(slots[currentSlotIndex].gameObject);
    }
    public void RefillItems()
    {
        for(int i=0;i<slots.Count;i++)
        {
            if(openBarginBin.binSlotsPrevious[i].myItem!=null)
            {
                int x = 0;
                if(inventoryUI.GetSlotWithName(openBarginBin.binSlotsPrevious[i].myItem.itemName))
                {
                    x = inventoryUI.GetSlotWithName(openBarginBin.binSlotsPrevious[i].myItem.itemName).amount;
                }
                 
                if (x >= openBarginBin.binSlotsPrevious[i].amount)
                {
                    if(slots[i].amount>0)
                    {
                        Debug.Log("Return Item: " + inventoryUI.GetSlotWithName(slots[i].myItem.itemName).amount + slots[i].amount);
                        inventoryUI.GetSlotWithName(slots[i].myItem.itemName).UpdateAmount(inventoryUI.GetSlotWithName(slots[i].myItem.itemName).amount + slots[i].amount);
                    }
                    slots[i].SetItem(openBarginBin.binSlotsPrevious[i].myItem, openBarginBin.binSlotsPrevious[i].amount);
                    inventoryUI.GetSlotWithName(slots[i].myItem.itemName).UpdateAmount(inventoryUI.GetSlotWithName(slots[i].myItem.itemName).amount - slots[i].amount);
                    openBarginBin.SetSlot(i, slots[i].myItem, slots[i].amount);
                }
            }
        }
        openBarginBin.UpdateSlotsWithItems();

    }
    public void ClearAllItems()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].amount > 0)
            {
                if (inventoryUI.GetSlotWithName(slots[i].myItem.itemName))
                inventoryUI.GetSlotWithName(slots[i].myItem.itemName).UpdateAmount(inventoryUI.GetSlotWithName(slots[i].myItem.itemName).amount + slots[i].amount);
                else
                {
                    inventoryUI.AddItemToInventory(slots[i].myItem, slots[i].amount);
                }
            }
            slots[i].SetNullItem();
            openBarginBin.ClearSlot(i);
        }
        openBarginBin.UpdateSlotsWithItems();
        MMSoundManager.Instance.PlaySound(takeItemAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
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
        MMSoundManager.Instance.PlaySound(takeItemAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
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
    public void SetDiscountAmount(float amount_)
    {

        discountUIText.text = (Mathf.Round(amount_ * 100.0f)).ToString();
        openBarginBin.SetBinDiscountAmount(amount_);
    }
    public void PlayAudio(string audio_)
    {
        if (ShopManager.instance)
        {
            ShopManager.instance.PlayUIAudio(audio_);
        }
    }
    public void PlaySliderAudio(string audio_)
    {
        if (ShopManager.instance)
        {
            if (currentTimebetweenSliderAudios > 0)
                return;
            ShopManager.instance.PlayUIAudio(audio_);
            currentTimebetweenSliderAudios = maxTimebetweenSliderAudios;
        }
    }
}
