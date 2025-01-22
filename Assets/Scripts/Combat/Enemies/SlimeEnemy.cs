using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.VFX;
public class SlimeEnemy : BasicEnemy
{
    [SerializeField] Vector3 jumpSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float lowestJumpPercentage;
    public float slamRange;
    public float dashDistance;
    float jumpStart;
     float jumpEnd;
    float currentJumpPercentage;
    float currentMoveSpeedPercent;
    bool isJumping;
    //used to set the dash attack of the slime
    bool isDashing;
    private float dashTime;
    bool isSlaming;
    [SerializeField] GameObject slamParticleEffect;
    [SerializeField] MMF_Player JumpEffect;
    public SkinnedMeshRenderer rend;
    public VisualEffect slimeDashEffect;
    public EnemyDamageCollider slimeDashCollider;
    public override void Attack()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
        {
            StartDash();
            return;
        }
        canMove = false;
        isJumping = true ;
        isSlaming = false;
        agent.enabled = false;
        jumpStart = transform.position.y;
        jumpEnd = jumpStart + jumpHeight;
        currentJumpPercentage = 1.0f;
        JumpEffect.PlayFeedbacks();
    }
    private void StartDash()
    {
        if (isDashing)
            return;
        slimeDashEffect.Play();
        slimeDashCollider.gameObject.SetActive(true);
        if (target)
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), Vector3.up);
        agent.enabled = false;
        dashTime = 1.0f;
        isDashing = true;
    }
    private void EndDash()
    {
        isDashing = false;
        dashTime = 0.0f;
        agent.enabled = true;
        slimeDashCollider.gameObject.SetActive(false);
        slimeDashEffect.Stop();
    }


    private void LandJump()
    {
        canMove = true;
        isSlaming = false;
        isJumping = false;
        agent.enabled = true;
        EndAttack();
        Instantiate(slamParticleEffect, transform.position, Quaternion.Euler(new Vector3(-90,0,0)));
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Player")
            {
                hitCollider.gameObject.GetComponent<CombatPlayerMovement>().TakeDamage(damage*1.5f,0,myElement,0,this.gameObject,isMysticalDamage);
            }
            else if (hitCollider.tag == "Familiar")
            {
                hitCollider.gameObject.GetComponent<CombatFamiliar>().TakeDamage(damage * 1.5f, 0, myElement, 0, this.gameObject,isMysticalDamage);
            }
            else if (hitCollider.tag == "PlayerFamiliar")
            {
                hitCollider.gameObject.GetComponent<CombatCoopFamiliar>().TakeDamage(damage * 1.5f, 0, myElement, 0, this.gameObject, isMysticalDamage);
            }
            else if (hitCollider.tag == "Enemy")
            {
                if(CheckTeam(hitCollider.gameObject))
                    hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(damage * 1.5f, 0, myElement, 0, this.gameObject);
                
            }
            else if (hitCollider.tag == "Follower")
            {
                if (CheckTeam(hitCollider.gameObject))
                    hitCollider.gameObject.GetComponent<BasicFollower>().TakeDamage(damage * 1.5f, 0, myElement, 0, this.gameObject);

            }
        }
    }
    private void SlamDown()
    {
        isSlaming = true;

    }

    protected override void Update()
    {
        if (TempPause.instance.isPaused)
            return;
        if (!isJumping)
        {
            if (currentHitstun > 0)
            {
                CheckStun();
                return;
            }
            WaitingToAttack();
            if(isDashing)
            {
                //Dash Attack Logic
                dashTime -= Time.deltaTime;
                //check for wall or ground
                if (CheckForWallHit())
                {
                    dashTime = 0;
                    EndDash();
                    return;
                }
                if (dashTime <= 0)
                {
                    isDashing = false;
                    if(!GroundCheck())
                    {
                        Death();
                    }
                    EndDash();
                    return;
                }
                Vector3 temp = transform.position + (transform.forward * moveSpeed * Time.deltaTime * dashDistance);
                // transform.position = Vector3.SmoothDamp(transform.position, PreventGoingThroughWalls(temp), ref velocity, dampModifier);
                transform.position = temp;

            }
            else
            {
                Move();
            }
        }
        else
        {
            if (!isSlaming)
            {
                if (currentJumpPercentage > lowestJumpPercentage)
                    currentJumpPercentage -= Time.deltaTime*0.5f;
                transform.position += jumpSpeed * Time.deltaTime*currentJumpPercentage;
                if (transform.position.y >= jumpEnd)
                {
                    SlamDown();
                }
            }
            else
            {
                transform.position -= jumpSpeed * Time.deltaTime*2;
                if (transform.position.y <= jumpStart)
                {
                    LandJump();
                }

            }
        }
    }
    protected override void OnEnable()
    {
         /*public float damage;
    public Element myElement;
    public bool isMysticalDamage;
    public GameObject projectileExplosionObject;
    public string myTeam;
         */
        ResetEnemy();
        FindTarget();
        isJumping = false;
        isSlaming = false;
        slimeDashEffect.Stop();
        slimeDashCollider.gameObject.SetActive(false);
        slimeDashCollider.damage = damage;
        slimeDashCollider.myElement = myElement;
        slimeDashCollider.isMysticalDamage = isMysticalDamage;
       if(myTeamUser)
        {
            slimeDashCollider.myTeam = myTeamUser.myTeam;
        }
        if (rend)
        {
            rend.material.color = Color.white;
        }
    }
    public override void Move()
    {
        if (!agent.isActiveAndEnabled)
            return;
        if (canMove)
        {
            if (!target)
               target= CheckIfPlayerIsCloserThanFamiliar();
            if (!target.activeInHierarchy)
               target= CheckIfPlayerIsCloserThanFamiliar();
            agent.SetDestination(target.transform.position);
            agent.speed = moveSpeed * currentMoveSpeedPercent;
            currentMoveSpeedPercent -= Time.deltaTime;
            if (currentMoveSpeedPercent <= 0)
            {
                currentMoveSpeedPercent = 1;
            }
        }
        else
            agent.ResetPath();
    }
   
}
