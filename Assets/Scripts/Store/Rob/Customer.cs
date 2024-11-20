using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class TempItem
{
    public ItemData myItem;
    public int amount;
}
public class Customer : MonoBehaviour
{
    public bool isInHell;
    [SerializeField]protected NavMeshAgent myAgent;
    [SerializeField] protected GameObject tempTarget;
    public float chanceToLookAtBArginBin;
    public float chanceToStealItem;
    public int cashOnHand;
    public int cashOwed;
    public int startingCash;
    public float waitTimePerPedestalMax;
    public float waitTimePerEmptyPedestalMax;
    public float currentWaitTime;
    public float haggleValueMax;
    public float mood;
    public Pedestal hagglePedestal;
    public BarginBin currentBarginBin;
    public GameObject haggleInteraction;
    public float stopDistance;
    public int maxBrowseChances;
    public int currentBrowseChances;
    protected bool isMoving;
    protected bool hasBeenSmallTalked;
    [SerializeField]protected GameObject haggleIndicator;
    [SerializeField] List<GameObject> pedestalsSeen = new List<GameObject>();
    [SerializeField] List<TempItem> heldItems = new List<TempItem>();
    [SerializeField] GameObject waitingObject;
    public bool isInUse;
    protected bool isLeavingShop;
    //Haggle Dialogues 
    public List<string> greetings = new List<string>();
    public List<string> wayTooHigh = new List<string>();
    public List<string> bitTooHigh = new List<string>();
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
    public void BeginHaggle()
    {
        //haggleIndicator.SetActive(false);
        ShopManager.instance.OpenHaggleScreen(hagglePedestal,this,1);
        isInUse = true;
    }
    //return 0 if the cost is ok, 1 if it exceeeds my cost and 2 if I want it cheaper
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
    private void ChangeMood(float mood_)
    {
        mood += mood_;
        if (mood < 0.1f)
            mood = 0.1f;
        if (mood > 1.0f)
            mood = 1.0f;
    }
    public  virtual void EndHaggle(int cost_)
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
    public void ForceEndHaggle()
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
    public void SmallTalk()
    {
        if (hasBeenSmallTalked)
            return;
        hasBeenSmallTalked = true;
        ChangeMood(0.1f);
    }
    private void PurchaseBarginItem(int cost_)
    {
        cashOnHand -= cost_;
        cashOwed += cost_;
        CustomerManager.instance.PlayEmote(0, transform);
    }
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
    public void StartShopping()
    {
        currentBrowseChances = maxBrowseChances;
        isInUse = false;
        isLeavingShop = false;
        pedestalsSeen.Clear();
        if (waitingObject)
            waitingObject.SetActive(false);
    }
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
    public void SellHeldItems()
    {
        ShopManager.instance.AddCash(cashOwed,isInHell);
        cashOwed = 0;
    }
    public float GetDistanceToTarget()
    {
        if(tempTarget)
        {
            return Vector3.Distance(transform.position, tempTarget.transform.position);
        }
        return 0;
    }
}
