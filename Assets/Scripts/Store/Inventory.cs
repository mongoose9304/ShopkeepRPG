using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // actual inventory
    public List<Item> items;
    // Inventory pages for displaying the items in the ui
    public List<GameObject> inventoryPages;
    // inventory ui
    public GameObject InventoryParent;


    // prefab for an inventory cell
    public GameObject inventoryCell;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            // Get the Item component from the collided GameObject
            Item item = other.GetComponent<Item>();
            
            if (item != null)
            {
                // Add the item to the inventory
                AddItem(item);

                // Optionally, deactivate or destroy the collected item
                //other.gameObject.SetActive(false); // Deactivate the GameObject
                // or
                Destroy(other.gameObject); // Destroy the GameObject
            }
        }
    }

    public void AddItem(Item item)
    {   
        items.Add(item);
        Debug.Log("Added " + item.name + " to inventory.");

        // add item to the ui, the item cell prefab has an image and two text objects, name and price
        GameObject cell = Instantiate(inventoryCell, inventoryPages[0].transform.GetChild(0).transform);
        if (cell != null){
            cell.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
            cell.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = item.basePrice.ToString();
        }
        else {
            Debug.Log("Error: could not instantiate inventory cell");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject page in inventoryPages){
            page.SetActive(false);
        }
        inventoryPages[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // use this function to update the entire ui
    public void UpdateInventoryUI(){
    }
}
