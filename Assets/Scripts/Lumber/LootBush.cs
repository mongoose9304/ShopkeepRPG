using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBush : InteractableObject
{
    public int myQuality;
    public float chanceForHigherLoot;
    [SerializeField] bool useSpecificItemDrop;
    LootDropper myDropper;

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
    }
    public void SetUpLootDrop()
    {
        int x = myQuality;
        if(Random.Range(0,100)<chanceForHigherLoot)
        {
            x += 1;
        }
        myDropper.SetLootTable(LumberLevelManager.instance.currentLevel.GetItemTier(x));
      
    }
}
