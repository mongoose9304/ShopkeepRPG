using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutCustomer : Customer
{
    private void OnEnable()
    {
        SetTarget(tempTarget);
    }
    protected override void Update()
    {
        //SetTarget(tempTarget);
        if (isMoving)
        {
            if (!myAgent.pathPending)
            {
                if (myAgent.remainingDistance <= stopDistance)
                {

                    if (hagglePedestal)
                    {
                        ObservePedestal(hagglePedestal);

                    }
                    else if (currentBarginBin)
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
            if (currentWaitTime > 0)
            {
                currentWaitTime -= Time.deltaTime;
                if (currentWaitTime <= 0)
                {
                    EndWait();
                }
            }
        }
    }
    public override void ObservePedestal(Pedestal p_)
    {
        if (p_.myItem)
        {
            if (p_.amount > 0)
            {
                if (Random.Range(0.0f, 1.0f) < chanceToStealItem)
                {
                  //Steal
                }
                RequestHaggle(p_);
                haggleIndicator.SetActive(true);


            }
        }
        else
        {
            currentWaitTime = waitTimePerEmptyPedestalMax;
            isMoving = false;
        }
    }
    public override void SetTarget(GameObject location)
    {
        myAgent.SetDestination(location.transform.position);
        tempTarget = location;
        if (location.TryGetComponent<Pedestal>(out Pedestal p))
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
}
