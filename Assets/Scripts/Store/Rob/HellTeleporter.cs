using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellTeleporter : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
             ShopManager.instance.WarpPlayerToOtherStore(other.gameObject);

        }
    }
}
