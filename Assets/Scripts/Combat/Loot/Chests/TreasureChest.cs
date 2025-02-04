using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : InteractableObject
{
    public int value;
    public int curseSeverity;
    public bool isCursed;
    protected bool playerInRange;
    protected bool isOpening;
    [SerializeField]
    protected Transform spawnLocation;
    [SerializeField] protected GameObject myText;
    [SerializeField] ParticleSystem OpenEffect;
    [SerializeField] ParticleSystem OpenCursedEffect;
    [SerializeField] GameObject curseIcon;
    [SerializeField] Animator anim;

    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
    {
        InteractWithChest();
    }
    virtual protected void OpenChest()
    {
        if (isOpening)
            return;
        myText.SetActive(false);
        playerInRange = false;
        isOpening = true;
        if(isCursed)
        {
            DungeonManager.instance.AddRandomCurse(curseSeverity);
            OpenCursedEffect.Play();
        }
        CombatPlayerManager.instance.RemoveInteractableObject(gameObject);
        if(anim)
        {
            anim.SetBool("Open", true);
        }
        gameObject.SetActive(false);
        Invoke("OpenChestEffectDelay",0.3f);
    }
    virtual protected void OpenChestEffectDelay()
    {
        value = DungeonManager.instance.currentDungeon.GetTreasureChestAmount();
        CoinSpawner.instance_.CreateDemonCoins(value, spawnLocation);
        OpenEffect.Play();
    }
    virtual public void SetIsCursed(bool isCursed_,int severity_)
    {
        isCursed = isCursed_;
        curseSeverity = severity_;
        curseIcon.SetActive(isCursed);
    }

    virtual public void InteractWithChest()
    {
        OpenChest();
    }
    virtual protected void ToggleInteractablity(bool inRange_)
    {
        if (isOpening)
            return;
        myText.SetActive(inRange_);
        playerInRange = inRange_;

    }
    
}
