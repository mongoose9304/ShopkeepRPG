using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEnemy : BasicEnemy
{
    public GameObject flamethrower;
    bool flameThrowerActive;
    public Animator anim;
    public void FlameBreathStart()
    {
        anim.SetBool("FlameThrower",true);
        flameThrowerActive = true;
    }
    public void FlameBreathEnd()
    {
        anim.SetBool("FlameThrower", false);
        flameThrowerActive = false;
    }
    protected override void OnEnable()
    {
        anim.SetBool("FlameThrower", false);
        flameThrowerActive = false;
        base.OnEnable();
    }
    protected override void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (!flameThrowerActive)
        {
            if (currentHitstun > 0)
            {
                CheckStun();
                return;
            }
            WaitingToAttack();
            Move();
        }
        else
        {

        }
    }
    
}
