using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // inventory backup
    public List<Item> items;
    // Inventory pages for displaying the items in the ui
    public List<GameObject> inventoryPages;
    // inventory ui
    public GameObject InventoryParent;
    // prefab for an inventory cell
    public GameObject inventoryCell;

    public GameObject currentPedestal;
    private bool isInventoryOpen = false;


    public bool isOpen(){
        return isInventoryOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                AddItem(item);
                Destroy(other.gameObject);
            }
        }
        else if (other.CompareTag("Pedestal"))
        {
            currentPedestal = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pedestal") && other.gameObject == currentPedestal)
        {
            currentPedestal = null;
        }
    }


    public bool PlaceItemOnPedestal(Item item) {
        if (currentPedestal != null)
        {
            ItemPedestal pedestal = currentPedestal.GetComponent<ItemPedestal>();
            if (pedestal != null && pedestal.item == null)
            {
                pedestal.AddItem(item);
                items.Remove(item);
                Debug.Log("Placed " + item.itemName + " on pedestal.");

                ToggleInventory();
                return true;
            }
            Debug.Log("Error: pedestal already has an item");
            return false;
        }
        else {
            Debug.Log("Error: no pedestal selected");
            return false;
        }
    }

    public void AddItem(Item item) {   
        items.Add(item);
        Debug.Log("Added " + item.name + " to inventory.");

        // add item to the ui, the item cell prefab has an image and two text objects, name and price
        GameObject cell = Instantiate(inventoryCell, inventoryPages[(int)item.type].transform.GetChild(0).transform);
        if (cell != null){
            cell.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
            cell.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = item.basePrice.ToString();
            cell.transform.GetChild(3).GetComponent<Item>().SetItem(item);
            Button placeButton = cell.transform.GetChild(3).transform.GetChild(0).GetComponent<Button>();
            Button removeButton = cell.transform.GetChild(3).transform.GetChild(1).GetComponent<Button>();

             if (placeButton != null && removeButton != null) {
                // Add event listeners to the buttons, this is very important to do IN the script, not in the editor
                // or else it won't work
                placeButton.onClick.AddListener(() => InventoryCellPlace(cell));
                removeButton.onClick.AddListener(() => InventoryCellRemove(cell));
            }
            else
            {
                Debug.Log("Error: Cell buttons not found.");
            }
        }
        else {
            Debug.Log("Error: could not instantiate inventory cell");
        }
    }
    
    // Start is called before the first frame update
    void Start() {

        foreach(GameObject page in inventoryPages){
            page.SetActive(false);
        }
        inventoryPages[0].SetActive(true);
        InventoryParent.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (currentPedestal != null && Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
            Debug.Log(currentPedestal.name);
        }
    }

    public void ToggleInventory() {
        isInventoryOpen = !isInventoryOpen;
        InventoryParent.SetActive(isInventoryOpen);

        if (!isInventoryOpen)
        {
            currentPedestal = null;
        }
    }

    public void InventoryCellPlace(GameObject cell_){
        Item item_ = cell_.transform.GetChild(3).GetComponent<Item>();
        bool itemPlaced = PlaceItemOnPedestal(item_);
        if (itemPlaced){
            Destroy(cell_);
        }
    }

    public void InventoryCellRemove(GameObject cell_){
        Destroy(cell_);
    }

    // use this function to update the entire ui
    public void UpdateInventoryUI() {
    }
}
