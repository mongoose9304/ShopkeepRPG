using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
public class LootDigSpot : InteractableObject
{
    public int myQuality;
    public float chanceForHigherLoot;
    [SerializeField] bool useSpecificItemDrop;
    LootDropper myDropper;
    public GameObject lootableIndicator;
    public ParticleSystem lootEffect;
    [SerializeField] float maxHoldDuration;
    [SerializeField] float currentHoldDuration;
    [SerializeField] float shakeSpeed;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
    private void Awake()
    {
        myDropper = GetComponent<LootDropper>();
    }
    private void Start()
    {
        if (!useSpecificItemDrop)
            SetUpLootDrop();
        currentHoldDuration = 0;
    }

    public override void Interact(GameObject interactingObject_ = null)
    {
        currentHoldDuration += Time.deltaTime;
        if (currentHoldDuration >= maxHoldDuration)
        {
            DropItems();
        }
        AdjustBar();
    }
    void DropItems()
    {
        myDropper.DropItems();
        gameObject.SetActive(false);
        lootableIndicator.SetActive(false);
        if (lootEffect)
            lootEffect.Play();
    }
    public void SetUpLootDrop()
    {
        int x = myQuality;
        if (Random.Range(0, 100) < chanceForHigherLoot)
        {
            x += 1;
        }
        myDropper.SetLootTable(LumberLevelManager.instance.currentLevel.GetDigItemTier(x));
        lootableIndicator.SetActive(true);
    }
    private void AdjustBar()
    {
        myUIBar.UpdateBar01(currentHoldDuration / maxHoldDuration);
    }
}
