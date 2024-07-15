using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEnemy : BasicMiningEnemy
{
    
    [SerializeField]float hideDistance;
    Vector3 startPos;
    Vector3 endPos;
    Collider collider;
    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        endPos = startPos - new Vector3(0, 1.25f, 0);
        collider = GetComponent<Collider>();
    }
    private void Update()
    {
       
            if (Vector3.Distance(transform.position, player.transform.position) < hideDistance)
                Hide();
            if (Vector3.Distance(transform.position, player.transform.position) > hideDistance)
                UnHide();
       
    }
    private void Hide()
   {
       transform.position= Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * moveSpeed);
        collider.enabled = false;
           
    }
    private void UnHide()
    {
      transform.position=  Vector3.MoveTowards(transform.position, startPos, Time.deltaTime * moveSpeed);
        collider.enabled = true;

    }
}
