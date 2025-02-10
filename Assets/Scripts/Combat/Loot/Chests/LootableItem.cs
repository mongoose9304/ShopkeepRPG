using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableItem : InteractableObject
{
    [SerializeField] LootDropper dropper;
    public int numberOfItemsToDrop;
    [SerializeField] ParticleSystem OpenEffect;
    public GameObject objectToDisable;
    public GameObject objectToEnable;
    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
    {
        LootTheItem();
    }
    
    public void LootTheItem()
    {
        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            dropper.DropSpecificItem(LootManager.instance.GetTieredItem(LootManager.instance.GetRandomItemTier()));
        }
        gameObject.SetActive(false);
        objectToDisable.gameObject.SetActive(false);
        objectToEnable.gameObject.SetActive(true);
        CombatPlayerManager.instance.RemoveInteractableObject(gameObject);
        if(OpenEffect)
        {
            OpenEffect.Play();
        }
    }
}
