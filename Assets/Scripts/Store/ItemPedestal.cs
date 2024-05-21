using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPedestal : MonoBehaviour
{
    // Holds an item
    public Item item;
    public bool isEmpty = true;
    public bool isWindow = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem (Item item_){
        item = item_;
        isEmpty = false;
    }

    public void RemoveItem (){
        item = null;
        isEmpty = true;
    }

    public Item GetItem (){
        return item;
    }
}
