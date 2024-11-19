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

    public List<Sprite> emojis = new List<Sprite>();
    public Image SliderEmoji;
    private float currentTimebetweenSliderAudios;
    [SerializeField] float maxTimebetweenSliderAudios;
    private void Update()
    {
        if (currentTimebetweenSliderAudios > 0)
            currentTimebetweenSliderAudios -= Time.deltaTime;
    }
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
        currentTimebetweenSliderAudios = 0;
    }
    public void SetHaggleAmount(float amount_)
    {
        currentHaggleAmount = amount_;
        CalculateHagglePrice();
        ChangeHaggleSliderEmotion(amount_);
    }
    private void CalculateHagglePrice()
    {
        currentSellValue = Mathf.RoundToInt(openPedestal.GetItemCost() * currentHaggleAmount);
        haggleItemValue.text = currentSellValue.ToString();
    }
    public void Sell()
    {
        //0 = deal accepted, 1= way too high, 2 = mood too low
        switch (currentCustomer.AttemptHaggle(currentSellValue, currentHaggleAmount,openPedestal.hotItem,openPedestal.coldItem))
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
    public void NoDeal()
    {
        currentCustomer.ForceEndHaggle();
        openPedestal.SetInUse(false);
        ShopManager.instance.CloseMenu();
    }
    public void SmallTalk()
    {
        currentCustomer.SmallTalk();
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = currentCustomer.smallTalks[Random.Range(0, currentCustomer.smallTalks.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    private void RandomGreeting()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = currentCustomer.greetings[Random.Range(0, currentCustomer.greetings.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    private void RandomBitTooHigh()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = currentCustomer.bitTooHigh[Random.Range(0, currentCustomer.bitTooHigh.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    private void RandomWayTooHigh()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = currentCustomer.wayTooHigh[Random.Range(0, currentCustomer.wayTooHigh.Count)];
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
