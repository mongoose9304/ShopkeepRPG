using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class ItemCollector : MonoBehaviour
    {
        public Inventory inventory;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                inventory.AddItemStack(other.GetComponent<ItemObject>().item);
                Destroy(other.gameObject);
            }
        }
    }
}
