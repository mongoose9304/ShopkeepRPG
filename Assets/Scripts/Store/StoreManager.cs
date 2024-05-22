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

}
