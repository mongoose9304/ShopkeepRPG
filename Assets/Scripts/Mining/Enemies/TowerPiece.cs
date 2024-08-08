using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPiece : MonoBehaviour
{
    bool canDrop;
    [SerializeField] bool isHead;
    [SerializeField] Material rockMat;
    [SerializeField] Material gemMat;
    [SerializeField] float damage;
    [SerializeField] bool isBoss;
    [SerializeField] GameObject mineableObject;
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
        canDrop = canDrop_;;
        if(canDrop)
        {
        GetComponent<MeshRenderer>().material = gemMat;
            if(mineableObject)
            mineableObject.SetActive(true);
        }
        else
        {
        GetComponent<MeshRenderer>().material = rockMat;
        gameObject.tag = "Untagged";
            if (mineableObject)
                mineableObject.SetActive(false);
        }
    }
}
