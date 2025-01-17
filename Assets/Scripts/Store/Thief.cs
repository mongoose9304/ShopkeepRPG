using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// A NPC that tries to take your items or money in the shop minigame
/// </summary>
public class Thief : MonoBehaviour
{
    public bool isInHell;
    [Tooltip("Speed the npc moves")]
    [SerializeField] float speed;
    [Tooltip("Speed the npc moves when the player is in another zone")]
    [SerializeField] float slowSpeed;
    private bool hasStolenMoney;
    [Tooltip("the % chance an npc will try to rob the store's backroom before leaving")]
    public float chanceToGoForStoreCash;
    public NavMeshAgent myAgent;
    public ItemData stolenItem;
    public int stolenItemAmount;
    public int stolenCash;
    public ParticleSystem startStealEffect;
    [SerializeField] List<TempItem> heldItems = new List<TempItem>();
    /// <summary>
    /// Flee the shop
    /// </summary>
    public void Flee()
    {
        myAgent.SetDestination(ShopManager.instance.GetRandomNPCExit(isInHell).transform.position);
        CheckSpeed();
    }
    public void SetTarget(Transform target_)
    {
        myAgent.SetDestination(target_.position);
    }
    public void CheckSpeed()
    {
        if(ShopManager.instance.twoPlayerMode)
        {
            myAgent.speed = speed;
            return;
        }
        if (isInHell != ShopManager.instance.playerInHell)
        {
            myAgent.speed = slowSpeed;
        }
        else
        {
            myAgent.speed = speed;
        }
    }
    /// <summary>
    /// try to rob the backroom before leaving
    /// </summary>
    public void HeadToStoreRoom()
    {
        myAgent.SetDestination(ShopManager.instance.GetStoreRoom(isInHell).transform.position);
    }
    public void StealItem(ItemData item_,int amount_)
    {
        hasStolenMoney = false;
        startStealEffect.Play();
        stolenItem = item_;
        stolenItemAmount = amount_;
        if (Random.Range(0, 1.0f) > chanceToGoForStoreCash)
        {
            Flee();
        }
        else
        {
            HeadToStoreRoom();
        }
        ShopManager.instance.SetStealAlert(true, isInHell);
    }
    public void StealMoney()
    {
        if (hasStolenMoney)
            return;
        hasStolenMoney = true;
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
    /// <summary>
    /// The player has caught the thief, they return the items /money
    /// </summary>
    public void Caught()
    {
        if (ShopTutorialManager.instance.inTut)
        {
            ShopTutorialManager.instance.SetTutorialState(8);
            gameObject.SetActive(false);
            return;
        }
        if (stolenItem)
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

        CustomerManager.instance.CaughtThief(isInHell);
        CustomerManager.instance.PlayEmote(2, transform);
        ShopManager.instance.currentThieves.Remove(this);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndZone")
        {
            gameObject.SetActive(false);
            if (!ShopTutorialManager.instance.inTut)
            {
                ShopManager.instance.currentThieves.Remove(this);
                CustomerManager.instance.CaughtThief(isInHell, true);
            }
            else
            {
                ShopTutorialManager.instance.CreateSmoke(transform);
                ShopTutorialManager.instance.SetAltTutorialState(8);
            }
        }
        if (other.tag == "StoreRoom")
        {
            StealMoney();
            Flee();
        }
    }
    public void SetHeldItems(List<TempItem> heldItems_)
    {
        heldItems = heldItems_;
    }
}
