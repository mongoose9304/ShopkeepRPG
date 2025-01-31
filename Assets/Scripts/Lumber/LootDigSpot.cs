using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.Events;
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
    [SerializeField] AudioSource diggingAudio;
    float timeSinceInteration;
    float startVolume;
    [Tooltip("REFERNCE to the UI bar that fills up as held")]
    public MMProgressBar myUIBar;
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
        startVolume = diggingAudio.volume;
    }
    private void Update()
    {
        if (timeSinceInteration<=0)
        {
            if (diggingAudio.isPlaying)
            {
                diggingAudio.volume -= startVolume * Time.deltaTime * 3;
            }
        }
        if(timeSinceInteration >= 0)
        {
            timeSinceInteration -= Time.deltaTime ;
        }
    }
    public override void Interact(GameObject interactingObject_ = null, InteractLockOnButton btn = null)
    {
        timeSinceInteration = 0.5f;
        diggingAudio.volume = startVolume;
        if (!diggingAudio.isPlaying)
        {
            diggingAudio.Play();
        }
        if (interactingObject_.TryGetComponent<LumberPlayer>(out LumberPlayer p_))
        {
            currentHoldDuration += Time.deltaTime*p_.shovelPower;
        }
        else
        currentHoldDuration += Time.deltaTime;
        if (currentHoldDuration >= maxHoldDuration)
        {
            DropItems();
        }
        AdjustBar();
        if (btn)
        {
            btn.IsInteracting(maxHoldDuration, currentHoldDuration);
        }
    }
    void DropItems()
    {
        StartCoroutine(FadeAudio());
        myDropper.DropItems();
        gameObject.SetActive(false);
        lootEvent.Invoke();
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
        //myUIBar.UpdateBar01(currentHoldDuration / maxHoldDuration);
    }
    IEnumerator FadeAudio()
    {
        if (timeSinceInteration <= 0)
        {
            if (diggingAudio.isPlaying)
            {
                diggingAudio.volume -= startVolume * Time.deltaTime * 6;
                if (diggingAudio.volume <= 0.1f)
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
