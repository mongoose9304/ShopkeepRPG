using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] NavMeshAgent myAgent;
    [SerializeField] GameObject tempTarget;
    public int cashOnHand;
    public float haggleValueMax;
    public float mood;
    public Pedestal hagglePedestal;
    public GameObject haggleInteraction;
    private bool isMoving;
    [SerializeField]GameObject haggleIndicator;
    private void Start()
    {
        SetTarget(tempTarget);
    }
    private void Update()
    {
        //SetTarget(tempTarget);
       if(isMoving)
        {
            if (!myAgent.pathPending)
            {
                if (myAgent.remainingDistance <= myAgent.stoppingDistance)
                {
                    if (!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
                    {
                        if(hagglePedestal)
                        {
                            ObservePedestal(hagglePedestal);
                            isMoving = false;
                        }
                    }
                }
            }
        }
    }
    public void ObservePedestal(Pedestal p_)
    {
        if(p_.myItem)
        {
            if(p_.amount>0)
            {
                if(p_.myItem.basePrice*p_.amount<=cashOnHand)
                {
                    RequestHaggle(p_);
                    haggleIndicator.SetActive(true);
                }
            }
        }
    }
    public virtual void RequestHaggle(Pedestal p_)
    {
        hagglePedestal = p_;
        haggleInteraction.SetActive(true);
        p_.SetInUse(true);
    }

    public void SetTarget(GameObject location)
    {
        myAgent.SetDestination(location.transform.position);
        if(location.TryGetComponent<Pedestal>(out Pedestal p))
        {
            hagglePedestal = p;
        }
        isMoving = true;
    }
    public void BeginHaggle()
    {
        //haggleIndicator.SetActive(false);
        ShopManager.instance.OpenHaggleScreen(hagglePedestal,this,1);
    }
    //return 0 if the cost is ok, 1 if it exceeeds my cost and 2 if I want it cheaper
    public int AttemptHaggle(int itemCost_,float haggleAmount)
    {
        if(itemCost_>cashOnHand)
        {
            return 1;
        }
        if (haggleAmount > haggleValueMax)
        {
            ChangeMood(-0.05f);
            //if they get a bad deal they should be unhappy
            return 2;
        }
        if(haggleAmount > haggleValueMax*mood)
        {
            ChangeMood(-0.01f);
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
    private void ChangeMood(float mood_)
    {
        mood += mood_;
        Mathf.Clamp(mood, 0.1f, 1.0f);
    }
}
