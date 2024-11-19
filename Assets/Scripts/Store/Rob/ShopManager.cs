using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Unity.AI.Navigation;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public List<ItemData> hotItems=new List<ItemData>();
    public List<ItemData> hotItemsHell=new List<ItemData>();
    public List<ItemData> coldItems=new List<ItemData>();
    public List<ItemData> coldItemsHell=new List<ItemData>();
    public bool playerInHell;
    public bool hellShopEnabled;
    public bool humanShopEnabled;
    public bool shopRunning;
    public TextMeshProUGUI cashEarnedText;
    public MMF_Player cashSymbol;
    public MMF_Player stealAlert;
    public MMF_Player cashSymbolHell;
    public MMF_Player stealAlertHell;
    public TextMeshProUGUI cashEarnedTextHell;
    public StorePlayer player;
    public GameObject storeRoom;
    public GameObject storeRoomHell;
    public GameObject[] exitSpots;
    public GameObject[] exitSpotsHell;
    public int currentCashEarned;
    public int currentCashEarnedHell;
    public PedestalScreen pedScreen;
    public InventoryUI invScreen;
    public BarginBinScreen barginScreen;
    public HaggleUI haggleScreen;
    public MoveableObjectUI moveableObjectScreen;
    public bool inMenu;
    MMF_TMPCountTo cashCounter;
    MMF_TMPCountTo cashCounterHell;
    [SerializeField] MMF_Player cashFeedback;
    [SerializeField] MMF_Player cashFeedbackHell;
    public List<Pedestal> regularPedestals = new List<Pedestal>();
    public List<Pedestal> windowPedestals = new List<Pedestal>();
    public List<Pedestal> regularPedestalsHell = new List<Pedestal>();
    public List<Pedestal> windowPedestalsHell = new List<Pedestal>();
    public List<BarginBin> barginBins = new List<BarginBin>();
    public List<BarginBin> barginBinsHell = new List<BarginBin>();
    public List<ShopDoor> mydoors = new List<ShopDoor>();
    public List<ShopDoor> mydoorsHell = new List<ShopDoor>();
    public CashRegister cashRegister;
    public CashRegister cashRegisterHell;
    public List<Thief> currentThieves=new List<Thief>();
    public List<InventoryItem> debugItemsToAdd=new List<InventoryItem>();
    public List<InventoryItem> debugItemsToAdd2=new List<InventoryItem>();
    [SerializeField] private List<Pedestal> allPedestals = new List<Pedestal>();
    [SerializeField] private List<BarginBin> allBarginBins = new List<BarginBin>();
    public Transform teleportLocationHuman;
    public ParticleSystem[] teleportEffectsHuman;
    public Transform teleportLocationHell;
    public ParticleSystem[] teleportEffectsHell;
    public AudioClip[] cashAudios;
    public List<AudioClip> BGMs = new List<AudioClip>();
    public List<AudioClip> ShopActiveBGMs = new List<AudioClip>();
    public AudioClip hoverUIAudio;
    public AudioClip clickUIAudio;
    public AudioClip sliderUIAudio;
    public AudioClip closeUIAudio;
    public AudioClip enterHellAudio;
    public AudioClip openShopAudio;
    public NavMeshSurface surface;
    private void Awake()
    {
        instance = this;

        if (cashFeedback)
            cashCounter = cashFeedback.GetFeedbackOfType<MMF_TMPCountTo>();

        if (cashFeedbackHell)
            cashCounterHell = cashFeedbackHell.GetFeedbackOfType<MMF_TMPCountTo>();
      
    }
    private void Start()
    {
        SetUpLevel();
    }
    public void SetUpLevel()
    {
        SetPedestalList();
        SetBarginBinList();
        LoadAllPedestals();
        LoadAllBarginBins();
        PlayRandomBGM();
        StartCoroutine(WaitAFrameBeforeRedoingNavmesh());
    }
    public void OpenPedestal(Pedestal p_)
    {
        pedScreen.gameObject.SetActive(true);
        pedScreen.OpenMenu(p_);
        inMenu = true;
    }
    public void OpenBarginBin(BarginBin b_)
    {
        barginScreen.gameObject.SetActive(true);
        barginScreen.OpenMenu(b_);
        inMenu = true;
    }
    public void OpenHaggleScreen(Pedestal p_,Customer c_,float haggleStart_)
    {
        haggleScreen.gameObject.SetActive(true);
        haggleScreen.OpenMenu(p_,c_,haggleStart_);
        inMenu = true;
    }
    public void OpenMoveableObjectScreen()
    {
        moveableObjectScreen.gameObject.SetActive(true);
        moveableObjectScreen.OpenMenu();
        inMenu = true;
    }
    public void CloseMenu()
    {
        inMenu = false;
        pedScreen.gameObject.SetActive(false);
        barginScreen.gameObject.SetActive(false);
        barginScreen.CloseMenu();
        haggleScreen.gameObject.SetActive(false);
        haggleScreen.CloseMenu();
        moveableObjectScreen.gameObject.SetActive(false);
        moveableObjectScreen.CloseMenu();
        invScreen.OpenMenu(false);

         
        
    }
    public void MenuBackButton()
    {
        CloseMenu();
    }
    public void ResetCashEarned()
    {
        currentCashEarned = 0;
        cashEarnedText.text = currentCashEarned.ToString();
        currentCashEarnedHell = 0;
        cashEarnedTextHell.text = currentCashEarnedHell.ToString();
    }
    public void AddCash(int cash, bool inHell = false)
    {
        if (!inHell)
        {
            cashCounter.CountFrom = currentCashEarned;
            currentCashEarned += cash;
            cashCounter.CountTo = currentCashEarned;
            cashFeedback.PlayFeedbacks();
        }
        else
        {
            cashCounterHell.CountFrom = currentCashEarnedHell;
            currentCashEarnedHell += cash;
            cashCounterHell.CountTo = currentCashEarnedHell;
            cashFeedbackHell.PlayFeedbacks();
        }
        MMSoundManager.Instance.PlaySound(cashAudios[Random.Range(0,cashAudios.Length)], MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.95f, 1.05f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public GameObject GetRandomNPCExit(bool inHell = false)
    {
        if (!inHell)
        {    
        return exitSpots[Random.Range(0, exitSpots.Length)];
        }
        else
        {
        return exitSpotsHell[Random.Range(0, exitSpotsHell.Length)];
        }
    }
    public GameObject GetStoreRoom(bool inHell = false)
    {
        if (!inHell)
        {
            return storeRoom;
        }
        else
        {
            return storeRoomHell;
        }
    }
    public void RemoveInteractableObject(GameObject obj)
    {
        if (player.myInteractableObjects.Contains(obj))
            player.RemoveInteractableObject(obj);
    }
    public GameObject GetRandomTargetPedestal(float chanceToTargetWindows,bool inHell=false)
    {
        if (!inHell)
        {
            if (windowPedestals.Count > 0)
            {
                if (Random.Range(0, 1.0f) < chanceToTargetWindows)
                    return windowPedestals[Random.Range(0, windowPedestals.Count)].gameObject;
            }
            if (regularPedestals.Count > 0)
            {
                return regularPedestals[Random.Range(0, regularPedestals.Count)].gameObject;
            }
            return null;
        }
        else
        {
            if (windowPedestalsHell.Count > 0)
            {
                if (Random.Range(0, 1.0f) < chanceToTargetWindows)
                    return windowPedestalsHell[Random.Range(0, windowPedestalsHell.Count)].gameObject;
            }
            if (regularPedestalsHell.Count > 0)
            {
                return regularPedestalsHell[Random.Range(0, regularPedestalsHell.Count)].gameObject;
            }
            return null;
        }
    }
    public GameObject GetRandomTargetBarginBin(bool inHell = false)
    {
        if (!inHell)
        {
            if (barginBins.Count > 0)
            {
             return barginBins[Random.Range(0, barginBins.Count)].gameObject;
            }
            return null;
        }
        else
        {
            if (barginBinsHell.Count > 0)
            {
                return barginBinsHell[Random.Range(0, barginBinsHell.Count)].gameObject;
            }
            return null;
        }
    }
    public void HeadToCashRegister(Customer C_,bool inHell=false)
    {
        if(!inHell)
        {
            cashRegister.AddCustomer(C_);
            cashRegister.SetCustomerTargets();
        }
        else
        {
            cashRegisterHell.AddCustomer(C_);
            cashRegisterHell.SetCustomerTargets();
        }
    }
    public void OpenShop()
    {
        if(player.isInMovingMode)
        {
            foreach (ShopDoor door_ in mydoors)
            {
                door_.ResetDoor();
            }
            return;
        }
        if (!humanShopEnabled && !hellShopEnabled)
        {
            foreach (ShopDoor door_ in mydoors)
            {
                door_.ResetDoor();
            }
            return;
        }
        if (humanShopEnabled)
        {
            CustomerManager.instance.OpenShop(8, 4);
            foreach (ShopDoor door_ in mydoors)
            {
                door_.RotateDoor();
            }
        }
        if (hellShopEnabled)
        {
            CustomerManager.instance.OpenShop(8, 0, true);
            foreach (ShopDoor door_ in mydoorsHell)
            {
                door_.RotateDoor();
            }
        }
        shopRunning = true;
        PlayRandomShopActiveBGM();
        MMSoundManager.Instance.PlaySound(openShopAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, 1, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);

    }
    public void ReturnItemToInventory(ItemData item_,int amount)
    {
        if(amount>0)
        {
            invScreen.AddItemToInventory(item_, amount);
        }
    }
    public void SetStealAlert(bool isStealing,bool inHell=false)
    {
        if(isStealing)
        {
            if(!inHell)
            {
                cashSymbol.gameObject.SetActive(false);
                stealAlert.gameObject.SetActive(true);
                stealAlert.GetComponent<MMF_Player>().PlayFeedbacks();
            }
            else
            {
                cashSymbolHell.gameObject.SetActive(false);
                stealAlertHell.gameObject.SetActive(true);
                stealAlertHell.GetComponent<MMF_Player>().PlayFeedbacks();
            }
        }
        else
        {
            if (!inHell)
            {
                cashSymbol.gameObject.SetActive(true);
                stealAlert.gameObject.SetActive(false);
            }
            else
            {
                cashSymbolHell.gameObject.SetActive(true);
                stealAlertHell.gameObject.SetActive(false);
            }
        }
    }
    public void EnterHell()
    {
        playerInHell = true;
        foreach(Thief t_ in currentThieves)
        {
            t_.CheckSpeed();
        }
        foreach (ParticleSystem sys in teleportEffectsHell)
        {
            sys.Play();
        }
        MMSoundManager.Instance.PlaySound(enterHellAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, 1, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void ExitHell()
    {
        playerInHell = false;
        foreach (Thief t_ in currentThieves)
        {
            t_.CheckSpeed();
        }
        foreach(ParticleSystem sys in teleportEffectsHuman)
        {
            sys.Play();
        }
        MMSoundManager.Instance.PlaySound(enterHellAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, 1, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void DebugSaveItems()
    {
        PlayerInventory.instance.UpdateItems(invScreen.slots);
        PlayerInventory.instance.UpdateMoveableItems(moveableObjectScreen.invUI.slots);
        PlayerInventory.instance.SaveItems();
        SaveAllPedestals();
        SaveAllBarginBins();
        
    }
    public void DebugAddItems()
    {
        foreach(InventoryItem item_ in debugItemsToAdd)
        invScreen.AddItemToInventory(PlayerInventory.instance.GetItem(item_.myItemName), item_.amount);
    }
    public void DebugAddItems2()
    {
        foreach (InventoryItem item_ in debugItemsToAdd2)
            invScreen.AddItemToInventory(PlayerInventory.instance.GetItem(item_.myItemName), item_.amount);
    }
    //grabs all the pedestals from the moveable object list
    public void SetPedestalList()
    {
        //need to add hell
        windowPedestals.Clear();
        windowPedestalsHell.Clear();
        regularPedestals.Clear();
        regularPedestalsHell.Clear();
        for(int i=0;i<MoveableObjectManager.instance.humanSlots.Count;i++)
        {
            if(MoveableObjectManager.instance.humanSlots[i].worldObject!=null)
            {
                if (MoveableObjectManager.instance.humanSlots[i].worldObject.GetComponentInChildren<Pedestal>())
                {
                    if(MoveableObjectManager.instance.humanSlots[i].isWindow)
                    {
                        windowPedestals.Add(MoveableObjectManager.instance.humanSlots[i].worldObject.GetComponentInChildren<Pedestal>());
                    }
                    else
                    {
                        regularPedestals.Add(MoveableObjectManager.instance.humanSlots[i].worldObject.GetComponentInChildren<Pedestal>());
                    }
                }
            }
        }
        for (int i = 0; i < MoveableObjectManager.instance.hellSlots.Count; i++)
        {
            if (MoveableObjectManager.instance.hellSlots[i].worldObject!=null)
            {
                if (MoveableObjectManager.instance.hellSlots[i].worldObject.GetComponentInChildren<Pedestal>())
                {
                    if (MoveableObjectManager.instance.hellSlots[i].isWindow)
                    {
                        windowPedestalsHell.Add(MoveableObjectManager.instance.hellSlots[i].worldObject.GetComponentInChildren<Pedestal>());
                    }
                    else
                    {
                        regularPedestalsHell.Add(MoveableObjectManager.instance.hellSlots[i].worldObject.GetComponentInChildren<Pedestal>());
                    }
                }
            }
        }
        InitPedestalList();
    }
    //grabs all the bins from the moveable object list
    public void SetBarginBinList()
    {
        //need to add hell
        barginBins.Clear();
        barginBinsHell.Clear();
        for (int i = 0; i < MoveableObjectManager.instance.humanSlots.Count; i++)
        {
            if (MoveableObjectManager.instance.humanSlots[i].worldObject)
            {
                if (MoveableObjectManager.instance.humanSlots[i].worldObject.GetComponentInChildren<BarginBin>())
                {
                  barginBins.Add(MoveableObjectManager.instance.humanSlots[i].worldObject.GetComponentInChildren<BarginBin>());
                }
            }
        }
        for (int i = 0; i < MoveableObjectManager.instance.hellSlots.Count; i++)
        {
            if (MoveableObjectManager.instance.hellSlots[i].worldObject)
            {
                if (MoveableObjectManager.instance.hellSlots[i].worldObject.GetComponentInChildren<BarginBin>())
                {
                    barginBinsHell.Add(MoveableObjectManager.instance.hellSlots[i].worldObject.GetComponentInChildren<BarginBin>());
                }
            }
        }
        InitBarginBinList();
    }
    private void InitPedestalList()
    {
        allPedestals.Clear();
        allPedestals.AddRange(windowPedestals);
        allPedestals.AddRange(regularPedestals);
        allPedestals.AddRange(windowPedestalsHell);
        allPedestals.AddRange(regularPedestalsHell);
    }
    private void InitBarginBinList()
    {
        allBarginBins.Clear();
        allBarginBins.AddRange(barginBins);
        allBarginBins.AddRange(barginBinsHell);
    }
    private void SaveAllPedestals()
    {
        List<InventoryItem> masterItemList_=new List<InventoryItem>();
        List<InventoryItem> masterPreviousItemList_=new List<InventoryItem>();
        for (int i = 0; i < allPedestals.Count; i++)
        {
            InventoryItem item_ = new InventoryItem();
            if (allPedestals[i].myItem)
            {
                
                item_.myItemName = allPedestals[i].myItem.itemName;
                item_.amount = allPedestals[i].amount;


                masterItemList_.Add(item_);
            }
            else
            {
                item_.myItemName = null;
                item_.amount = 0;


                masterItemList_.Add(item_);
            }
            if (allPedestals[i].myItemPrevious)
            {

                item_.myItemName = allPedestals[i].myItemPrevious.itemName;
                item_.amount = allPedestals[i].amountPrevious;


                masterPreviousItemList_.Add(item_);
            }
            else
            {
                item_.myItemName = null;
                item_.amount = 0;


                masterPreviousItemList_.Add(item_);
            }
        }
        FileHandler.SaveToJSON(masterItemList_, "PedestalInventory");
        FileHandler.SaveToJSON(masterPreviousItemList_, "PedestalInventoryPrevious");
    }
    private void LoadAllPedestals()
    {
        List<InventoryItem> masterItemList_ = FileHandler.ReadListFromJSON<InventoryItem>("PedestalInventory");
        List<InventoryItem> masterItemListPrevious_ = FileHandler.ReadListFromJSON<InventoryItem>("PedestalInventoryPrevious");
        if (masterItemList_ != null)
        {
           for(int i=0;i<masterItemList_.Count;i++)
            {
                if(masterItemList_[i].myItemName!="")
                {
                    allPedestals[i].SetItem(PlayerInventory.instance.GetItem(masterItemList_[i].myItemName), masterItemList_[i].amount);
                }
            }
            for (int i = 0; i < masterItemListPrevious_.Count; i++)
            {
                if (masterItemListPrevious_[i].myItemName != "")
                {
                    allPedestals[i].SetPreviousItem(PlayerInventory.instance.GetItem(masterItemListPrevious_[i].myItemName), masterItemListPrevious_[i].amount);
                }
            }
        }
    }
    private void SaveAllBarginBins()
    {
        List<InventoryItemList> masterItemList_ = new List<InventoryItemList>();
        List<InventoryItemList> masterItemListPrevious_ = new List<InventoryItemList>();
        List<float> discounts = new List<float>();
        for (int i = 0; i < allBarginBins.Count; i++)
        {
            InventoryItemList listX = new InventoryItemList();
            masterItemList_.Add(listX);
            for (int x = 0; x < allBarginBins[i].binSlots.Count; x++)
            {
                InventoryItem item_ = new InventoryItem();
                if (allBarginBins[i].binSlots[x].myItem)
                {

                    item_.myItemName = allBarginBins[i].binSlots[x].myItem.itemName;
                    item_.amount = allBarginBins[i].binSlots[x].amount;


                    
                }
                else
                {
                    item_.myItemName = null;
                    item_.amount = 0;


                    
                }
                masterItemList_[i].myList.Add(item_);
            }
            discounts.Add(allBarginBins[i].itemDiscount);
        }
        for (int i = 0; i < allBarginBins.Count; i++)
        {
            InventoryItemList listY = new InventoryItemList();
            masterItemListPrevious_.Add(listY);
            for (int x = 0; x < allBarginBins[i].binSlotsPrevious.Count; x++)
            {
                InventoryItem item_ = new InventoryItem();
                if (allBarginBins[i].binSlotsPrevious[x].myItem)
                {

                    item_.myItemName = allBarginBins[i].binSlotsPrevious[x].myItem.itemName;
                    item_.amount = allBarginBins[i].binSlotsPrevious[x].amount;



                }
                else
                {
                    item_.myItemName = null;
                    item_.amount = 0;



                }
                masterItemListPrevious_[i].myList.Add(item_);
            }
        }
        FileHandler.SaveToJSON(masterItemList_, "BarginBinInventory");
        FileHandler.SaveToJSON(masterItemListPrevious_, "BarginBinInventoryPrevious");
        FileHandler.SaveToJSON(discounts, "BarginBinDiscounts");
    }
   
    private void LoadAllBarginBins()
    {
        List<InventoryItemList> masterItemList_ = FileHandler.ReadListFromJSON<InventoryItemList>("BarginBinInventory");
        List<InventoryItemList> masterItemListPrevious_ = FileHandler.ReadListFromJSON<InventoryItemList>("BarginBinInventoryPrevious");
        List<float> discounts = FileHandler.ReadListFromJSON<float>("BarginBinDiscounts");
        if (masterItemList_ != null)
        {
            for (int i = 0; i < masterItemList_.Count; i++)
            {
                for (int x = 0; x < masterItemList_[i].myList.Count; x++)
                {
                    if (masterItemList_[i].myList[x].myItemName != "")
                    {
                        allBarginBins[i].SetSlot(x, PlayerInventory.instance.GetItem(masterItemList_[i].myList[x].myItemName), masterItemList_[i].myList[x].amount);
                    }
                }
                if (discounts.Count > i)
                {
                    allBarginBins[i].itemDiscount = discounts[i];
                    allBarginBins[i].UpdateSlotsWithItems();
                    allBarginBins[i].ApplyDiscountToAllItems();
                }
            }
        }
        if (masterItemListPrevious_ != null)
        {
            for (int i = 0; i < masterItemListPrevious_.Count; i++)
            {
                for (int x = 0; x < masterItemListPrevious_[i].myList.Count; x++)
                {
                    if (masterItemListPrevious_[i].myList[x].myItemName!="")
                        allBarginBins[i].SetPreviousSlot(x, PlayerInventory.instance.GetItem(masterItemListPrevious_[i].myList[x].myItemName), masterItemListPrevious_[i].myList[x].amount);
                }
            }
        }
    }
    public void EndShopEvent()
    {
        foreach (ShopDoor door_ in mydoors)
        {
            door_.ResetDoor();
        }
    }
    public void ToggleShopOpen(bool enabled_,bool inHell_)
    {
       
            if (!inHell_)
            {
                humanShopEnabled = enabled_;
            }
            else
            {
                hellShopEnabled = enabled_;
            }

    }
    public void CloseShop()
    {
        if (currentThieves.Count > 0)
            return;
        shopRunning = false;
        foreach (ShopDoor door_ in mydoors)
        {
            door_.ResetDoor();
        }
        foreach (ShopDoor door_ in mydoorsHell)
        {
            door_.RotateDoor();
        }
        CustomerManager.instance.CloseShop();
    }
    public bool CheckIfShopIsOpen(bool inHell_)
    {
        if(!inHell_)
        {
            return humanShopEnabled;
        }
        else
        {
            return hellShopEnabled;
        }
    }
    public void WarpPlayerToOtherStore()
    {
        if(playerInHell)
        {
            ExitHell();
            player.gameObject.transform.position = teleportLocationHuman.position;
        }
        else
        {
            EnterHell();
            player.gameObject.transform.position = teleportLocationHell.position;
        }
    }
    public void PlayRandomBGM()
    {
        MMSoundManager.Instance.StopTrack(MMSoundManager.MMSoundManagerTracks.Music);
        MMSoundManager.Instance.PlaySound(BGMs[Random.Range(0, BGMs.Count)], MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero, true,0.6f);
    }
    public void PlayRandomShopActiveBGM()
    {
        MMSoundManager.Instance.StopTrack(MMSoundManager.MMSoundManagerTracks.Music);
        MMSoundManager.Instance.PlaySound(ShopActiveBGMs[Random.Range(0, ShopActiveBGMs.Count)], MMSoundManager.MMSoundManagerTracks.Music, Vector3.zero, true);
    }
    public void PlayUIAudio(string Audio)
    {
        switch(Audio)
        {
            case "Hover":
                MMSoundManager.Instance.PlaySound(hoverUIAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.98f, 1.02f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                break;
            case "Close":
                MMSoundManager.Instance.PlaySound(closeUIAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.98f, 1.02f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                break;
            case "Slider":
                MMSoundManager.Instance.PlaySound(sliderUIAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 0.6f, 0, false, 0, 1, null, false, null, null, Random.Range(0.98f, 1.02f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                break;
            case "Click":
                MMSoundManager.Instance.PlaySound(clickUIAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
    false, 1.0f, 0, false, 0, 1, null, false, null, null, Random.Range(0.98f, 1.02f), 0, 0.0f, false, false, false, false, false, false, 128, 1f,
    1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
                break;
        }
    }
    public void ToggleMoveMode()
    {
        if(!shopRunning)
        player.ToggleMoveMode();
    }
    public void RedoNavMesh()
    {
        surface.BuildNavMesh();
    }
    IEnumerator WaitAFrameBeforeRedoingNavmesh()
    {
        yield return new WaitForSeconds(0.001f);
        RedoNavMesh();
    }
    public MoveableObject GetPlayerHeldMoveableItem()
    {
       return player.GetHeldObject();
    }
    public void SetPlayerHeldMoveableItem(MoveableObject obj_)
    {
        player.SetHeldObject(obj_);
    }
    //1=hot, 2=cold 0= neutral;
    public int CheckIfItemIsHot(ItemData itemToCheck,bool inHell=false)
    {
        if (!inHell)
        {
            foreach (ItemData data in hotItems)
            {
                if (itemToCheck.itemName == data.itemName)
                    return 1;
            }
            foreach (ItemData data in coldItems)
            {
                if (itemToCheck.itemName == data.itemName)
                    return 2;
            }
            return 0;
        }
        else
        {
            foreach (ItemData data in hotItemsHell)
            {
                if (itemToCheck.itemName == data.itemName)
                    return 1;
            }
            foreach (ItemData data in coldItemsHell)
            {
                if (itemToCheck.itemName == data.itemName)
                    return 2;
            }
            return 0;
        }
    }
    public float GetColdItemMultiplier()
    {
        return 0.6f;
    }
    public float GetHotItemMultiplier()
    {
        return 1.3f;
    }
}
