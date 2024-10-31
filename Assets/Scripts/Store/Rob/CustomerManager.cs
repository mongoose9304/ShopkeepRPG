using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    public int averageCustomerCash;
    public float averageCustomerMood;
    public float chanceToCheckWindowsFirst;
    [SerializeField] float minTimeBetweenCustomerSpawns;
    [SerializeField] float maxTimeBetweenCustomerSpawns;
    [SerializeField] float currentTimeBetweenCustomerSpawns;
    [SerializeField]int customerCount;
    [SerializeField]int maxCustomers;
    [SerializeField]protected MMMiniObjectPooler basicCustomerPool;
    public Transform[] customerSpawns;
    public List<Pedestal> regularPedestalsWithItems = new List<Pedestal>();
    public List<Pedestal> windowPedestalsWithItems = new List<Pedestal>();
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        currentTimeBetweenCustomerSpawns -= Time.deltaTime;
        if(currentTimeBetweenCustomerSpawns<=0)
        {
            SpawnRandomCustomer();
        }
    }
    public void OpenShop(int maxCustomers_,int burstCustomers_)
    {
        CheckPedestalsforItems();
        maxCustomers = maxCustomers_;
        customerCount = 0;
    }
    public void SpawnRandomCustomer()
    {
        currentTimeBetweenCustomerSpawns = maxTimeBetweenCustomerSpawns;
        if(customerCount>=maxCustomers)
        {
            return;
        }
        customerCount += 1;
        SpawnBasicCustomer();
    }
    private void SpawnBasicCustomer()
    {
        Customer c = basicCustomerPool.GetPooledGameObject().GetComponent<Customer>();
        c.cashOnHand = Mathf.RoundToInt(averageCustomerCash * Random.Range(0.8f, 1.2f));
        c.mood = averageCustomerMood * Random.Range(0.8f, 1.2f);
        c.transform.position = customerSpawns[Random.Range(0, customerSpawns.Length)].position;
        GameObject target = GenerateTargetPedestalWithItem();
        if(target==null)
        {
            target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
        }
        c.gameObject.SetActive(true);
        c.SetTarget(target);
        c.StartShopping();
    }
    public GameObject GenerateTargetPedestalWithItem()
    {
        if(windowPedestalsWithItems.Count>=0)
        {
            if(Random.Range(0,1.0f)<chanceToCheckWindowsFirst)
            return windowPedestalsWithItems[Random.Range(0, windowPedestalsWithItems.Count)].gameObject;
        }
        if (regularPedestalsWithItems.Count >= 0)
        {
            return regularPedestalsWithItems[Random.Range(0, regularPedestalsWithItems.Count)].gameObject;
        }
        return null;
    }
    public void CheckPedestalsforItems()
    {
        foreach (Pedestal p in ShopManager.instance.windowPedestals)
        {
            if(p.myItem!=null&&p.amount>0)
            {
                windowPedestalsWithItems.Add(p);
            }
        }
        foreach (Pedestal p in ShopManager.instance.regularPedestals)
        {
            if (p.myItem != null && p.amount > 0)
            {
                regularPedestalsWithItems.Add(p);
            }
        }
    }
    public void NPCGetNewTarget(Customer c_)
    {
        GameObject target = GenerateTargetPedestalWithItem();
        if (target == null)
        {
            target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
        }
        c_.SetTarget(target);
    }
}
