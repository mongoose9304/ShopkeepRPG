using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The cash register found at the front of the players store to sell items picked up from bargin bins
/// </summary>
public class CashRegister : InteractableObject
{
    [Tooltip("REFERENCE to the locations the NPCs can wait in line ")]
    public GameObject[] waitingLocations;
    [Tooltip("All the current NPCs waiting in line")]
    public List<Customer> customersWaiting = new List<Customer>();
    [Tooltip("TSmall delay between uses so NPCs cant be cashed out instantly ")]
    public float timeBetweenUses = 0;
    [Tooltip("If teh register has an Employee they will be cashed out automatically")]
    public bool hasEmployee;
    public AudioClip[] cashRegisterAudio;
    public ParticleSystem cashEffect;
    private void Update()
    {
        if(timeBetweenUses>0)
        timeBetweenUses -= Time.deltaTime;
        if(hasEmployee&& customersWaiting.Count>0)
        {
            Interact();
        }
    }
    public override void Interact(GameObject interactingObject_ = null)
    {
        if(timeBetweenUses<=0)
        Use();
    }
    /// <summary>
    /// The cash register found at the front of the players store to sell items picked up from bargin bins
    /// </summary>
    public void Use()
    {
        if(customersWaiting.Count>0)
        {
            //only activate if NPC is close to the register 
            if (customersWaiting[0].GetDistanceToTarget() < 0.5f)
            {
                customersWaiting[0].SellHeldItems();
                customersWaiting[0].LeaveShop();
                customersWaiting.RemoveAt(0);
                SetCustomerTargets();
                timeBetweenUses = 1;
                cashEffect.Play();
                MMSoundManager.Instance.PlaySound(cashRegisterAudio[Random.Range(0,cashRegisterAudio.Length)], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
            }
        }
    }
    /// <summary>
    /// Add customer to the waiting list
    /// </summary>
    public void AddCustomer(Customer c_)
    {
        customersWaiting.Add(c_);
    }
    /// <summary>
    /// Send each customer to thier proper waiting spot
    /// </summary>
    public void SetCustomerTargets()
    {
        for(int i=0;i<customersWaiting.Count;i++)
        {
            customersWaiting[i].SetTarget(waitingLocations[i]);
        }
    }

}
