using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Thief : MonoBehaviour
{
    public bool isInHell;
    public NavMeshAgent myAgent;
    public ItemData stolenItem;
    public int stolenItemAmount;
    public int stolenCash;
    public ParticleSystem startStealEffect;
    [SerializeField] List<TempItem> heldItems = new List<TempItem>();
    public void Flee()
    {
        myAgent.SetDestination(ShopManager.instance.GetRandomNPCExit(isInHell).transform.position);
    }
    public void StealItem(ItemData item_,int amount_)
    {
        startStealEffect.Play();
        stolenItem = item_;
        stolenItemAmount = amount_;
        Flee();
    }
    public void StealMoney()
    {
        startStealEffect.Play();
        if (!isInHell)
        {
            stolenCash = ShopManager.instance.currentCashEarned;
            ShopManager.instance.AddCash(-stolenCash,false);
        }
        else
        {
            stolenCash = ShopManager.instance.currentCashEarnedHell;
            ShopManager.instance.AddCash(-stolenCash, true);
        }
        Flee();
    }
    public void Caught()
    {
        if(stolenItem)
        {
            ShopManager.instance.ReturnItemToInventory(stolenItem, stolenItemAmount);
        }
        if(stolenCash>0)
        {
            ShopManager.instance.AddCash(stolenCash,isInHell);
        }
        if(heldItems.Count>0)
        {
            foreach(TempItem item_ in heldItems)
            {
                ShopManager.instance.ReturnItemToInventory(item_.myItem, item_.amount);
            }
            
        }

        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndZone")
        {
            gameObject.SetActive(false);
        }
    }
    public void SetHeldItems(List<TempItem> heldItems_)
    {
        heldItems = heldItems_;
    }
}
