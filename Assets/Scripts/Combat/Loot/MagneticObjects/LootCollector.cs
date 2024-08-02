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
            Debug.Log("Item found " + other.GetComponent<LootWorldObject>().myItem.name);
            LootManager.instance.AddLootItem(other.GetComponent<LootWorldObject>().myItem);
            Destroy(other.gameObject);
            itemCollected.Play();
            }
        if (other.tag == "DemonCoin")
        {
            LootManager.instance.AddDemonMoney(other.GetComponent<DemonCoin>().value);
            Destroy(other.gameObject);
            coinCollected.Play();
        }
        if (other.tag == "Lumber")
        {
            LootManager.instance.AddResource(other.GetComponent<LumberPickUp>().lumberAmount);
            Destroy(other.gameObject);
            coinCollected.Play();
        }
        if (other.tag == "Stone")
        {
            LootManager.instance.AddResource(other.GetComponent<StonePickUp>().stoneAmount);
            Destroy(other.gameObject);
            coinCollected.Play();
        }
        if (other.tag == "RegularCoin")
        {
            LootManager.instance.AddRegularMoney(other.GetComponent<DemonCoin>().value);
            Destroy(other.gameObject);
            coinCollected.Play();
        }

    }
}
