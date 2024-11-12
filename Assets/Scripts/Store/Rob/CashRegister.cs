using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : InteractableObject
{
    public GameObject[] waitingLocations;
    public List<Customer> customersWaiting = new List<Customer>();
    public float timeBetweenUses = 0;
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
    public void Use()
    {
        if(customersWaiting.Count>0)
        {
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
    public void AddCustomer(Customer c_)
    {
        customersWaiting.Add(c_);
    }
    public void SetCustomerTargets()
    {
        for(int i=0;i<customersWaiting.Count;i++)
        {
            customersWaiting[i].SetTarget(waitingLocations[i]);
        }
    }

}
