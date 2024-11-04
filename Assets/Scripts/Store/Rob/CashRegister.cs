using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    public GameObject[] waitingLocations;
    public List<Customer> customersWaiting = new List<Customer>();

    public void Use()
    {
        if(customersWaiting.Count>0)
        {
            customersWaiting.RemoveAt(0);
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
