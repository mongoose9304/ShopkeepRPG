using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPedestal : MonoBehaviour
{
    // Holds an item
    public ItemData item;
    public bool isEmpty = true;
    public bool isWindow = false;

    public GameObject pedestalParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update the UI when the item changes
    void Update()
    {
    
    }

    // manually called when any changes happen to the ui
    public void UpdateUI(){
        if (!isEmpty){
            pedestalParent.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
            pedestalParent.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "$"+item.basePrice.ToString();
            pedestalParent.transform.GetChild(3).gameObject.SetActive(true);
            if(isWindow){
                pedestalParent.transform.GetChild(4).gameObject.SetActive(true);
            }
        } else {
            pedestalParent.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "No item";
            pedestalParent.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "";
            pedestalParent.transform.GetChild(3).gameObject.SetActive(false);
            pedestalParent.transform.GetChild(4).gameObject.SetActive(false);
        }
    }

    public void AddItem (ItemData item_){
        item = item_;
        isEmpty = false;
    }

    public void RemoveItem (){
        item = null;
        isEmpty = true;
    }

    public ItemData GetItem (){
        return item;
    }
}
