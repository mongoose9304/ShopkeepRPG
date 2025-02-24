using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System;

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
    [Tooltip("REFERNCE to the rich customers")]
    [SerializeField] protected MMMiniObjectPooler richCustomerPool;
    [Tooltip("REFERNCE to the poor customers")]
    [SerializeField] protected MMMiniObjectPooler poorCustomerPool;
    [Tooltip("REFERNCE to linford")]
    [SerializeField] protected MMMiniObjectPooler linfordPool;
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
    public List<Pedestal> pedestalsWithItems = new List<Pedestal>();

    [Tooltip("all the pedestals with items in hell, needs to be calculated any time there is a change")]
    public List<Pedestal> pedestalsWithItemsHell = new List<Pedestal>();
    [Tooltip("all the pedestals with items at windows, needs to be calculated any time there is a change")]
    public List<BarginBin> barginBinsWithItems = new List<BarginBin>();
    [Tooltip("all the bargin bins with items in hell, needs to be calculated any time there is a change")]
    public List<BarginBin> barginBinsWithItemsHell = new List<BarginBin>();
    //prevents NPCs from spawning on top of each other
    private int lastNPCSpawnIndex=0;
    private int lastNPCSpawnIndexHell=0;
    [SerializeField] AudioClip stealAudio;
    [SerializeField] AudioClip thiefCaughtAudio;
    [SerializeField] AudioClip haggleAudio;

    bool testLinfordSpawnedToday = false;
   
    public float WarmFactorTotal = 0;
    public float OccultFactorTotal = 0;
    public float LivingFactorTotal = 0;
    public float ViolentFactorTotal = 0;
    public float GrossFactorTotal = 0;

    //default percentage -1/ 0 / 1
    public float[] warmPopularity = { 0.333f, 0.333f, 0.333f };
    public float[] occultPopularity = { 0.333f, 0.333f, 0.333f };
    public float[] livingPopularity = { 0.333f, 0.333f, 0.333f };
    public float[] violentPopularity = { 0.333f, 0.333f, 0.333f };
    public float[] grossPopularity = { 0.333f, 0.333f, 0.333f };

    public void CalculateReputation_ItemBased() {
        int count = pedestalsWithItems.Count;
        Dictionary<string, float[]> popularityMap = new Dictionary<string, float[]>
          {
             { "Warm", warmPopularity },
             { "Occult", occultPopularity },
             { "Living", livingPopularity },
             { "Violent", violentPopularity },
             { "Gross", grossPopularity }
           };
        string dominantFactorName = "";

        for (int i = 0; i < count; i++) {
            WarmFactorTotal += pedestalsWithItems[i].myItem.WarmFactor;
            OccultFactorTotal += pedestalsWithItems[i].myItem.OccultFactor;
            LivingFactorTotal += pedestalsWithItems[i].myItem.LivingFactor;
            ViolentFactorTotal += pedestalsWithItems[i].myItem.ViolentFactor;
            GrossFactorTotal += pedestalsWithItems[i].myItem.GrossFactor;
        }
        WarmFactorTotal /= count;
        OccultFactorTotal /= count;
        LivingFactorTotal /= count;
        ViolentFactorTotal /= count;
        GrossFactorTotal /= count;

        

        float dominantFactor = Mathf.Max(WarmFactorTotal, OccultFactorTotal, LivingFactorTotal, ViolentFactorTotal, GrossFactorTotal) ;
        int domCount = 0;
        if (WarmFactorTotal == dominantFactor) {
            domCount++;
            dominantFactorName = "Warm";
        }
        if (OccultFactorTotal == dominantFactor)
        {
            domCount++;
            dominantFactorName = "Occult";
        }
        if (LivingFactorTotal == dominantFactor) {
            domCount++;
            dominantFactorName = "Living";
        }
        if (ViolentFactorTotal == dominantFactor) {
            domCount++;
            dominantFactorName = "Violent";
        }
        if (GrossFactorTotal == dominantFactor) {
            domCount++;
            dominantFactorName = "Gross";
        }

        if (domCount > 1)
        {
            //increase a bit the percentage of popularity of the one with the same amount (subdom)
            Debug.Log("no clear dominant factor");
            if (popularityMap.ContainsKey(dominantFactorName))
            {
                popularityMap[dominantFactorName][0] = 0.20f;
                popularityMap[dominantFactorName][1] = 0.30f;
                popularityMap[dominantFactorName][2] = 0.50f;
            }

        }
        else {
            //increase a bit the percentage of popularity of the one that has the biggest factor (dom)

            //We will have to test this later, if those values are too crazy
            Debug.Log(dominantFactorName);
            Debug.Log(dominantFactor);

            if (popularityMap.ContainsKey(dominantFactorName))
            {
                popularityMap[dominantFactorName][0] = 0.10f;
                popularityMap[dominantFactorName][1] = 0.25f;
                popularityMap[dominantFactorName][2] = 0.65f;
            }
        }
       
    }

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
        CalculateReputation_ItemBased();
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
     
            int val = 0;
            int[] values = { 0, 1, 2, 3 };
            float[] weights = {0.75f, 0.0833f, 0.0833f, 0.0833f };

            float randomValue = Random.value;
            float cumulative = 0f;

            for (int i = 0; i < values.Length; i++)
            {
                cumulative += weights[i];
                if (randomValue <= cumulative) 
                {
                    val = values[i];
                    break;
                }          
            }
            switch (val) 
            {
                case 0:
                    SpawnCustomer(basicCustomerPool, pedestalsWithItems, barginBinsWithItems);
                    break;
                case 1:
                    SpawnCustomer(richCustomerPool, pedestalsWithItems, barginBinsWithItems);
                    break;
                case 2:
                    SpawnCustomer(poorCustomerPool, pedestalsWithItems, barginBinsWithItems);
                    break;
                case 3:
                    if (testLinfordSpawnedToday) SpawnCustomer(basicCustomerPool, pedestalsWithItems, barginBinsWithItems);
                    else
                    {
                        SpawnCustomer(linfordPool, pedestalsWithItems, barginBinsWithItems);
                        testLinfordSpawnedToday = true;
                    }
                    break;
            }
        }
        else
        {
            currentTimeBetweenCustomerSpawnsHell = Random.Range(minTimeBetweenCustomerSpawnsHell,maxTimeBetweenCustomerSpawnsHell);
            if (customerCountHell >= maxCustomersHell)
            {
                return;
            }
            customerCountHell += 1;
            SpawnCustomer(basicCustomerPool, pedestalsWithItemsHell, barginBinsWithItemsHell);
        }
    }

    private void SpawnCustomer(MMMiniObjectPooler pool, List<Pedestal> pedestalsWithItems, List<BarginBin> barginBinsWithItems)
    {
        Customer c = pool.GetPooledGameObject().GetComponent<Customer>();
        c.isInHell = false;
        c.GiveStartingCash(Mathf.RoundToInt(averageCustomerCash * Random.Range(0.5f, 2.0f)));
        //cap mood after setting it
        c.mood = averageCustomerMood * Random.Range(0.5f, 2.0f);
        c.ChangeMood(0);
        c.transform.position = customerSpawns[lastNPCSpawnIndex].position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(c.transform.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            c.myAgent.Warp(hit.position);
            //transform.position = hit.position;
        }
        lastNPCSpawnIndex += 1;
        if (lastNPCSpawnIndex >= customerSpawns.Length)
        {
            lastNPCSpawnIndex = 0;
        }
        GameObject target = ChoosePedestal(c, pedestalsWithItems, barginBinsWithItems);
        if (target == null)
        {
            target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
        }
        c.gameObject.SetActive(true);
        c.SetTarget(target);
        c.StartShopping();
        currentCustomersInStore.Add(c);
    }


    //lots of hardcoded values for now
    public GameObject ChoosePedestal(Customer customer, List<Pedestal> pedestals, List<BarginBin> bins) 
    {
        //placeholder variables
        float windowModifier = 2.0f;

        //float favorabilityModifier = 1.5f;

        float totalWeight = 0f;

        Dictionary<GameObject, float> objectWeights = new Dictionary<GameObject, float>();

        if (pedestals.Count > 0)
        {
            foreach (var pedestal in pedestals)
            {
                //first we need to check how strongly the customer aligns with the item on the pedestal
                float warmWeight = customer.GetWeight(customer.WarmFavorability, pedestal.myItem.WarmFactor);
                float occultWeight = customer.GetWeight(customer.OccultFavorability, pedestal.myItem.OccultFactor);
                float livingWeight = customer.GetWeight(customer.LivingFavorability, pedestal.myItem.LivingFactor);
                float violentWeight = customer.GetWeight(customer.ViolentFavorability, pedestal.myItem.ViolentFactor);
                float grossWeight = customer.GetWeight(customer.GrossFavorability, pedestal.myItem.GrossFactor);

                float totalPedestalWeight = (warmWeight + occultWeight + livingWeight + violentWeight + grossWeight);

                //now we include window modifier
                totalPedestalWeight *= pedestal.nearWindow ? windowModifier : 1.0f;

                //additionalModifiers
                //totalPedestalWeight *= pedestal.quality * additionalModifier;

                objectWeights[pedestal.gameObject] = totalPedestalWeight;
                totalWeight += totalPedestalWeight;
            }
        }

        if(bins.Count > 0) 
        {
            foreach(var bin in bins) 
            {
                float totalBinWeight = (bin.averageWarmFactor + bin.averageOccultFactor + bin.averageLivingFactor + bin.averageViolentFactor + bin.averageGrossFactor) + customer.chanceToLookAtBArginBin; //* bin favorability
                objectWeights[bin.gameObject] = totalBinWeight;
                totalWeight += totalBinWeight;
            }
        }

        if (totalWeight == 0) return null;

        float randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0;

        foreach (var weight in objectWeights)
        {
            cumulativeWeight += weight.Value;

            if (randomValue <= cumulativeWeight)
            {
                return weight.Key;
            }
        }

        return null;
    }

    /// <summary>
    /// Find all pedestals with items
    /// </summary>
    public void CheckPedestalsforItems()
    {
            pedestalsWithItems.Clear();
            foreach (Pedestal p in ShopManager.instance.regularPedestals)
            {
                if (p.myItem != null && p.amount > 0)
                {
                    pedestalsWithItems.Add(p);
                }
            }

            pedestalsWithItemsHell.Clear();
            foreach (Pedestal p in ShopManager.instance.regularPedestalsHell)
            {
                if (p.myItem != null && p.amount > 0)
                {
                    pedestalsWithItemsHell.Add(p);
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
            GameObject target = ChoosePedestal(c_, pedestalsWithItems, barginBinsWithItems);
            if (target == null)
            {
                target = ShopManager.instance.GetRandomTargetPedestal(chanceToCheckWindowsFirst);
            }
            c_.SetTarget(target);
        }
        else
        {
            
            GameObject target = ChoosePedestal(c_, pedestalsWithItemsHell, barginBinsWithItemsHell);
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

internal struct NewStruct
{
    public float Item1;
    public float Item2;
    public float Item3;

    public NewStruct(float item1, float item2, float item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }

    public override bool Equals(object obj)
    {
        return obj is NewStruct other &&
               Item1 == other.Item1 &&
               Item2 == other.Item2 &&
               Item3 == other.Item3;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Item1, Item2, Item3);
    }

    public void Deconstruct(out float item1, out float item2, out float item3)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
    }

    public static implicit operator (float, float, float)(NewStruct value)
    {
        return (value.Item1, value.Item2, value.Item3);
    }

    public static implicit operator NewStruct((float, float, float) value)
    {
        return new NewStruct(value.Item1, value.Item2, value.Item3);
    }
}