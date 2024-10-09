using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldLootBush : InteractableObject
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
    [SerializeField] MMF_Player feedBackPlayer;
     MMF_RotationShake feedBackShake;
    [SerializeField] AudioSource rustleAudio;
    bool isInteracting;
    private void Awake()
    {
        myDropper = GetComponent<LootDropper>();
    }
    private void Start()
    {
        if (!useSpecificItemDrop)
            SetUpLootDrop();
        currentHoldDuration = 0;
        Debug.Log("Feedback counts " + feedBackPlayer.FeedbacksList.Count);
        feedBackShake = (MMF_RotationShake)feedBackPlayer.FeedbacksList[0];
    }
    private void Update()
    {
        if (!isInteracting)
        {
            if (rustleAudio.isPlaying)
            {
                rustleAudio.Stop();
            }
        }
        if (isInteracting)
        {
            isInteracting = false;
        }
    }

    public override void Interact(GameObject interactingObject_ = null)
    {
        isInteracting = true;
        if (!rustleAudio.isPlaying)
        {
            rustleAudio.Play();
        }
        currentHoldDuration += Time.deltaTime;
        feedBackPlayer.PlayFeedbacks();
        feedBackShake.ShakeSpeed = shakeSpeed * (currentHoldDuration/maxHoldDuration);
        if(currentHoldDuration>=maxHoldDuration)
        {
            DropItems();
        }
    }
    void DropItems()
    {
        rustleAudio.Stop();
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
        myDropper.SetLootTable(LumberLevelManager.instance.currentLevel.GetItemTier(x));
        lootableIndicator.SetActive(true);
    }
    private void ShakeEffect()
    {
        
    }
}
