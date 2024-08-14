using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public int value;
    public int curseSeverity;
    public bool isCursed;
    bool playerInRange;
    bool isOpening;
    [SerializeField]
    Transform spawnLocation;
    [SerializeField] GameObject myText;
    [SerializeField] ParticleSystem OpenEffect;
    [SerializeField] ParticleSystem OpenCursedEffect;

    virtual protected void Update()
    {
        if (!playerInRange)
            return;


        if (Input.GetButtonUp("Fire1"))
        {
            OpenChest();
        }


    }
    virtual protected void OpenChest()
    {
        if (isOpening)
            return;
        CoinSpawner.instance_.CreateDemonCoins(value,spawnLocation);
        OpenEffect.Play();
        myText.SetActive(false);
        playerInRange = false;
        isOpening = true;
        if(isCursed)
        {
            DungeonManager.instance.AddRandomCurse(curseSeverity);
            OpenCursedEffect.Play();
        }
    }

    virtual public void InteractWithChest()
    {
        OpenChest();
    }
    virtual protected void ToggleInteractablity(bool inRange_)
    {
        if (isOpening)
            return;
        myText.SetActive(inRange_);
        playerInRange = inRange_;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ToggleInteractablity(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            ToggleInteractablity(false);
    }
}
