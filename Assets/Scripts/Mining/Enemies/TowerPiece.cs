using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPiece : MonoBehaviour
{
    LootDropper dropper;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Explosion")
        {
            dropper = GetComponent<LootDropper>();
            dropper.DropItems();
            gameObject.SetActive(false);
        }
    }
    public void SetTowerPiece(bool canDrop_)
    {

    }
}
