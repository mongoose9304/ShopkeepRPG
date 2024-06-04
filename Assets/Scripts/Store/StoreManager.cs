using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This class is responsible for managing the store UI and functionality. It will hold an array of gameobjects for each item pedistal
public class StoreManager : MonoBehaviour
{
    // StoreManager holds all the item pedestals, they can be empty or holding an item
    public GameObject[] itemPedestals;

    // Reference for the player shell prefab
    public GameObject player;

    // We need to know the specific checkout pedestal
    public GameObject checkoutPedestal;

    public GameObject storeUI;

    public TMP_InputField checkoutInput;

    private int randomAmount; // determines how many items will sell, maybe later weeks we can up the minimum

    public void Start(){
        storeUI.SetActive(false);

        // assuming we want to use a standard currency
        checkoutInput.onValueChanged.AddListener(ValidateCurrencyInput);
    }

    private void ValidateCurrencyInput(string input)
    {
        if (float.TryParse(input, out float value))
        {
            // Format the input to two decimal places
            checkoutInput.text = value.ToString("F2");
        }
        else
        {
            // If input is invalid, reset to last valid value
            checkoutInput.text = "0.00";
        }
    }
    
    public void Update(){

    }

    public void StartStore(){
        // Show the store UI
        storeUI.SetActive(true);

        List<GameObject> tempArray = new List<GameObject>(); // will hold the non empty pedestals

        // pick a random item from the item pedestals array that is not empty
        for(int i = 0; i < itemPedestals.Length; i++){
            if(!itemPedestals[i].GetComponent<ItemPedestal>().isEmpty){
                tempArray.Add(itemPedestals[i]);
            }
        }

        // if there are no items in the store then return
        if(tempArray.Count == 0){
            Debug.Log("No items in store");
            storeUI.SetActive(false);
            return;
        }

        // pick a random item from the tempArray
        int randomIndex = Random.Range(0, tempArray.Count);
        randomAmount = Random.Range(1, tempArray.Count);

        // set the item pedestal to the random item
        checkoutPedestal = tempArray[randomIndex];

        // update the store ui to show the item and the base price
        storeUI.transform.GetChild(0).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = checkoutPedestal.GetComponent<ItemPedestal>().item.name;
        storeUI.transform.GetChild(0).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = checkoutPedestal.GetComponent<ItemPedestal>().item.basePrice.ToString();

        // wait for the player to press the sell button
    }
    
    public void SellItem(){
        // save the inputted value
        string input = checkoutInput.text;

        if (float.TryParse(input, out float currencyValue)){
            // Save the currency value as a float
            Debug.Log("Currency Value: $" + currencyValue);}
        else{
            Debug.LogError("Invalid currency value entered.");
        }

        // decide if the item is in the range that the npc will pay for (basePrice * NpcCofficient) > price > (basePrice * 1-NpcCoefficient) 

        // if it is then add the price to the player's inventory and remove the item from the pedestal

        // if not then npc gets +1 to anger and the player can try again, if the player runs out of tries then the npc leaves and the player keeps the item

        return;
    }

    public void CloseStore(){
        checkoutPedestal = null;
        storeUI.SetActive(false);
    }
    


}
