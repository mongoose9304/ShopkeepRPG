using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInNearExitDetector : MonoBehaviour
{
    public GameObject objectToToggle; 
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if (!other.GetComponent<StorePlayer>().isPlayer2)
            {
                ShopManager.instance.PlayerIsNearExit = true;
                objectToToggle.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponent<StorePlayer>().isPlayer2)
            {
                ShopManager.instance.PlayerIsNearExit = false;
                objectToToggle.SetActive(false);
            }
        }
    }
    public void OpenShop()
    {
        ShopManager.instance.PlayerIsNearExit = false;
        objectToToggle.SetActive(false);
        gameObject.SetActive(false);
    }
    public void CloseShop()
    {
        ShopManager.instance.PlayerIsNearExit = true;
        objectToToggle.SetActive(true);
        gameObject.SetActive(true);
    }
}
