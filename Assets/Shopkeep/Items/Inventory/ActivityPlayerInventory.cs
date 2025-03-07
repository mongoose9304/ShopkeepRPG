using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class ActivityPlayerInventory : InventoryDefault
    {
        //.
        public void TransferInventory() 
        {
            Inventory itemsGlobal = ManagerGlobal.GetManager<GlobalPlayerInventory>();
            foreach (var item in items) 
            {
                itemsGlobal.AddItemStack(item);
            }
            
        }
    }
}
