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
    [SerializeField] NavMeshAgent myAgent;
    [SerializeField] GameObject tempTarget;
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
    private bool isMoving;
    [SerializeField]GameObject haggleIndicator;
    [SerializeField] List<GameObject> pedestalsSeen = new List<GameObject>();
    [SerializeField] List<TempItem> heldItems = new List<TempItem>();
    public bool isInUse;
    private void Update()
    {
        //SetTarget(tempTarget);
       if(isMoving)
        {
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
                        Debug.Log("No hagglePedestal");
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
    public void ObservePedestal(Pedestal p_)
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
                if(Random.Range(0,1.0f)<chanceToStealItem)
                {
                    CustomerManager.instance.CreateItemThief(transform,p_.myItem,p_.amount,heldItems);
                    p_.ItemSold();
                    gameObject.SetActive(false);
                    return;
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
                PurchaseBarginItem(bSlot.discountedCost);
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
    }

    public void SetTarget(GameObject location)
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
    }
    public void BeginHaggle()
    {
        //haggleIndicator.SetActive(false);
        ShopManager.instance.OpenHaggleScreen(hagglePedestal,this,1);
        isInUse = true;
    }
    //return 0 if the cost is ok, 1 if it exceeeds my cost and 2 if I want it cheaper
    public int AttemptHaggle(int itemCost_,float haggleAmount)
    {
        if(itemCost_==0)
        {
            ChangeMood(0.3f);
            //no one refuses free stuff
            return 0;
        }
        if(itemCost_>cashOnHand)
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
        if(haggleAmount > haggleValueMax*mood)
        {
            ChangeMood(-0.05f);
            //if they get a slightly bad deal they should be slightly unhappy
            return 2;
        }

        if (haggleAmount < haggleValueMax/1.25f)
        {
            ChangeMood(0.1f);
            //if they get a good deal they should be happy
        }
        if(haggleAmount<=0.1f)
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
        Mathf.Clamp(mood, 0.1f, 1.0f);
    }
    public void EndHaggle(int cost_)
    {
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
    private void PurchaseBarginItem(int cost_)
    {
        cashOnHand -= cost_;
        cashOwed += cost_;
    }
    public void EndWait()
    {
        ShopManager.instance.RemoveInteractableObject(haggleInteraction.gameObject);
        haggleInteraction.SetActive(false);
        haggleIndicator.SetActive(false);
        if(hagglePedestal)
        {
            hagglePedestal.SetInUse(false);
        }
        GetNewTarget();
    }
    public void StartShopping()
    {
        currentBrowseChances = maxBrowseChances;
        isInUse = false;
        pedestalsSeen.Clear();
    }
    public void LeaveShop()
    {
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
            gameObject.SetActive(false);
        }
    }
    private void GetNewTarget()
    {
        hagglePedestal = null;
        currentBarginBin = null;
        currentBrowseChances -= 1;
        if(currentBrowseChances<=0)
        {
            LeaveShop();
            return;
        }
        CustomerManager.instance.NPCGetNewTarget(this,isInHell);
    }
    private void TargetHasAlreadyBeenSeen()
    {
        Debug.Log("TargetSeenAlready");
        GameObject target_ = CustomerManager.instance.GenerateTargetPedestalWithItem(isInHell);
        int x = 0;
        while(pedestalsSeen.Contains(target_))
        {
            x += 1;
            target_ = CustomerManager.instance.GenerateTargetPedestalWithItem(isInHell);
            if (x>=6)
            {
                target_ = ShopManager.instance.GetRandomTargetPedestal(0.2f, isInHell);
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
