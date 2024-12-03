using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
/// <summary>
/// UI screen that is activated when a player tries to haggle at a pedestal with an NPC
/// </summary>
public class HaggleUI : MonoBehaviour
{
    public bool isPlayer2;
    [Tooltip("UI for the current object being haggled over")]
    public InventorySlot currentHaggleSlot;
    [Tooltip("The current pedestal we are haggling at")]
    public Pedestal openPedestal;
    [Tooltip("The customer we are haggling with")]
    public Customer currentCustomer;
    [Tooltip("current % to sell at the player has set")]
    public float currentHaggleAmount;
    [Tooltip("current total price to sell at the player has set")]
    public int currentSellValue;
    [Tooltip("Small delay between Audio of moving the haggle slider")]
    [SerializeField] float maxTimebetweenSliderAudios;
    [Tooltip("REFERNCE to the slider that is used when haggling")]
    public Slider haggleSlider;
    [Tooltip("REFERNCE to the slider controller that is used when haggling")]
    public SliderGamepadController haggleSliderController;
    [Tooltip("REFERENCE to the sell button")]
    public GameObject sellButton;
    [Tooltip("REFERENCE to the UI text for the current item's name")]
    public TextMeshProUGUI currentItemNameText;
    [Tooltip("REFERENCE to the UI text for the current item's price before haggles")]
    public TextMeshProUGUI currentItemValue;
    [Tooltip("REFERENCE to the UI text for the current item's price after haggles")]
    public TextMeshProUGUI haggleItemValue;
    [Tooltip("REFERENCE to the UI text for the dialogue box")]
    public TextMeshProUGUI dialogueText;
    [Tooltip("REFERENCE to the emojis to display NPC mood on the current haggle")]
    public List<Sprite> emojis = new List<Sprite>();
    [Tooltip("REFERENCE to the image that changes with the NPCs mood on the current haggle price")]
    public Image SliderEmoji;
    private float currentTimebetweenSliderAudios;
    public InputSystemUIInputModule model;
    private void Update()
    {
        if (currentTimebetweenSliderAudios > 0)
            currentTimebetweenSliderAudios -= Time.deltaTime;
    }
    /// <summary>
    /// Open the menu and set up the currently selected UI object/all the UI
    /// </summary>
    public void OpenMenu(Pedestal p_,Customer c_,float haggleStart_, bool isPlayer2 = false)
    {
        if (isPlayer2 == false)
            model.actionsAsset = PlayerManager.instance.GetPlayers()[0].input.actions;
        else
            model.actionsAsset = PlayerManager.instance.GetPlayers()[1].input.actions;
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
    /// <summary>
    /// Set the haggle amount. This is used by the UI slider 
    /// </summary>
    public void SetHaggleAmount(float amount_)
    {
        currentHaggleAmount = amount_;
        CalculateHagglePrice();
        ChangeHaggleSliderEmotion(amount_);
    }
    /// <summary>
    /// Do the math to seee how much the item will cost based on the % discounted
    /// </summary>
    private void CalculateHagglePrice()
    {
        currentSellValue = Mathf.RoundToInt(openPedestal.GetItemCost() * currentHaggleAmount);
        haggleItemValue.text = currentSellValue.ToString();
    }
    /// <summary>
    /// The actions taken when the player presses the sell button. If the price works for the NPC close the menu and sell the item
    /// </summary>
    public void Sell()
    {
        //0 = deal accepted, 1= way too high, 2 = mood too low
        switch (currentCustomer.AttemptHaggle(currentSellValue, currentHaggleAmount,openPedestal.hotItem,openPedestal.coldItem))
        {
            case 0:
                currentCustomer.EndHaggle(currentSellValue);
                openPedestal.ItemSold();
                ShopManager.instance.AddCash(currentSellValue,currentCustomer.isInHell);
                ShopManager.instance.CloseMenu(isPlayer2);
                break;
            case 1:
                RandomWayTooHigh();
                break;
            case 2:
                RandomBitTooHigh();
                break;
        }
    }
    /// <summary>
    /// The actions taken when the player presses the no deal button. Close the menu and send the NPC away
    /// </summary>
    public void NoDeal()
    {if (!ShopTutorialManager.instance.inTut)
        {
            currentCustomer.ForceEndHaggle();
            openPedestal.SetInUse(false);
            ShopManager.instance.CloseMenu(isPlayer2);
        }
        else
        {
            currentCustomer.ForceEndHaggle();
            ShopManager.instance.CloseMenu(isPlayer2);
        }
    }
    /// <summary>
    /// The actions taken when the player presses the small talk button. Raise the NPC's mood 
    /// </summary>
    public void SmallTalk()
    {
        if (!currentCustomer.hasBeenSmallTalked)
        {
            currentCustomer.SmallTalk();
            dialogueText.gameObject.SetActive(false);
            dialogueText.text = currentCustomer.smallTalks[Random.Range(0, currentCustomer.smallTalks.Count)];
            dialogueText.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Display one of the NPC's random greetings when opening the menu
    /// </summary>
    private void RandomGreeting()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = currentCustomer.greetings[Random.Range(0, currentCustomer.greetings.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    /// <summary>
    /// Display one of the NPC's random bit too high messages
    /// </summary>
    private void RandomBitTooHigh()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueText.text = currentCustomer.bitTooHigh[Random.Range(0, currentCustomer.bitTooHigh.Count)];
        dialogueText.gameObject.SetActive(true);
    }
    /// <summary>
    /// Display one of the NPC's random way too high messages
    /// </summary>
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
    /// <summary>
    ///Chnage the emotion of the haggle slider. 0 = superHappy, 1= super pissed, 2 = little pissed,3 little happy, 4 normal
    /// </summary>
    private void ChangeHaggleSliderEmotion(float haggleAmount)
    {
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
