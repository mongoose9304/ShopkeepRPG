using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;
    public int maxSteals;
    public int currentSteals;
   [SerializeField] private int currentThieves;
   [SerializeField] private int currentThievesInHell;
    public int averageCustomerCash;
    public float averageCustomerMood;
    public float chanceToCheckWindowsFirst;
    [SerializeField] float minTimeBetweenCustomerSpawns;
    [SerializeField] float maxTimeBetweenCustomerSpawns;
    [SerializeField] float currentTimeBetweenCustomerSpawns;
    [SerializeField] float minTimeBetweenCustomerSpawnsHell;
    [SerializeField] float maxTimeBetweenCustomerSpawnsHell;
    [SerializeField] float currentTimeBetweenCustomerSpawnsHell;
    [SerializeField]int customerCount;
    [SerializeField]int maxCustomers;
    [SerializeField] int customerCountHell;
    [SerializeField] int maxCustomersHell;
    [SerializeField]protected MMMiniObjectPooler basicCustomerPool;
    [SerializeField]protected MMMiniObjectPooler basicCustomerPoolHell;
    [SerializeField]protected MMMiniObjectPooler basicThiefPool;
    [SerializeField]protected MMMiniObjectPooler basicThiefPoolHell;
    [SerializeField] protected MMMiniObjectPooler happyEmotePool;
    [SerializeField] protected MMMiniObjectPooler angerEmotePool;
    [SerializeField] protected MMMiniObjectPooler ThiefCaughtPool;
    public Transform[] customerSpawns;
    public Transform[] customerSpawnsHell;
    public List<Pedestal> regularPedestalsWithItems = new List<Pedestal>();
    public List<Pedestal> regularPedestalsWithItemsHell = new List<Pedestal>();
    public List<Pedestal> windowPedestalsWithItems = new List<Pedestal>();
    public List<Pedestal> windowPedestalsWithItemsHell = new List<Pedestal>();
    public List<BarginBin> barginBinsWithItems = new List<BarginBin>();
    public List<BarginBin> barginBinsWithItemsHell = new List<BarginBin>();
    private int lastNPCSpawnIndex=0;
    private int lastNPCSpawnIndexHell=0;
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        currentTimeBetweenCustomerSpawns -= Time.deltaTime;
        currentTimeBetweenCustomerSpawnsHell -= Time.deltaTime;
        if(currentTimeBetweenCustomerSpawns<=0)
        {
            SpawnRandomCustomer();
        }
        if (currentTimeBetweenCustomerSpawnsHell <= 0)
        {
            SpawnRandomCustomer(true);
        }
    }
    public void OpenShop(int maxCustomers_,int burstCustomers_,bool inHell=false)
    {
        CheckPedestalsforItems();
        CheckBarginBinsForItems();
        currentSteals = 0;
        if (!inHell)
        {

            maxCustomers = maxCustomers_;
            customerCount = 0;
            for (int i = 0; i < burstCustomers_; i++)
            {
                SpawnRandomCustomer();
            }
        }
        else
        {
            maxCustomersHell = maxCustomers_;
            customerCountHell = 0;
            for (int i = 0; i < burstCustomers_; i++)
            {
                SpawnRandomCustomer(true);
            }
        }
    }
    public void SpawnRandomCustomer(bool inHell = false)
    {
        if(!ShopManager.instance.CheckIfShopIsOpen(inHell))
        {
            return;
        }
        if (!inHell)
        {
            currentTimeBetweenCustomerSpawns = Random.Range(minTimeBetweenCustomerSpawns, maxTimeBetweenCustomerSpawns);
            if (customerCount >= maxCustomers)
            {
                return;
            }
            customerCount += 1;
            SpawnBasicCustomer();
        }
        else
        {
            currentTimeBetweenCustomerSpawnsHell = Random.Range(minTimeBetweenCustomerSpawnsHell,maxTimeBetweenCustomerSpawnsHell);
            if (customerCountHell >= maxCustomersHell)
            {
                return;
            }
            customerCountHell += 1;
            SpawnBasicCustomer(true);
        }
    }
    private void SpawnBasicCustomer(bool inHell=false)
    {
        if (!inHell)
        {
            Customer c = basicCustomerPool.GetPooledGameObject().GetComponent<Customer>();
            c.isInHell = false;
            c.GiveStartingCash(Mathf.RoundToInt(averageCustomerCash * Random.Range(0.5f, 2.0f)));
            c.mood = averageCustomerMood * Random.Range(0.5f, 2.0f);
            c.transform.position = customerSpawns[lastNPCSpawnIndex].position;
            lastNPCSpawnIndex += 1;
            if (lastNPCSpawnIndex >= customerSpawns.Length)
            {
                lastNPCSpawnIndex = 0;
            }
            GameObject target = GenerateTargetPedestalWithItem();
            if (target == null)
            {
                target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
            }
            c.gameObject.SetActive(true);
            c.SetTarget(target);
            c.StartShopping();
        }
        else
        {
            Customer c = basicCustomerPoolHell.GetPooledGameObject().GetComponent<Customer>();
            c.isInHell = true;
            c.GiveStartingCash(Mathf.RoundToInt(averageCustomerCash * Random.Range(0.5f, 2.0f)));
            c.mood = averageCustomerMood * Random.Range(0.5f, 2.0f);
            c.transform.position = customerSpawnsHell[lastNPCSpawnIndexHell].position;
            lastNPCSpawnIndexHell += 1;
            if (lastNPCSpawnIndexHell >= customerSpawnsHell.Length)
            {
                lastNPCSpawnIndexHell = 0;
            }
            GameObject target = GenerateTargetPedestalWithItem(true);
            if (target == null)
            {
                target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst,true);
            }
            c.gameObject.SetActive(true);
            c.SetTarget(target);
            c.StartShopping();
        }
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
    public GameObject GenerateTargetBarginBinWithItem(bool inHell = false)
    {
        if (!inHell)
        {
            if (barginBinsWithItems.Count > 0)
            {
                return barginBinsWithItems[Random.Range(0, barginBinsWithItems.Count)].gameObject;
            }
            return null;
        }
        else
        {
            if (barginBinsWithItemsHell.Count > 0)
            {
                return barginBinsWithItemsHell[Random.Range(0, barginBinsWithItemsHell.Count)].gameObject;
            }
            return null;
        }
    }
    public void CheckPedestalsforItems()
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
    public void CheckBarginBinsForItems()
    {

        barginBinsWithItems.Clear();
        
        foreach (BarginBin b in ShopManager.instance.barginBins)
        {
            if (b.binSlotsWithItems.Count > 0)
                barginBinsWithItems.Add(b);
        }
        barginBinsWithItemsHell.Clear();
        foreach (BarginBin b in ShopManager.instance.barginBinsHell)
        {
            if (b.binSlotsWithItems.Count > 0)
                barginBinsWithItemsHell.Add(b);
        }

    }
    public void NPCGetNewTarget(Customer c_,bool inHell=false)
    {
        if (!inHell)
        {
            if (Random.Range(0, 1.0f) < c_.chanceToLookAtBArginBin)
            {
                //look at bargin bin and return
                GameObject targetBin = GenerateTargetBarginBinWithItem();
                if (targetBin == null)
                {
                    targetBin = ShopManager.instance.GetRandomTargetBarginBin();
                }
                c_.SetTarget(targetBin);
                return;

            }
            GameObject target = GenerateTargetPedestalWithItem();
            if (target == null)
            {
                target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
            }
            c_.SetTarget(target);
        }
        else
        {
            if (Random.Range(0, 1.0f) < c_.chanceToLookAtBArginBin)
            {
                //look at bargin bin and return
                GameObject targetBin = GenerateTargetBarginBinWithItem(true);
                if (targetBin == null)
                {
                    targetBin = ShopManager.instance.GetRandomTargetBarginBin(true);
                }
                c_.SetTarget(targetBin);
                return;

            }
            GameObject target = GenerateTargetPedestalWithItem(true);
            if (target == null)
            {
                target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst,true);
            }
            c_.SetTarget(target);
        }
    }
    public void CreateItemThief(Transform location_,ItemData item_,int amount_,List<TempItem> heldItems=null,bool inHell=false)
    {
        currentSteals += 1;
        if(inHell)
        {
            currentThievesInHell += 1;
        }
        else
        {
            currentThieves += 1;
        }
        GameObject obj = null;
        if (!inHell)
        {
             obj = basicThiefPool.GetPooledGameObject();
        }
        else
        {
             obj = basicThiefPoolHell.GetPooledGameObject();
        }
        obj.transform.position = location_.position;
        obj.SetActive(true);
        obj.GetComponent<Thief>().StealItem(item_,amount_);
        if(heldItems!=null)
        obj.GetComponent<Thief>().SetHeldItems(heldItems);
        ShopManager.instance.currentThieves.Add(obj.GetComponent<Thief>());
    }
    public bool CheckStealLimit()
    {
        if (currentSteals < maxSteals)
            return true;

        return false;
    }
    public void CaughtThief(bool inHell=false)
    {
        if (!inHell)
        {
            currentThieves -= 1;
            if(currentThieves<=0)
            {
                currentThieves = 0;
                ShopManager.instance.SetStealAlert(false, false);
            }
        }
        else
        {
            currentThievesInHell -= 1;
            if (currentThievesInHell <= 0)
            {
                currentThievesInHell = 0;
                ShopManager.instance.SetStealAlert(false, true);
            }
        }
    }
    public void PlayEmote(int emote_,Transform location_)
    {//0=happy, 1=anger,2= thief caught
        GameObject obj = null;
        switch (emote_)
        {
           
            case 0:
                obj = happyEmotePool.GetPooledGameObject();
                obj.transform.position = location_.position;
                obj.transform.position += new Vector3(0, 1, 0);
                obj.SetActive(true);
                break;
            case 1:
                obj = angerEmotePool.GetPooledGameObject();
                obj.transform.position = location_.position;
                obj.transform.position += new Vector3(0, 1, 0);
                obj.SetActive(true);
                break;
            case 2:
                obj = ThiefCaughtPool.GetPooledGameObject();
                obj.transform.position = location_.position;
                obj.SetActive(true);
                break;
        }
    }
}
