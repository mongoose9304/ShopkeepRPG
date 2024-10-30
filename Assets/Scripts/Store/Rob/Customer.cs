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
        haggleIndicator.SetActive(false);

    }
}
