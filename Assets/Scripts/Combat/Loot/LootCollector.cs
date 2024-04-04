using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       
            if (other.tag == "Item")
            {
            LootManager.instance.AddLootItem(other.GetComponent<LootWorldObject>().myItem);
            Destroy(other.gameObject);
            }
        
    }
}
