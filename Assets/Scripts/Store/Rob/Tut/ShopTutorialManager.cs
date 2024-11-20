using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTutorialManager : TutorialManager
{
    public static ShopTutorialManager instance;
    public ItemData hotItem;
    public TutCustomer customerA;
    public GameObject doorA;
    public List<ShopDoor> doorAPivots = new List<ShopDoor>();
    public ItemData coldItem;
    public List<InventoryItem> tutItemsA = new List<InventoryItem>();
    public List<InventoryItem> tutItemsB = new List<InventoryItem>();
    public InventoryUI invScreen;
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
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
                doorA.SetActive(true);
                break;
            case 3:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
                customerA.gameObject.SetActive(true);
                break;
            case 4:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
                customerA.gameObject.SetActive(false);
                break;
            case 5:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 1, true);
                break;
            case 6:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                break;
            case 7:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
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
}
