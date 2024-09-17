using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Item : MonoBehaviour
{
    public ItemData itemData;

    public void SetItem(ItemData item_){
        itemData = item_;
    }

    public ItemData GetItem(){
        return itemData; 
    }
   
}
