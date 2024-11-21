using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

/// <summary>
/// Manages all the customers that will be spanwd in the store
/// </summary>
public class CustomerManager : MonoBehaviour
{
    [Tooltip("The singleton instance of this object")]
    public static CustomerManager instance;
    [Header("behavior variables")]
    [Tooltip("The max amount of times customers will try to steal items")]
    public int maxSteals;
    public int currentSteals;
    [Tooltip("the amount of cash each NPC will be given, this number will change between 50-200%")]
    public int averageCustomerCash;
    [Tooltip("the amount of cash each NPC will be given, this number will change between 50-200%")]
    public float averageCustomerMood;
    [Tooltip("chance an NPC will go to the windows rather than the regular pedestals")]
    public float chanceToCheckWindowsFirst;
    [Tooltip("min time between customer spawns")]
    [SerializeField] float minTimeBetweenCustomerSpawns;
    [Tooltip("max time between customer spawns")]
    [SerializeField] float maxTimeBetweenCustomerSpawns;
    [Tooltip("min time between customer spawns in hell")]
    [SerializeField] float minTimeBetweenCustomerSpawnsHell;
    [Tooltip("max time between customer spawns in hell")]
    [SerializeField] float maxTimeBetweenCustomerSpawnsHell;
    [Tooltip("max customers that will enter the human store")]
    [SerializeField] int maxCustomers;
    [Tooltip("max customers that will enter the hell store")]
    [SerializeField] int maxCustomersHell;

    [Tooltip("all the current customers in the store")]
    [SerializeField] List<Customer> currentCustomersInStore;
    [Tooltip("current thieves in the human store")]
    [SerializeField] private int currentThieves;
    [Tooltip("current thieves in hell")]
    [SerializeField] private int currentThievesInHell;
    [SerializeField] float currentTimeBetweenCustomerSpawns;
    [SerializeField] float currentTimeBetweenCustomerSpawnsHell;
    [SerializeField]int customerCount;
    [SerializeField] int customerCountHell;
    [Tooltip("REFERNCE to the regular customers")]
    [SerializeField]protected MMMiniObjectPooler basicCustomerPool;
    [Tooltip("REFERNCE to the regular customers in hell")]
    [SerializeField]protected MMMiniObjectPooler basicCustomerPoolHell;
    [Tooltip("REFERNCE to the regular thieves")]
    [SerializeField]protected MMMiniObjectPooler basicThiefPool;
    [Tooltip("REFERNCE to the regular thieves in hell")]
    [SerializeField]protected MMMiniObjectPooler basicThiefPoolHell;
    [Tooltip("REFERNCE to the happyEmotes")]
    [SerializeField] protected MMMiniObjectPooler happyEmotePool;
    [Tooltip("REFERNCE to the anger emotes")]
    [SerializeField] protected MMMiniObjectPooler angerEmotePool;
    [Tooltip("REFERNCE to the effects that play when catching a thief")]
    [SerializeField] protected MMMiniObjectPooler ThiefCaughtPool;
    [Tooltip("REFERNCE to the spawns for customers")]
    public Transform[] customerSpawns;
    [Tooltip("REFERNCE to the spawns for customers in hell")]
    public Transform[] customerSpawnsHell;
    [Tooltip("all the pedestals with items, needs to be calculated any time there is a change")]
    public List<Pedestal> regularPedestalsWithItems = new List<Pedestal>();
    [Tooltip("all the pedestals with items in hell, needs to be calculated any time there is a change")]
    public List<Pedestal> regularPedestalsWithItemsHell = new List<Pedestal>();
    [Tooltip("all the pedestals with items at windows, needs to be calculated any time there is a change")]
    public List<Pedestal> windowPedestalsWithItems = new List<Pedestal>();
    [Tooltip("all the pedestals with items at windows in hell, needs to be calculated any time there is a change")]
    public List<Pedestal> windowPedestalsWithItemsHell = new List<Pedestal>();
    [Tooltip("all the bargin bins with items, needs to be calculated any time there is a change")]
    public List<BarginBin> barginBinsWithItems = new List<BarginBin>();
    [Tooltip("all the bargin bins with items in hell, needs to be calculated any time there is a change")]
    public List<BarginBin> barginBinsWithItemsHell = new List<BarginBin>();
    //prevents NPCs from spawning on top of each other
    private int lastNPCSpawnIndex=0;
    private int lastNPCSpawnIndexHell=0;
    [SerializeField] AudioClip stealAudio;
    [SerializeField] AudioClip thiefCaughtAudio;
    [SerializeField] AudioClip haggleAudio;
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
    /// <summary>
    /// Open the shop and spawn a burst of customers, this must be called again if you want to open both hell and human shops
    /// </summary>
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
    /// <summary>
    /// Spawn a random customer
    /// </summary>
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
    /// <summary>
    /// Manages a basic customer
    /// </summary>
    private void SpawnBasicCustomer(bool inHell=false)
    {
        if (!inHell)
        {
            Customer c = basicCustomerPool.GetPooledGameObject().GetComponent<Customer>();
            c.isInHell = false;
            c.GiveStartingCash(Mathf.RoundToInt(averageCustomerCash * Random.Range(0.5f, 2.0f)));
            //cap mood after setting it
            c.mood = averageCustomerMood * Random.Range(0.5f, 2.0f);
            c.ChangeMood(0);
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
            currentCustomersInStore.Add(c);
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
            currentCustomersInStore.Add(c);
        }
    }
    /// <summary>
    /// Get a pedestal with items for an NPC to go to if they exist
    /// </summary>
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
    /// <summary>
    /// Get a bin with items for an NPC to go towards if they exist
    /// </summary>
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
    /// <summary>
    /// Find all pedestals with items
    /// </summary>
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
    /// <summary>
    /// Find all bargin bins with items
    /// </summary>
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
    /// <summary>
    /// Find a new target for an NPC
    /// </summary>
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
    /// <summary>
    /// Spawn a theif at a customer's location and remove that customer. The thief will carry any items that NPC had.
    /// </summary>
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
        MMSoundManager.Instance.PlaySound(stealAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
     false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.98f, 1.02f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
     1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    /// <summary>
    /// Check if a thief can be spawned
    /// </summary>
    public bool CheckStealLimit()
    {
        if (currentSteals < maxSteals)
            return true;

        return false;
    }
    /// <summary>
    /// Thief has been caught, return items and cash
    /// </summary>
    public void CaughtThief(bool inHell=false ,bool gotAway=false)
    {
        if (!inHell)
        {
            currentThieves -= 1;
            if(currentThieves<=0)
            {
                currentThieves = 0;
                ShopManager.instance.SetStealAlert(false, false);
                CheckToCloseShop();
            }
        }
        else
        {
            currentThievesInHell -= 1;
            if (currentThievesInHell <= 0)
            {
                currentThievesInHell = 0;
                ShopManager.instance.SetStealAlert(false, true);
                CheckToCloseShop();
            }
        }
        if(!gotAway)
        MMSoundManager.Instance.PlaySound(thiefCaughtAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.98f, 1.02f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    /// <summary>
    /// Play an emote at a location. 0=happy, 1=anger,2= thief caught
    /// </summary>
    public void PlayEmote(int emote_,Transform location_)
    {
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
    /// <summary>
    /// Play an audio for emotes. 0=happy, 1=anger,2= thief caught
    /// </summary>
    public void PlayCustomerAudio(int emote_)
    {
        switch (emote_)
        {

            case 0:
                MMSoundManager.Instance.PlaySound(haggleAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
  false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
  1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    /// <summary>
    /// Remove a customer from the store and see if the minigame needs to end
    /// </summary>
    public void RemoveCustomer(Customer c_)
    {
        if(currentCustomersInStore.Contains(c_))
        {
            currentCustomersInStore.Remove(c_);
            CheckToCloseShop();
        }
        
    }
    /// <summary>
    /// If there are no more NPCs close the shop
    /// </summary>
    private void CheckToCloseShop()
    {
        if (currentCustomersInStore.Count == 0)
        {
            if (customerCountHell < maxCustomersHell && ShopManager.instance.hellShopEnabled)
            {
                return;
            }
            if (customerCount < maxCustomers && ShopManager.instance.humanShopEnabled)
            {
                return;
            }
            ShopManager.instance.CloseShop();
        }
    }
    /// <summary>
    /// Close the shop and end the time block
    /// </summary>
    public void CloseShop()
    {
        for(int i=0;i<currentCustomersInStore.Count;i++)
        {
            currentCustomersInStore[i].gameObject.SetActive(false);
            currentCustomersInStore.RemoveAt(i);
        }
    }
}
