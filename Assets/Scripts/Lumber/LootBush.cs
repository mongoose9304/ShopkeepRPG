using MoreMountains.Tools;
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
    public AudioClip bushAudio;
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
       if( interactingObject_.TryGetComponent<LumberPlayer>(out LumberPlayer player_))
        {
            player_.RemoveObjectFromInteractableObjects(this.gameObject);
        }
        MMSoundManager.Instance.PlaySound(bushAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
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
