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
    }
    public void SetHaggleAmount(float amount_)
    {
        currentHaggleAmount = amount_;
        CalculateHagglePrice();
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
        dialogueText.text = greetings[Random.Range(0, greetings.Count)];
    }
    private void RandomBitTooHigh()
    {
        dialogueText.text = bitTooHigh[Random.Range(0, bitTooHigh.Count)];
    }
    private void RandomWayTooHigh()
    {
        dialogueText.text = wayTooHigh[Random.Range(0, wayTooHigh.Count)];
    }
}
