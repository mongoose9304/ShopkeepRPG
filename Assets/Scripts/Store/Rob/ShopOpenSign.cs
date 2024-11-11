using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopOpenSign : InteractableObject
{
    public Image signImage;
    public bool isInHell;
    public Transform scaleObject;
    public bool isOpen;
    public Sprite[] signSprites;
    bool isSwapping;
    float swapTime;
    private void Update()
    {
        if(swapTime>0)
        {
            swapTime -= Time.deltaTime;
        }
        if(isSwapping)
        {
            
            if(swapTime<=0)
            {
                scaleObject.DOScale(1.0f, 0.1f);
                if(isOpen)
                signImage.sprite = signSprites[0];
                else
                    signImage.sprite = signSprites[1];
                isSwapping = false;
                swapTime = 0.3f;
            }
        }
    }

    public void ToggleOpen()
    {
        scaleObject.DOScale(0.1f, 0.1f);
        isSwapping = true;
        swapTime = 0.1f;
        isOpen = !isOpen;
        ShopManager.instance.ToggleShopOpen(isOpen,isInHell);
    }
    public override void Interact(GameObject interactingObject_ = null)
    {
        if(!isSwapping&&swapTime<=0)
        ToggleOpen();
    }
}
