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
    [SerializeField]protected MMMiniObjectPooler basicCustomerPoolHell;
    public Transform[] customerSpawns;
    public Transform[] customerSpawnsHell;
    public List<Pedestal> regularPedestalsWithItems = new List<Pedestal>();
    public List<Pedestal> regularPedestalsWithItemsHell = new List<Pedestal>();
    public List<Pedestal> windowPedestalsWithItems = new List<Pedestal>();
    public List<Pedestal> windowPedestalsWithItemsHell = new List<Pedestal>();
    private int lastNPCSpawnIndex=0;
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
    public void OpenShop(int maxCustomers_,int burstCustomers_,bool inHell=false)
    {
        CheckPedestalsforItems();
        maxCustomers = maxCustomers_;
        customerCount = 0;
        for(int i=0;i<burstCustomers_;i++)
        {
            SpawnRandomCustomer();
        }
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
        c.GiveStartingCash(Mathf.RoundToInt(averageCustomerCash * Random.Range(0.5f, 2.0f)));
        c.mood = averageCustomerMood * Random.Range(0.5f, 2.0f);
        c.transform.position = customerSpawns[lastNPCSpawnIndex].position;
        lastNPCSpawnIndex += 1;
        if(lastNPCSpawnIndex>=customerSpawns.Length)
        {
            lastNPCSpawnIndex = 0;
        }
        GameObject target = GenerateTargetPedestalWithItem();
        if(target==null)
        {
            target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
        }
        c.gameObject.SetActive(true);
        c.SetTarget(target);
        c.StartShopping();
    }
    public GameObject GenerateTargetPedestalWithItem(bool inHell=false)
    {
        if (!inHell)
        {


            if (windowPedestalsWithItems.Count > 0)
            {
                if (Random.Range(0, 1.0f) < chanceToCheckWindowsFirst)
                    return windowPedestalsWithItems[Random.Range(0, windowPedestalsWithItems.Count)].gameObject;
            }
            if (regularPedestalsWithItems.Count > 0)
            {
                return regularPedestalsWithItems[Random.Range(0, regularPedestalsWithItems.Count)].gameObject;
            }
            return null;
        }
        else
        {
            if (windowPedestalsWithItemsHell.Count > 0)
            {
                if (Random.Range(0, 1.0f) < chanceToCheckWindowsFirst)
                    return windowPedestalsWithItemsHell[Random.Range(0, windowPedestalsWithItemsHell.Count)].gameObject;
            }
            if (regularPedestalsWithItemsHell.Count > 0)
            {
                return regularPedestalsWithItemsHell[Random.Range(0, regularPedestalsWithItemsHell.Count)].gameObject;
            }
            return null;
        }
    }
    public void CheckPedestalsforItems(bool inHell=false)
    {
        if (!inHell)
        {
            windowPedestalsWithItems.Clear();
            regularPedestalsWithItems.Clear();
            foreach (Pedestal p in ShopManager.instance.windowPedestals)
            {
                if (p.myItem != null && p.amount > 0)
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
        else
        {
            windowPedestalsWithItemsHell.Clear();
            regularPedestalsWithItemsHell.Clear();
            foreach (Pedestal p in ShopManager.instance.windowPedestalsHell)
            {
                if (p.myItem != null && p.amount > 0)
                {
                    windowPedestalsWithItemsHell.Add(p);
                }
            }
            foreach (Pedestal p in ShopManager.instance.regularPedestalsHell)
            {
                if (p.myItem != null && p.amount > 0)
                {
                    regularPedestalsWithItemsHell.Add(p);
                }
            }
        }
    }
    public void NPCGetNewTarget(Customer c_,bool inHell=false)
    {
        if (!inHell)
        {
            GameObject target = GenerateTargetPedestalWithItem();
            if (target == null)
            {
                target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
            }
            c_.SetTarget(target);
        }
        else
        {
            GameObject target = GenerateTargetPedestalWithItem(true);
            if (target == null)
            {
                target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst,true);
            }
            c_.SetTarget(target);
        }
    }
}
