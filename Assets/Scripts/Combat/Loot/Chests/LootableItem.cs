using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//will spawn a random lootable item when interacted with. The item and rarity will be determined by the loot manager and teh current dungeon
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
        if (objectToDisable)
            objectToDisable.gameObject.SetActive(false);
        if(objectToEnable)
        objectToEnable.gameObject.SetActive(true);
        CombatPlayerManager.instance.RemoveInteractableObject(gameObject);
        if(OpenEffect)
        {
            OpenEffect.Play();
        }
    }
}
