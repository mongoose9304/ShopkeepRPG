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
    [SerializeField] float damage;
    [SerializeField] bool isBoss;
    private void OnTriggerEnter(Collider other)
    {
        if(isBoss)
        {
            if (other.tag == "Explosion")
            {
                gameObject.SetActive(false);
                GetComponentInParent<TumbleSection>().CheckObjects();

            }
            return;
        }
        if (!canDrop)
        {
            if (other.tag == "Explosion")
            {
                gameObject.SetActive(false);
                if(isHead)
                {
                    TumbleTowerEnemy enemy = GetComponentInParent<TumbleTowerEnemy>();
                    enemy.Death();
                  
                }
             
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
              
            }
            if (other.tag == "Pickaxe")
            {
                dropper.DropItems();
                gameObject.SetActive(false);
             
            }
        }  
        if(other.tag=="Player")
        {
            other.gameObject.GetComponent<MiningPlayer>().TakeDamage(damage);
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
