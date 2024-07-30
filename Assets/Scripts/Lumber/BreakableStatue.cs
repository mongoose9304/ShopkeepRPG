using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStatue : Breakable
{
    [SerializeField] GameObject inTactStatue;
    [SerializeField] GameObject ruins;
    [SerializeField] Collider myCollider;
    public override void Break(GameObject breaker_=null)
    {
        inTactStatue.SetActive(false);
        ruins.SetActive(true);
        myCollider.enabled = false;
    }
}
