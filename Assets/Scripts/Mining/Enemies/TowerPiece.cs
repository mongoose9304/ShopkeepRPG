using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPiece : MonoBehaviour
{
    LootDropper dropper;
    bool canDrop;
    [SerializeField] bool isHead;
    [SerializeField] Material rockMat;
    [SerializeField] Material gemMat;
    private void OnTriggerEnter(Collider other)
    {
        if (!canDrop)
        {
            if (other.tag == "Explosion")
            {
                gameObject.SetActive(false);
                if(isHead)
                {
                    TumbleTowerEnemy enemy = GetComponentInParent<TumbleTowerEnemy>();
                    enemy.Death();
                    Debug.Log("IsHead");
                }
                Debug.Log("NoDrop");
            }
        }
        else
        {
            if (other.tag == "Explosion")
            {
                gameObject.SetActive(false);
                if (isHead)
                {
                    GetComponentInParent<TumbleTowerEnemy>().TakeDamage(1);
                }
                Debug.Log("NoDropYourFault");
            }
            if (other.tag == "Pickaxe")
            {
                dropper.DropItems();
                gameObject.SetActive(false);
                Debug.Log("Drop");
            }
        }  
    }
    public void SetTowerPiece(bool canDrop_)
    {
        if (isHead)
            return;
        canDrop = canDrop_;
        dropper = GetComponent<LootDropper>();
        dropper.dropNothing = !canDrop;
        if(canDrop)
        {
        GetComponent<MeshRenderer>().material = gemMat;
        }
        else
        {
        GetComponent<MeshRenderer>().material = rockMat;
        }
    }
}
