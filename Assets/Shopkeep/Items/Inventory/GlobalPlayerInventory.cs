using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Shopkeeper.UniversalSaveManager;

namespace Shopkeeper
{
    public class GlobalPlayerInventory : InventoryDefault, ISaveListener
    {
        public void Save(JObject json)
        {
            JObject inventoryObject = new JObject();
            foreach (var item in items)
            {
                inventoryObject[item.item.name] = item.amount;
            }

            JObject jsonGlobalInventory = new JObject
            {
                ["Money"] = money,
                ["Items"] = inventoryObject
            };

            json["GlobalInventory"] = jsonGlobalInventory;
        }

        public void SavePre(JObject json)
        {

        }

        public void SavePost(JObject json)
        {

        }

        public void Load(JObject json)
        {
            if (json == null || json["GlobalInventory"] == null) 
            {
                //todo - new global inventory
                Debug.LogError("Load failed: No GlobalInventory in JSON.");
                return;
            }

            JObject jsonGlobalInventory = (JObject)json["GlobalInventory"];

            if (jsonGlobalInventory["Money"] != null) 
            {
                money = jsonGlobalInventory["Money"].ToObject<int>();
            }

            if (jsonGlobalInventory["Items"] != null) 
            {
                JObject inventoryItems = (JObject)jsonGlobalInventory["Items"];
                items.Clear();

                foreach (var itemStack in inventoryItems) 
                {
                    ItemStack loadedItem = new ItemStack
                    {
                        item = ItemDatabase.GetItem(itemStack.Key),
                        amount = itemStack.Value.ToObject<int>()
                    };

                    items.Add(loadedItem);
                }
            }

        }

        public void LoadPre(JObject json)
        {

        }

        public void LoadPost(JObject json)
        {

        }
    }
}

