using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//simmilar to a lootable item script, this one will display the rarity of the item beforehand and can have a minimum rarity.
public class LootablePillar : InteractableObject
{
    public int minItemTier;
    [SerializeField]int currentItemTier;
    [SerializeField] LootDropper dropper;
    public int numberOfItemsToDrop;
    [SerializeField] ParticleSystem OpenEffect;
    public GameObject objectToDisable;
    public GameObject objectToEnable;
    public GameObject[] rarityDisplayItems;
    private void Start()
    {
        currentItemTier = LootManager.instance.GetRandomItemTier();
        foreach (GameObject obj in rarityDisplayItems)
        {
            obj.SetActive(false);
        }
        if(currentItemTier<minItemTier)
        {
            currentItemTier = minItemTier;
        }
        rarityDisplayItems[currentItemTier].SetActive(true);
    }
    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
    {
        LootTheItem();
    }

    public void LootTheItem()
    {
        for (int i = 0; i < numberOfItemsToDrop; i++)
        {
            dropper.DropSpecificItem(LootManager.instance.GetTieredItem(currentItemTier));
        }
        gameObject.SetActive(false);
        objectToDisable.gameObject.SetActive(false);
        objectToEnable.gameObject.SetActive(true);
        CombatPlayerManager.instance.RemoveInteractableObject(gameObject);
        if (OpenEffect)
        {
            OpenEffect.Play();
        }
    }
}
