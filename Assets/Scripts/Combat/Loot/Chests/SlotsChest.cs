using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Tools;

public class SlotsChest : TreasureChest
{
   public SlotMachineSingle slotMachine;
    public int cost;
    public TextMeshProUGUI costText;
    public AudioClip winAudio;
    public AudioClip loseAudio;
    public AudioClip jackpotAudio;
    public AudioSource spinAudioSource;
    private void OnEnable()
    {
        cost = DungeonManager.instance.currentDungeon.GetTreasureChestAmount() / 3;
        costText.text = cost.ToString();
        myText.SetActive(true);
    }
    protected override void OpenChest()
    {
        if (slotMachine.isSpinning)
            return;
        if(LootManager.instance.AttemptDemonPayment(cost))
        {
            Debug.Log("Payed2sin");
        slotMachine.Spin();
            spinAudioSource.Play();
        }

    }
    public void Jackpot()
    {
        value = DungeonManager.instance.currentDungeon.GetTreasureChestAmount();
        CoinSpawner.instance_.CreateDemonCoins(value*3, spawnLocation);
        base.OpenChest();
        spinAudioSource.Stop();
        MMSoundManager.Instance.PlaySound(jackpotAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 0.8f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void Loss()
    {
        spinAudioSource.Stop();
        MMSoundManager.Instance.PlaySound(loseAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.5f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void Win()
    {
        value = DungeonManager.instance.currentDungeon.GetTreasureChestAmount();
        CoinSpawner.instance_.CreateDemonCoins(value, spawnLocation);
        base.OpenChest();
        spinAudioSource.Stop();
        MMSoundManager.Instance.PlaySound(winAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
         false, 1.5f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
         1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    protected override void ToggleInteractablity(bool inRange_)
    {
        if (isOpening)
            return;
        cost = DungeonManager.instance.currentDungeon.GetTreasureChestAmount() / 3;
        costText.text = cost.ToString();
        myText.SetActive(inRange_);
        playerInRange = inRange_;
   

    }
}
