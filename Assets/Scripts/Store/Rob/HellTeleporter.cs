using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellTeleporter : MonoBehaviour
{
    public bool inHell;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if (inHell)
                ShopManager.instance.ExitHell(other.gameObject);
            else
                ShopManager.instance.EnterHell(other.gameObject);
        }
    }
}
