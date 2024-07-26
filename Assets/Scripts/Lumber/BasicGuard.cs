using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum GuardStates
{
    Idle,Chase,Wander,Searching
};
public class BasicGuard : MonoBehaviour
{
    NavMeshAgent myAgent;
    GameObject chaseObject;
    [SerializeField] GuardStates myCurrentState;
    [SerializeField] float minDistanceForAcceptableHiding;
    [SerializeField] Transform searchLocation;
    // Start is called before the first frame update
    //referances
    [SerializeField] GameObject myVision;
    [SerializeField] GameObject myIdleObject;
    [SerializeField] GameObject myChaseObject;
    [SerializeField] GameObject mySearchObject;


    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        ChangeGuardState(GuardStates.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch (myCurrentState)
        {
            case GuardStates.Idle:
                break;
            case GuardStates.Chase:
                myAgent.SetDestination(chaseObject.transform.position);
                break;
            case GuardStates.Wander:
                break;
            case GuardStates.Searching:
                myAgent.SetDestination(searchLocation.position);
                if (Vector3.Distance(transform.position, searchLocation.transform.position) < 2)
                {
                    ChangeGuardState(GuardStates.Wander);
                    GuardManager.instance.LoudestNoiseInvestigated();
                    return;
                }
                break;
        }
    }

    private void ChangeGuardState(GuardStates newState_)
    {
        myIdleObject.SetActive(false);
        myChaseObject.SetActive(false);
        mySearchObject.SetActive(false);
        switch(newState_)
        {
            case GuardStates.Idle:
                myVision.SetActive(false);
                myIdleObject.SetActive(true);
                break;
            case GuardStates.Chase:
                myAgent.SetDestination(chaseObject.transform.position);
                myVision.SetActive(false);
                myChaseObject.SetActive(true);
                break;
            case GuardStates.Wander:
                myVision.SetActive(true);
                ChangeGuardState(GuardStates.Idle);
                break;
            case GuardStates.Searching:
                if(!searchLocation)
                {
                    ChangeGuardState(GuardStates.Wander);
                    return;
                }
                myVision.SetActive(true);
                mySearchObject.SetActive(true);
                break;
        }
        myCurrentState = newState_;
    }
    public void PlayerEnterVision(GameObject playerObject)
    {
        chaseObject = playerObject;
        ChangeGuardState(GuardStates.Chase);
    }
    public void SetSearchTarget(Transform loc_)
    {
        if (myCurrentState == GuardStates.Chase)
            return;
        searchLocation = loc_;
        ChangeGuardState(GuardStates.Searching);
    }
    public void AttemptHide(GameObject playerObject)
    {
        if(myCurrentState==GuardStates.Chase)
        {
            if (playerObject != chaseObject)
                return;
            if(Vector3.Distance(transform.position, playerObject.transform.position)>minDistanceForAcceptableHiding)
            {
                searchLocation = playerObject.transform;
                ChangeGuardState(GuardStates.Searching);
                GuardManager.instance.LoudestNoiseInvestigated();
            }
        }
    }
}
