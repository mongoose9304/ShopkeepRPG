using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollector : MonoBehaviour
{
    public ParticleSystem coinCollected;
    public ParticleSystem itemCollected;
    private void OnTriggerEnter(Collider other)
    {
       
            if (other.tag == "Item")
            {
            LootManager.instance.AddLootItem(other.GetComponent<LootWorldObject>().myItem);
            Destroy(other.gameObject);
            itemCollected.Play();
            }
        if (other.tag == "DemonCoin")
        {
            LootManager.instance.AddMoney(other.GetComponent<DemonCoin>().value);
            Destroy(other.gameObject);
            coinCollected.Play();
        }
        if (other.tag == "Lumber")
        {
            LootManager.instance.AddLumber(1);
            Destroy(other.gameObject);
            coinCollected.Play();
        }

    }
}
