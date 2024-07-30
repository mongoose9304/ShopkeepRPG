using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBush : InteractableObject
{
    public int myQuality;
    public float chanceForHigherLoot;
    [SerializeField] bool useSpecificItemDrop;
    LootDropper myDropper;
    public GameObject lootableIndicator;
    public ParticleSystem lootEffect;

    private void Awake()
    {
        myDropper = GetComponent<LootDropper>();
    }
    private void Start()
    {
        if(!useSpecificItemDrop)
        SetUpLootDrop();
    }
    public override void Interact(GameObject interactingObject_ = null)
    {
        myDropper.DropItems();
        gameObject.SetActive(false);
        lootableIndicator.SetActive(false);
        if(lootEffect)
        lootEffect.Play();
    }
    public void SetUpLootDrop()
    {
        int x = myQuality;
        if(Random.Range(0,100)<chanceForHigherLoot)
        {
            x += 1;
        }
        myDropper.SetLootTable(LumberLevelManager.instance.currentLevel.GetItemTier(x));
        lootableIndicator.SetActive(true);
    }
}
