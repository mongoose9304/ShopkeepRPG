using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MoreMountains.Tools;

public class ShopOpenSign : InteractableObject
{
    public Image signImage;
    public bool isInHell;
    public Transform scaleObject;
    public bool isOpen;
    public Sprite[] signSprites;
    bool isSwapping;
    float swapTime;
    public AudioClip flipSignAudio;
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
        MMSoundManager.Instance.PlaySound(flipSignAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
  false, 1.0f, 0, false, 0, 1, null, false, null, null, 1, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
  1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public override void Interact(GameObject interactingObject_ = null)
    {
        if(!isSwapping&&swapTime<=0)
        ToggleOpen();
    }
}
