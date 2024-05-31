using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public void Start(){

    }
    
    public void Update(){

    }

    public void StartStore(){
        // Show the store UI
        storeUI.SetActive(true);

        // pick a random item from the item pedestals array that is not empty
        
        // update the store ui to show the item and the base price

        // wait for the player to press the sell button

        // decide if the item is in the range that the npc will pay for (basePrice * NpcCofficient) > price > (basePrice * -NpcCoefficient) 

        // if it is then add the price to the player's inventory and remove the item from the pedestal

        // if not then npc gets +1 to anger and the player can try again, if the player runs out of tries then the npc leaves and the player keeps the item

        // repeat a few times then return control to the player
    }
    


}
