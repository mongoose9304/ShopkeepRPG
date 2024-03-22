using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStorm : PlayerSpecialAttack
{
    public GameObject particleEffect;
    [SerializeField] float lowestJumpPercentage;
    [SerializeField] Vector3 jumpSpeed;
    bool isJumping;
    bool isLanding;
    public float jumpHeight;
    float jumpEnd;
    float jumpStart;
    float currentJumpPercentage;
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
            Debug.Log("Jumping");
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
                
            }
        }

    }
}
