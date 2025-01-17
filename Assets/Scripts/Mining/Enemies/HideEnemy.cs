using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEnemy : BasicMiningEnemy
{
    
    [SerializeField]float hideDistance;
    Vector3 startPos;
    Vector3 endPos;
    Collider collider;
    [SerializeField] GameObject hatObject;
    float timeToWaitBeforePopingUpAfterLosingHat;
    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        endPos = startPos - new Vector3(0, 1.25f, 0);
        collider = GetComponent<Collider>();
    }
    private void Update()
    {
        if(hatObject.activeInHierarchy==false)
        {
            timeToWaitBeforePopingUpAfterLosingHat -= Time.deltaTime;
            if (timeToWaitBeforePopingUpAfterLosingHat <= 0)
                UnHide();
            return;
        }
       
          /*  if (Vector3.Distance(transform.position, player.transform.position) < hideDistance)
                Hide();
            if (Vector3.Distance(transform.position, player.transform.position) > hideDistance)
                UnHide();
          */
       
    }
    public void Hide()
   {
       transform.position= Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * moveSpeed);
        collider.enabled = false;
        timeToWaitBeforePopingUpAfterLosingHat = 0.5f;
    }
    public void UnHide()
    {
      transform.position=  Vector3.MoveTowards(transform.position, startPos, Time.deltaTime * moveSpeed);
        collider.enabled = true;

    }
    private void Attack()
    {

    }
}
