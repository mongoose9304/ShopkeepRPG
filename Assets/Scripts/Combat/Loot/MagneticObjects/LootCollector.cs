using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollector : MonoBehaviour
{
    public ParticleSystem[] coinCollected;
    public ParticleSystem itemCollected;
    public AudioClip[] coinAudios;
    public AudioClip[] rockAudios;
    private void OnTriggerEnter(Collider other)
    {
       
            if (other.tag == "Item")
            {
            Debug.Log("Item found " + other.GetComponent<LootWorldObject>().myItem.name);
            LootManager.instance.AddLootItem(other.GetComponent<LootWorldObject>().myItem);
            other.gameObject.SetActive(false);
            itemCollected.Play();
            }
        if (other.tag == "DemonCoin")
        {
            LootManager.instance.AddDemonMoney(other.GetComponent<DemonCoin>().value);
            other.gameObject.SetActive(false);
            PlayCoinCollectedEffect();
            MMSoundManager.Instance.PlaySound(coinAudios[Random.Range(0,coinAudios.Length)], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 0.2f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
        if (other.tag == "Lumber")
        {
            LootManager.instance.AddResource(other.GetComponent<LumberPickUp>().lumberAmount);
            other.gameObject.SetActive(false);
            PlayCoinCollectedEffect();
        }
        if (other.tag == "Stone")
        {
            LootManager.instance.AddResource(other.GetComponent<StonePickUp>().stoneAmount);
            other.gameObject.SetActive(false);
            PlayCoinCollectedEffect();
            MMSoundManager.Instance.PlaySound(rockAudios[Random.Range(0, rockAudios.Length)], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
          false, 2.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.9f, 1.1f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
          1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
        }
        if (other.tag == "RegularCoin")
        {
            LootManager.instance.AddRegularMoney(other.GetComponent<DemonCoin>().value);
            other.gameObject.SetActive(false);
            PlayCoinCollectedEffect();
        }

    }
    private void PlayCoinCollectedEffect()
    {
        foreach(ParticleSystem sys in coinCollected)
        {
            if(!sys.isPlaying)
            {
                sys.Play();
                break;
            }
        }

    }
}
