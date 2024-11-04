using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : InteractableObject
{
    public GameObject[] waitingLocations;
    public List<Customer> customersWaiting = new List<Customer>();
    public float timeBetweenUses = 0;
    public bool hasEmployee;
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
