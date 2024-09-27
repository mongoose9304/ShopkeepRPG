using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A jump/slam aoe attack
/// </summary>
public class ThunderStorm : PlayerSpecialAttack
{
    [Header("Referecnes")]
    public GameObject particleEffect;
    bool isJumping;
    bool isLanding;
    float jumpEnd;
    float jumpStart;
    float currentJumpPercentage;
    [Header("Stats")]
    [SerializeField] float lowestJumpPercentage;
    [SerializeField] Vector3 jumpSpeed;
    [SerializeField]float slamRange;
    public float jumpHeight;
    public override void OnPress(GameObject obj_)
    {
        Player = obj_;
        isBusy = true;
        isJumping = true;
        jumpStart = Player.transform.position.y;
        jumpEnd = jumpStart + jumpHeight;
        currentJumpPercentage = 1.0f;
    }
    private void Update()
    {
        if (isJumping)
        {
         
            if (currentJumpPercentage > lowestJumpPercentage)
                currentJumpPercentage -= Time.deltaTime * 0.5f;
            Player.transform.position += jumpSpeed * Time.deltaTime * currentJumpPercentage;
            if (Player.transform.position.y >= jumpEnd)
            {
                isLanding = true;
                isJumping = false;
               GameObject obj= Instantiate(particleEffect, transform.position, transform.rotation);
                obj.transform.eulerAngles+= new Vector3(-90,0,0);
            }
        }
        else if(isLanding)
        {
            Player.transform.position -= jumpSpeed * Time.deltaTime * 2;
            if (Player.transform.position.y <= jumpStart)
            {
                isLanding = false;
                isBusy = false;
                Player.transform.position = new Vector3(Player.transform.position.x,jumpStart, Player.transform.position.z);
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRange);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.tag == "Enemy")
                    {
                        hitCollider.gameObject.GetComponent<BasicEnemy>().ApplyDamage(baseDamage, 0, myElement, 0, this.gameObject,"Special");
                    }
                }

            }
        }

    }
    public override void CalculateDamage(float PATK, float MATK)
    {
        baseDamage = MATK * 8;
    }
}
