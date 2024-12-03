using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
/// <summary>
/// Holds item data and an amount of that item
/// </summary>
public class TempItem
{
    public ItemData myItem;
    public int amount;
}
/// <summary>
/// The class for the customers that can walk into the store. 
/// </summary>
public class Customer : MonoBehaviour
{
    [Header("behavior variables")]
    [Tooltip("%0-1 chnace to go to a bargin bin instead of a pedestal")]
    public float chanceToLookAtBArginBin;
    [Tooltip("the chance I will steal an item")]
    public float chanceToStealItem;
    [Tooltip("the cash i start with")]
    public int startingCash;
    [Tooltip("how long will i spend waiting for the player to come to me")]
    public float waitTimePerPedestalMax;
    [Tooltip("how long will i spend waiting for the player to come to me at a pedestal with no items")]
    public float waitTimePerEmptyPedestalMax;
    [Tooltip("whats the max haggle amount I am willing to go to. At 100% mood I will go up to this amount")]
    public float haggleValueMax;
    [Tooltip("my current mood, this affects how close to my max haggle value I am willing to get")]
    public float mood;
    [Tooltip("how far can I be while interacting with a pedestal")]
    public float stopDistance;
    [Tooltip("the max things I can look at before leaving the shop")]
    public int maxBrowseChances;
    public int currentBrowseChances;

    [Header("Fixed variables")]
    [Tooltip("Is this customer currently in hell")]
    public bool isInHell;
    [Tooltip("REFERNCE to my navnesh agent")]
    [SerializeField]protected NavMeshAgent myAgent;
    [Tooltip("the target I am currently moving towards")]
    [SerializeField] protected GameObject tempTarget;
    [Tooltip("how much cash I owe for items picked up from bins")]
    public int cashOwed;
    [Tooltip("how much cash I have left to spend today")]
    public int cashOnHand;
    public float currentWaitTime;
    [Tooltip("the current pedestal I'm looking at")]
    public Pedestal hagglePedestal;
    [Tooltip("the current bargin bin I'm looking at")]
    public BarginBin currentBarginBin;
    [Tooltip("REFERENCE to the interactable target for the player to haggle with me")]
    public GameObject haggleInteraction;
    [Tooltip("Am i not at my destination yet?")]
    protected bool isMoving;
    [Tooltip("Has my mood been bosted by small talk yet?")]
    public bool hasBeenSmallTalked;
    [Tooltip("REFERENCE to the ! above the NPC when they can be interacted with")]
    [SerializeField]protected GameObject haggleIndicator;
    [Tooltip("all the pedestals I have already seen so I don't keep looking at the same items")]
    [SerializeField] List<GameObject> pedestalsSeen = new List<GameObject>();
    [Tooltip("items I am holding in case of returns/steals")]
    [SerializeField] List<TempItem> heldItems = new List<TempItem>();
    [Tooltip("REFERENCE to the ... above the NPC when they are waiting")]
    [SerializeField]protected GameObject waitingObject;
    [Tooltip("am I currently being used")]
    public bool isInUse;
    [Tooltip("am I leaving the shop")]
    protected bool isLeavingShop;
    //Haggle Dialogues 
    [Tooltip("REFERENCE to the things I can say when haggling ")]
    public List<string> greetings = new List<string>();
    [Tooltip("REFERENCE to the things I can say when haggling when I get a really bad deal ")]
    public List<string> wayTooHigh = new List<string>();
    [Tooltip("REFERENCE to the things I can say when haggling when I get a slightly bad deal ")]
    public List<string> bitTooHigh = new List<string>();
    [Tooltip("REFERENCE to the things I can say when haggling when small talk button is pressed ")]
    public List<string> smallTalks = new List<string>();
    protected virtual void Update()
    {
        //SetTarget(tempTarget);
       if(isMoving)
        {
            if (!isLeavingShop)
            {
                if (currentWaitTime > 0)
                {
                    currentWaitTime -= Time.deltaTime;
                    if (currentWaitTime <= 0)
                    {
                        EndWait();
                        return;
                    }
                }
            }
            if (!myAgent.pathPending)
            {
                if (myAgent.remainingDistance <= stopDistance)
                {
                    
                        if(hagglePedestal)
                        {
                            ObservePedestal(hagglePedestal);
                            
                        }
                        else if(currentBarginBin)
                        {
                        ObserveBarginBin(currentBarginBin);
                        }
                        else
                        {
                        }
                    
                }
            }
        }
        else
        {
            if (isInUse)
                return;
            if(currentWaitTime>0)
            {
                currentWaitTime -= Time.deltaTime;
                if(currentWaitTime<=0)
                {
                    EndWait();
                }
            }
        }
    }
    /// <summary>
    /// When i see a pedestal I should check If i should buy from it
    /// </summary>
    public virtual void ObservePedestal(Pedestal p_)
    {
        if (p_.inUse)
        {
            if(!pedestalsSeen.Contains(p_.gameObject))
            pedestalsSeen.Add(p_.gameObject);
            GetNewTarget();
            return;
        }
        if (!pedestalsSeen.Contains(p_.gameObject))
            pedestalsSeen.Add(p_.gameObject);
        if (p_.myItem)
        {
            if(p_.amount>0)
            {
                if(Random.Range(0.0f,1.0f)<chanceToStealItem)
                {
                    if (CustomerManager.instance.CheckStealLimit())
                    {
                        CustomerManager.instance.CreateItemThief(transform, p_.myItem, p_.amount, heldItems, isInHell);
                        p_.ItemSold();
                        CustomerManager.instance.RemoveCustomer(this);
                        gameObject.SetActive(false);
                        return;
                    }
                }
                if(p_.myItem.basePrice*p_.amount<=cashOnHand)
                {
                    RequestHaggle(p_);
                    haggleIndicator.SetActive(true);
                }
                else
                {
                    GetNewTarget();
                }
            }
        }
        else
        {
            currentWaitTime = waitTimePerEmptyPedestalMax;
            isMoving = false;
        }
    }
    /// <summary>
    /// When I see a bargin bin I should check if I should buy from it
    /// </summary>
    public void ObserveBarginBin(BarginBin b_)
    {
        if (b_.inUse)
        {
            Debug.Log("B in use");
            if (!pedestalsSeen.Contains(b_.gameObject))
                pedestalsSeen.Add(b_.gameObject);
            GetNewTarget();
            return;
        }
        if (!pedestalsSeen.Contains(b_.gameObject))
            pedestalsSeen.Add(b_.gameObject);
        if (b_.binSlotsWithItems.Count>0)
        {
            BarginBinSlot bSlot = b_.binSlotsWithItems[Random.Range(0, b_.binSlotsWithItems.Count)];
            if (bSlot.discountedCost<=cashOnHand)
            {
                //purchase item
                switch(ShopManager.instance.CheckIfItemIsHot(bSlot.myItem,isInHell))
                {
                    case 0:
                        PurchaseBarginItem(bSlot.discountedCost);
                        break;
                    case 1:
                        PurchaseBarginItem(Mathf.RoundToInt(bSlot.discountedCost * ShopManager.instance.GetHotItemMultiplier()));
                        break;
                        break;
                    case 2:
                        PurchaseBarginItem(Mathf.RoundToInt(bSlot.discountedCost * ShopManager.instance.GetColdItemMultiplier()));
                        break;
                }
              
                TempItem item_=new TempItem();
                item_.amount = bSlot.amount;
                item_.myItem = bSlot.myItem;
                heldItems.Add(item_);
                currentBarginBin.SellItem(bSlot);
                GetNewTarget();
            }
            else
            {
                GetNewTarget();
            }
        }
        else
        {
            currentWaitTime = waitTimePerEmptyPedestalMax;
            isMoving = false;
        }
    }
    /// <summary>
    /// Set a pedestal as in use and allow the player to interact with me to start a haggle 
    /// </summary>
    public virtual void RequestHaggle(Pedestal p_)
    {
        hagglePedestal = p_;
        haggleInteraction.SetActive(true);
        isMoving = false;
        p_.SetInUse(true);
        currentWaitTime = waitTimePerPedestalMax;
        if (waitingObject)
            waitingObject.SetActive(true);
    }
    /// <summary>
    /// Set a target to walk towards and make sure I havent already seen it
    /// </summary>
    public virtual void SetTarget(GameObject location)
    {
        if(pedestalsSeen.Contains(location))
        {
            TargetHasAlreadyBeenSeen();
            return;
        }
        myAgent.SetDestination(location.transform.position);
        tempTarget = location;
        if(location.TryGetComponent<Pedestal>(out Pedestal p))
        {
            hagglePedestal = p;
        }
        if (location.TryGetComponent<BarginBin>(out BarginBin b))
        {
            currentBarginBin = b;
        }
        isMoving = true;
        currentWaitTime = waitTimePerPedestalMax * 2;
    }
    /// <summary>
    /// Start a haggle with the player when interacted with
    /// </summary>
    public void BeginHaggle(bool isPlayer2=false)
    {
        //haggleIndicator.SetActive(false);
        ShopManager.instance.OpenHaggleScreen(hagglePedestal,this,1,isPlayer2);
        isInUse = true;
    }
    /// <summary>
    ///The player is attempting to make a deal with this NPC. return 0 if the cost is ok, 1 if it exceeeds my cost and 2 if I want it cheaper
    /// </summary>
    public int AttemptHaggle(int itemCost_,float haggleAmount,bool itemHot=false,bool itemCold=false)
    {
        //the customer should spend more to buy hot items and less for cold items
        if (itemHot)
        {
            if (itemCost_ > cashOnHand * ShopManager.instance.GetHotItemMultiplier())
            {
                ChangeMood(-0.1f);
                return 1;
            }
            if (haggleAmount < haggleValueMax*ShopManager.instance.GetHotItemMultiplier()*mood)
            {
                ChangeMood(0.1f);
                return 0;
                //if they get a good deal they should be happy
            }
        }
        else if (itemCold)
        {
            if (haggleAmount > haggleValueMax* ShopManager.instance.GetColdItemMultiplier())
            {
                ChangeMood(-0.1f);
                return 1;
            }
        }

            if (itemCost_ == 0)
            {
                ChangeMood(0.3f);
                //no one refuses free stuff
                return 0;
            }
            if (itemCost_ > cashOnHand)
            {
                ChangeMood(-0.1f);
                return 1;
            }
            if (haggleAmount > haggleValueMax)
            {
                ChangeMood(-0.1f);
                //if they get a bad deal they should be unhappy
                return 2;
            }
            if (haggleAmount > haggleValueMax * mood)
            {
                ChangeMood(-0.05f);
                //if they get a slightly bad deal they should be slightly unhappy
                return 2;
            }

            if (haggleAmount < haggleValueMax / 1.25f)
            {
                ChangeMood(0.1f);
                //if they get a good deal they should be happy
            }
            if (haggleAmount <= 0.1f)
            {
                ChangeMood(0.3f);
                //if they get a really good deal they should be really happy
            }



            return 0;
        
    }
    /// <summary>
    /// The player is moving the haggle slider and this will return a mood that is close but not 100% percise of if they will accept or not
    /// </summary>
    public int CheckHaggle(int itemCost_, float haggleAmount)
    {
        if (itemCost_ == 0)
        {
            //no one refuses free stuff
            return 0;
        }
        if (itemCost_ > cashOnHand)
        {
            return 1;
        }
        if (haggleAmount > haggleValueMax)
        {
            //if they get a bad deal they should be unhappy
            return 2;
        }
        if (haggleAmount > haggleValueMax * mood*1.15f)
        {
            //+15% should be frowny face
            return 2;
        }
        if (haggleAmount < haggleValueMax * mood*0.85f)
        {
            //-15% should be happy face
            return 3;
        }




        return 4;
    }
    /// <summary>
    /// Change my mood, caps at 10% and 100%
    /// </summary>
    public void ChangeMood(float mood_)
    {
        mood += mood_;
        if (mood < 0.1f)
            mood = 0.1f;
        if (mood > 1.0f)
            mood = 1.0f;
    }
    /// <summary>
    /// End my haggle with a sale. If I'm out of cash leave, or else keep browsing
    /// </summary>
    public virtual void EndHaggle(int cost_)
    {
        if (waitingObject)
            waitingObject.SetActive(false);
        CustomerManager.instance.PlayEmote(0, transform);
        CustomerManager.instance.PlayCustomerAudio(0);
        cashOnHand -= cost_;
        ShopManager.instance.RemoveInteractableObject(haggleInteraction.gameObject);
        haggleInteraction.SetActive(false);
        haggleIndicator.SetActive(false);
        isInUse = false ;
        if(cashOnHand<=startingCash/2)
        {
            LeaveShop();
        }
        else
        {
            GetNewTarget();
        }
    }
    /// <summary>
    /// End the haggle without a sale
    /// </summary>
    public virtual void ForceEndHaggle()
    {
        if (waitingObject)
            waitingObject.SetActive(false);
        CustomerManager.instance.PlayEmote(1, transform);
        ShopManager.instance.RemoveInteractableObject(haggleInteraction.gameObject);
        haggleInteraction.SetActive(false);
        haggleIndicator.SetActive(false);
        isInUse = false;
        if (cashOnHand <= startingCash / 2)
        {
            LeaveShop();
        }
        else
        {
            GetNewTarget();
        }
    }
    /// <summary>
    /// Slightly incerase my mood
    /// </summary>
    public void SmallTalk()
    {
        if (hasBeenSmallTalked)
            return;
        hasBeenSmallTalked = true;
        ChangeMood(0.1f);
    }
    /// <summary>
    /// Store an item from a bin to buy it at the register
    /// </summary>
    private void PurchaseBarginItem(int cost_)
    {
        cashOnHand -= cost_;
        cashOwed += cost_;
        CustomerManager.instance.PlayEmote(0, transform);
    }
    /// <summary>
    /// If an NPC waits too long they will move on to another object
    /// </summary>
    public void EndWait()
    {
        if (waitingObject)
            waitingObject.SetActive(false);
        ShopManager.instance.RemoveInteractableObject(haggleInteraction.gameObject);
        haggleInteraction.SetActive(false);
        haggleIndicator.SetActive(false);
        if(hagglePedestal)
        {
            hagglePedestal.SetInUse(false);
            CustomerManager.instance.PlayEmote(1, transform);
        }
        GetNewTarget();
    }
    /// <summary>
    /// Called when spawned
    /// </summary>
    public void StartShopping()
    {
        currentBrowseChances = maxBrowseChances;
        isInUse = false;
        isLeavingShop = false;
        pedestalsSeen.Clear();
        if (waitingObject)
            waitingObject.SetActive(false);
    }
    /// <summary>
    /// Leave the shop or head to the register first 
    /// </summary>
    public void LeaveShop()
    {
        isLeavingShop = true;
        if(cashOwed>0)
        {
            ShopManager.instance.HeadToCashRegister(this,isInHell);
            return;
        }
        SetTarget(ShopManager.instance.GetRandomNPCExit(isInHell));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="EndZone")
        {
            CustomerManager.instance.RemoveCustomer(this);
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Get a new target to look at 
    /// </summary>
    private void GetNewTarget()
    {
        hagglePedestal = null;
        currentBarginBin = null;
        currentBrowseChances -= 1;
        //NPCs should have a limit on how much they can browse
        if(currentBrowseChances<=0)
        {
            LeaveShop();
            return;
        }
        //leave if the player closes the shop early
        if (!ShopManager.instance.CheckIfShopIsOpen(isInHell))
        {
            LeaveShop();
            return;
        }
        CustomerManager.instance.NPCGetNewTarget(this,isInHell);
    }
    /// <summary>
    /// Logic for if the NPC has already seen this pedestal
    /// </summary>
    private void TargetHasAlreadyBeenSeen()
    {
        GameObject target_ = CustomerManager.instance.GenerateTargetPedestalWithItem(isInHell);
        int x = 0;
        while(pedestalsSeen.Contains(target_))
        {
            x += 1;
            target_ = CustomerManager.instance.GenerateTargetPedestalWithItem(isInHell);
            if (x>=6)
            {
                target_ = ShopManager.instance.GetRandomTargetPedestal(0.2f, isInHell);
                break;
            }
        }
        if(target_==null)
            target_ = ShopManager.instance.GetRandomTargetPedestal(0.2f, isInHell);
        myAgent.SetDestination(target_.transform.position);
        tempTarget = target_;
        if (target_.TryGetComponent<Pedestal>(out Pedestal p))
        {
            hagglePedestal = p;
        }
        if (target_.TryGetComponent<BarginBin>(out BarginBin b))
        {
            currentBarginBin = b;
        }
        isMoving = true;
    }
    /// <summary>
    /// Give the npc a budget
    /// </summary>
    public void GiveStartingCash(int cash_)
    {
        startingCash = cash_;
        cashOnHand = cash_;
        cashOwed = 0;
        currentBarginBin = null;
        hagglePedestal = null;
        hasBeenSmallTalked = false;
        heldItems.Clear();
    }
    /// <summary>
    /// Sell all held items at the register
    /// </summary>
    public void SellHeldItems()
    {
        ShopManager.instance.AddCash(cashOwed,isInHell);
        cashOwed = 0;
    }
    /// <summary>
    /// How far am I from my target
    /// </summary>
    public float GetDistanceToTarget()
    {
        if(tempTarget)
        {
            return Vector3.Distance(transform.position, tempTarget.transform.position);
        }
        return 0;
    }
}
