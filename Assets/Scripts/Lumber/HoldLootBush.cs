using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    float timeSinceInteration;
    float startVolume;
    public UnityEvent lootEvent;
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
        startVolume = rustleAudio.volume;
    }
    private void Update()
    {
        if (timeSinceInteration<=0)
        {
            if (rustleAudio.isPlaying)
            {
                rustleAudio.volume -= startVolume * Time.deltaTime * 3;
            }
        }
        if (timeSinceInteration >= 0)
        {
            timeSinceInteration -= Time.deltaTime;
        }
    }

    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
    {
        timeSinceInteration = 0.5f;
        rustleAudio.volume = startVolume;
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
        timeSinceInteration = 0.1f;
        StartCoroutine(FadeAudio());
        myDropper.DropItems();
        gameObject.SetActive(false);
        lootableIndicator.SetActive(false);
        lootEvent.Invoke();
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
    IEnumerator FadeAudio()
    {
        if (timeSinceInteration <= 0)
        {
            if (rustleAudio.isPlaying)
            {
                rustleAudio.volume -= startVolume * Time.deltaTime * 6;
                if (rustleAudio.volume <= 0.1f)
                    yield break;
            }
        }
        if (timeSinceInteration >= 0)
        {
            timeSinceInteration -= Time.deltaTime;
        }
        yield return null;
    }
}
