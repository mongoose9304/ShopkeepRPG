using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HaggleUI : MonoBehaviour
{
    public InventorySlot currentHaggleSlot;
    public Pedestal openPedestal;
    public Customer currentCustomer;
    public Slider haggleSlider;
    public GameObject sellButton;
    public TextMeshProUGUI currentItemNameText;
    public TextMeshProUGUI currentItemValue;
    public TextMeshProUGUI haggleItemValue;
    public float currentHaggleAmount;
    public int currentSellValue;
    public TextMeshProUGUI dialogueText;
    public List<string> greetings =new List<string>();
    public List<string> wayTooHigh = new List<string>();
    public List<string> bitTooHigh = new List<string>();
    public List<Sprite> emojis = new List<Sprite>();
    public Image SliderEmoji;
    public void OpenMenu(Pedestal p_,Customer c_,float haggleStart_)
    {
        openPedestal = p_;
        currentCustomer = c_;
        currentItemValue.text = (p_.myItem.basePrice * p_.amount).ToString();
        currentHaggleAmount = haggleStart_;
        haggleSlider.value = currentHaggleAmount;
        CalculateHagglePrice();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(sellButton);
        RandomGreeting();
        currentHaggleSlot.SetItem(openPedestal.myItem, openPedestal.amount);
        currentItemNameText.text = openPedestal.myItem.itemName;
        ChangeHaggleSliderEmotion(haggleStart_);
    }
    public void SetHaggleAmount(float amount_)
    {
        currentHaggleAmount = amount_;
        CalculateHagglePrice();
        ChangeHaggleSliderEmotion(amount_);
    }
    private void CalculateHagglePrice()
    {
        currentSellValue = Mathf.RoundToInt(openPedestal.myItem.basePrice * openPedestal.amount * currentHaggleAmount);
        haggleItemValue.text = currentSellValue.ToString();
    }
    public void Sell()
    {
        //0 = deal accepted, 1= way too high, 2 = mood too low
        switch (currentCustomer.AttemptHaggle(currentSellValue, currentHaggleAmount))
        {
            case 0:
                currentCustomer.EndHaggle(currentSellValue);
                openPedestal.ItemSold();
                ShopManager.instance.AddCash(currentSellValue,currentCustomer.isInHell);
                ShopManager.instance.CloseMenu();
                
                break;
            case 1:
                RandomWayTooHigh();
                break;
            case 2:
                RandomBitTooHigh();
                break;
        }
    }
    private void RandomGreeting()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = greetings[Random.Range(0, greetings.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    private void RandomBitTooHigh()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = bitTooHigh[Random.Range(0, bitTooHigh.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    private void RandomWayTooHigh()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = wayTooHigh[Random.Range(0, wayTooHigh.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    public void CloseMenu()
    {
        if (currentCustomer)
            currentCustomer.isInUse = false;
        currentCustomer = null;
    }
    private void ChangeHaggleSliderEmotion(float haggleAmount)
    {
        //0 = superHappy, 1= super pissed, 2 = little pissed,3 little happy, 4 normal
        switch (currentCustomer.CheckHaggle(currentSellValue, haggleAmount))
        {
            case 0:
                SliderEmoji.sprite = emojis[0];
                break;
            case 1:
                SliderEmoji.sprite = emojis[1];
                break;
            case 2:
                SliderEmoji.sprite = emojis[2];
                break;
            case 3:
                SliderEmoji.sprite = emojis[3];
                break;
            case 4:
                SliderEmoji.sprite = emojis[4];
                break;
        }
    }
}
