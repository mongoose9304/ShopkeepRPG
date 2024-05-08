using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public List<Item> items;
    // Inventory pages for displaying the items in the ui
    public List<GameObject> inventoryPages;

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
                other.gameObject.SetActive(false); // Deactivate the GameObject
                // or
                // Destroy(other.gameObject); // Destroy the GameObject
            }
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        Debug.Log("Added " + item.name + " to inventory.");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
