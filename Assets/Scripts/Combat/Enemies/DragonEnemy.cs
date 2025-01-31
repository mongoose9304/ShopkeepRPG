using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonEnemy : BasicEnemy
{
    public GameObject flamethrower;
    public GameObject flyFlamethrower;
    public GameObject flyFlameDamageCollider;
    bool flameThrowerActive;
    public Animator anim;
    public float flameTurnSpeed;
    public float flameThrowerDurationMax;
    float flameThrowerDurationCurrent;
    Vector3 lookAt;
    public void FlameBreathStart()
    {
        anim.SetBool("FlameAttack", true);
        flameThrowerDurationCurrent = flameThrowerDurationMax;
        flamethrower.GetComponent<EnemyDamageColliderOnStay>().damage = damage;
        flamethrower.GetComponent<EnemyDamageColliderOnStay>().myTeam = myTeamUser.myTeam;
        flameThrowerActive = true;
        flamethrower.gameObject.SetActive(true);
        agent.isStopped = true;
    }
    public void FlyFlameBreathStart()
    {
        anim.SetBool("FlyFlameAttack", true);
        Invoke("SpawnSlyingFlamethrower", 1.0f);
        flameThrowerDurationCurrent = flameThrowerDurationMax;
        agent.isStopped = true;
    }
    private void SpawnSlyingFlamethrower()
    {
        flyFlameDamageCollider.GetComponent<EnemyDamageColliderOnStay>().damage = damage;
        flyFlameDamageCollider.GetComponent<EnemyDamageColliderOnStay>().myTeam = myTeamUser.myTeam;
        flameThrowerActive = true;
        flyFlamethrower.gameObject.SetActive(true);
        flyFlameDamageCollider.gameObject.SetActive(true);
    }
    public void FlameBreathEnd()
    {
        anim.SetBool("FlameAttack", false);
        anim.SetBool("FlyFlameAttack", false);
        flameThrowerActive = false;
        flamethrower.gameObject.SetActive(false);
        flyFlamethrower.gameObject.SetActive(false);
        flyFlameDamageCollider.gameObject.SetActive(false);
        agent.isStopped = false;
    }
    protected override void OnEnable()
    {
        anim.SetBool("FlameAttack", false);
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
            anim.SetFloat("WalkSpeed", agent.velocity.magnitude / agent.speed);
        }
        else
        {
            flameThrowerDurationCurrent -= Time.deltaTime;
            if(flameThrowerDurationCurrent<=0)
            {
                FlameBreathEnd();
            }
            if (target)
            {
                if (target.activeInHierarchy)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime*flameTurnSpeed);
                }
            }
        }
    }
    /// <summary>
    /// The enemy's basic attack
    /// </summary>
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
        {
            FlameBreathStart();
            return;
        }
        FlyFlameBreathStart();
    }

}
