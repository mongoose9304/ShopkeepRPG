using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // inventory backup
    public List<ItemData> items;
    // Inventory pages for displaying the items in the ui
    public List<GameObject> inventoryPages;
    // inventory ui
    public GameObject inventoryParent;
    // prefab for an inventory cell
    public GameObject inventoryCell;

    public GameObject pedestalParent;

    public GameObject currentPedestal;
    private bool isInventoryOpen = false;
    private bool isPedestalUIOpen = false;


    public bool isOpen(){
        return isInventoryOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemData item = other.GetComponent<Item>().itemData;
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


    public bool PlaceItemOnPedestal(ItemData item) {
        if (currentPedestal != null)
        {
            ItemPedestal pedestal = currentPedestal.GetComponent<ItemPedestal>();
            if (pedestal != null && pedestal.isEmpty)
            {
                // This code is all over the place but from my understanding you need to resave these or they'll end up with 'missing'
                ItemData itemData = item;
                if (itemData != null){
                    pedestal.AddItem(itemData);
                    items.Remove(itemData);
                    currentPedestal.GetComponent<ItemPedestal>().UpdateUI();
                    Debug.Log("Placed " + itemData.itemName + " on pedestal.");
                    return true;
                }
                Debug.Log("Error: item is null");
                return false;
            }
            Debug.Log("Error: pedestal already has an item");
            return false;
        }
        else {
            Debug.Log("Error: no pedestal selected");
            return false;
        }
    }

    // this function is specifically for the button of the ui, when an npc grabs an item it'll be run through a different function
     public void RemoveItemFromPedestal(){
        if (currentPedestal != null){
            ItemPedestal pedestal = currentPedestal.GetComponent<ItemPedestal>();
            
            AddItem(pedestal.item);
            pedestal.RemoveItem();
            currentPedestal.GetComponent<ItemPedestal>().UpdateUI();
        }
    }

    public void AddItem(ItemData item) {   
        ItemData itemData = item;
        if (itemData != null){
            items.Add(itemData);
            Debug.Log("Added " + itemData.itemName + " to inventory.");

            // add item to the ui, the item cell prefab has an image and two text objects, name and price
            GameObject cell = Instantiate(inventoryCell, inventoryPages[(int)itemData.type].transform.GetChild(0).transform);
            if (cell != null){
                cell.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = itemData.itemName;
                cell.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = itemData.basePrice.ToString();
                cell.transform.GetChild(3).GetComponent<Item>().SetItem(itemData);
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
        else{
            Debug.Log("Error: could not find item data");
        }
    }
    
    // Start is called before the first frame update
    void Start() {

        foreach(GameObject page in inventoryPages){
            page.SetActive(false);
        }
        inventoryPages[0].SetActive(true);
        inventoryParent.SetActive(false);
        pedestalParent.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (currentPedestal != null && Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
            TogglePedestalUI();
        }
    }

    // called from the close button on the inventory panel
    public void ToggleFullInventory(){
        ToggleInventory();
        TogglePedestalUI();
        
    }

    // just for the pedestal
    public void TogglePedestalUI(){
        isPedestalUIOpen = !isPedestalUIOpen;

        if (isPedestalUIOpen){
            currentPedestal.GetComponent<ItemPedestal>().UpdateUI();
        }
        pedestalParent.SetActive(isPedestalUIOpen);
        //TODO: add sprites and the rest of the info
    }

    // just for your main inventory when placing items
    public void ToggleInventory() {
        isInventoryOpen = !isInventoryOpen;
        inventoryParent.SetActive(isInventoryOpen);

        //TODO: if the inventory is opened not from the pedstal ui, disable the place button with a foreach loop.


        if (!isInventoryOpen)
        {
            currentPedestal = null;
        }
    }

    public void InventoryCellPlace(GameObject cell_){
        ItemData item_ = cell_.transform.GetChild(3).GetComponent<Item>().itemData;
        //currentPedestal = cell_.transform.parent.gameObject;
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
