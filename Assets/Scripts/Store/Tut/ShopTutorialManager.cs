using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTutorialManager : TutorialManager
{
    public static ShopTutorialManager instance;
    public ItemData hotItem;
    public TutCustomer customerA;
    public TutCustomer customerB;
    public Thief tutThief;
    public GameObject doorA;
    public GameObject moveModeToggle;
    public Transform thiefExit;
    public List<ShopDoor> doorAPivots = new List<ShopDoor>();
    public ItemData coldItem;
    public List<InventoryItem> tutItemsA = new List<InventoryItem>();
    public List<InventoryItem> tutItemsB = new List<InventoryItem>();
    public List<GameObject> tutRemovableWalls = new List<GameObject>();
    public InventoryUI invScreen;
    public GameObject smokeEffect;
    public AudioClip stealAudio;
    public GameObject[] players;
    public Transform tutEndLocation;
    protected override void Awake()
    {
        instance = this;
        instance_ = this;
        inTut = true;
    }
    public void AddTutItems()
    {
        invScreen.ClearAllSlots();
        foreach (InventoryItem item_ in tutItemsA)
            invScreen.AddItemToInventory(PlayerInventory.instance.GetItem(item_.myItemName), item_.amount);
    }
    public void AddTutItemsB()
    {
        invScreen.ClearAllSlots();
        foreach (InventoryItem item_ in tutItemsB)
            invScreen.AddItemToInventory(PlayerInventory.instance.GetItem(item_.myItemName), item_.amount);
    }
    /// <summary>
    /// Set the tutorial to a new state and each state can have its own seperate logic depending on what's needed
    /// </summary>
    public override void SetTutorialState(int tutorialState_)
    {
        tutorialState = tutorialState_;
        switch (tutorialState_)
        {
            case 0:
                tutUIManager.SetJoystickMessage(tutorialMessages[tutorialState]);
                break;
            case 1:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                AddTutItems();
                break;
            case 2:
                //allow door to open
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                doorA.SetActive(true);
                break;
            case 3:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                customerA.gameObject.SetActive(true);
                break;
            case 4:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                tutRemovableWalls[0].SetActive(false);
                customerA.gameObject.SetActive(false);
                AddTutItems();
                break;
            case 5:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                break;
            case 6:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                tutRemovableWalls[1].SetActive(false);
                break;
            case 7:
                tutUIManager.SetMessage(tutorialMessages[tutorialState],3,true);
                customerB.gameObject.SetActive(true);
                break;
            case 8:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                tutRemovableWalls[2].SetActive(false);
                break;
            case 9:
                AddTutItemsB();
                tutUIManager.SetMessage(tutorialMessages[tutorialState],3,true);
                break;
            case 10:
                tutRemovableWalls[3].SetActive(false);
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 2, true);
                break;
            case 11:
                tutRemovableWalls[4].SetActive(false);
                CreateSmoke(moveModeToggle.transform);
                moveModeToggle.SetActive(false);
                ShopManager.instance.ToggleMoveMode();
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                break;

        }
    }
    /// <summary>
    /// Set the tutorial to a new state and each state can have its own seperate logic depending on what's needed
    /// </summary>
    public override void SetAltTutorialState(int tutorialState_)
    {
        tutorialState = tutorialState_;
        switch (tutorialState_)
        {
            case 0:
                tutUIManager.SetJoystickMessage(tutorialMessages[tutorialState]);
                break;
            case 1:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                AddTutItems();
                break;
            case 2:
                //allow door to open
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
                doorA.SetActive(true);
                break;
            case 3:
                tutUIManager.SetMessage(tutorialAlternateMessages[tutorialState], 3, true);
                break;
            case 4:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
                tutRemovableWalls[0].SetActive(false);
                customerA.gameObject.SetActive(false);
                break;
            case 5:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 1, true);
                break;
            case 6:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
                tutRemovableWalls[1].SetActive(false);
                break;
            case 7:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                customerB.gameObject.SetActive(true);
                break;
            case 8:
                tutUIManager.SetMessage(tutorialAlternateMessages[tutorialState]);
                tutRemovableWalls[2].SetActive(false);
                break;

        }
    }
    public void OpenShop(int index_)
    {
        if (index_ == 1&&tutorialState==2)
        {
            foreach(ShopDoor door in doorAPivots)
            {
                door.RotateDoor();
            }
            SetTutorialState(3);
        }
    }
    public void StartSteal(Transform tansform_)
    {
        tutThief.gameObject.SetActive(true);
        tutThief.transform.position = tansform_.position;
        tutThief.SetTarget(thiefExit);
        MMSoundManager.Instance.PlaySound(stealAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
     false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.98f, 1.02f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
     1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public override void EndTutorial()
    {
        foreach (GameObject obj in objectsToDisableDuringTut)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisablePostTut)
        {
            obj.SetActive(false);
        }
        inTut = false;
        invScreen.LoadInventory();
        foreach(GameObject obj in players)
        {
            obj.transform.position = tutEndLocation.position;
        }
    }
    public void CreateSmoke(Transform trans_)
    {
        Instantiate(smokeEffect, trans_.position, trans_.rotation);
    }
}
