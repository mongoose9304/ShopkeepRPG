using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Unity.AI.Navigation;

/// <summary>
/// The singleton that manages and controls the shop minigame
/// </summary>
public class ShopManager : MonoBehaviour
{
    [Tooltip("The singleton instance")]
    public static ShopManager instance;
    [Tooltip("Is there two players connected")]
    public bool twoPlayerMode;

    [Header("References")]
    [Tooltip("REFERNCE to the script that loads into the town level")]
    public LoadLevel townLevelLoader;
    [Tooltip("REFERNCE to the script that loads into the town level")]
    public PlayerInNearExitDetector exitDetector;
    [Tooltip("REFERNCE to the text that shows how much money you have")]
    public GameObject cashTextUI;
    [Tooltip("REFERNCE to the UI that says press B to exit")]
    public GameObject exitMenuUI;
    [Tooltip("REFERNCE to the text that displays cash earned in the human world")]
    public TextMeshProUGUI cashEarnedText;
    [Tooltip("REFERNCE to the symbol for human cash")]
    public MMF_Player cashSymbol;
    [Tooltip("REFERNCE symbol that shows you are being robbed in the human world")]
    public MMF_Player stealAlert;
    [Tooltip("REFERNCE to the symbol for hell cash")]
    public MMF_Player cashSymbolHell;
    [Tooltip("REFERNCE symbol that shows you are being robbed in the hell world")]
    public MMF_Player stealAlertHell;
    [Tooltip("REFERNCE to the text that displays cash earned in hell")]
    public TextMeshProUGUI cashEarnedTextHell;
    [Tooltip("REFERNCE to the players")]
    public StorePlayer[] players;
    [Tooltip("REFERNCE to the storeroom for thief targeting ")]
    public GameObject storeRoom;
    [Tooltip("REFERNCE to the storeroom in hell for thief targeting ")]
    public GameObject storeRoomHell;
    [Tooltip("REFERNCE to NPC exits in the human world")]
    public GameObject[] exitSpots;
    [Tooltip("REFERNCE to NPC exits in the hell world")]
    public GameObject[] exitSpotsHell;
    [Tooltip("how much cash have we earned today ")]
    public int currentCashEarned;
    [Tooltip("how much cash have we earned today in hell ")]
    public int currentCashEarnedHell;
    [Tooltip("REFERNCE to the pedestal screen")]
    public PedestalScreen pedScreen;
    [Tooltip("REFERNCE to the inventory screen")]
    public InventoryUI invScreen;
    [Tooltip("REFERNCE to bargin screen")]
    public BarginBinScreen barginScreen;
    //Haggle Section is set up diffrently so both players can haggle at once
    [Tooltip("REFERNCE to haggle screen for player 1")]
    public HaggleUIHolder haggleScreenOriginal;
    [Tooltip("REFERNCE to haggle screen for player 2")]
    public HaggleUIHolder haggleScreenCopy;
    [Tooltip("REFERNCE moveable object inventory screen")]
    public MoveableObjectUI moveableObjectScreen;
    [Tooltip("REFERNCE to the tutorial UI")]
    public GameObject tutScreen;
    [Tooltip("REFERNCE to effects that play when getting human money")]
    [SerializeField] MMF_Player cashFeedback;
    [Tooltip("REFERNCE to effects that play when getting hell money")]
    [SerializeField] MMF_Player cashFeedbackHell;
    [Tooltip("REFERNCE to the doors in the human shop")]
    public List<ShopDoor> mydoors = new List<ShopDoor>();
    [Tooltip("REFERNCE to the doors in the hell shop")]
    public List<ShopDoor> mydoorsHell = new List<ShopDoor>();
    [Tooltip("REFERNCE to the cash register in the human world")]
    public CashRegister cashRegister;
    [Tooltip("REFERNCE to the cash register in the hell world")]
    public CashRegister cashRegisterHell;
    [Tooltip("REFERNCE to the human world teleport effects")]
    public ParticleSystem[] teleportEffectsHuman;
    [Tooltip("REFERNCE to the location you teleport to in the hell world when swapping between shops")]
    public Transform teleportLocationHell;
    [Tooltip("REFERNCE to the hell world teleport effects")]
    public ParticleSystem[] teleportEffectsHell;
    [Tooltip("REFERNCE to the location you teleport to in the human world when swapping between shops")]
    public Transform teleportLocationHuman;
    [Tooltip("REFERNCE to navmesh to rebuild when moving objects around")]
    public NavMeshSurface surface;
    [Tooltip("REFERNCE to canvas the shop UI is on")]
    public Canvas shopUI;
    [Tooltip("REFERNCE to text slots that can pop up with info for players")]
    public TextMeshProUGUI[] playerTextPopUps;

    [Header("Variables")]
    public bool PlayerIsNearExit;
    private bool isLeaving;
    //hot and cold items lists combined
    public List<ItemData> hotItems=new List<ItemData>();
    public List<ItemData> hotItemsHell=new List<ItemData>();
    public List<ItemData> coldItems=new List<ItemData>();
    public List<ItemData> coldItemsHell=new List<ItemData>();
    //hot and cold items lists that change every week or season or event
    public List<ItemData> hotItemsWeekly = new List<ItemData>();
    public List<ItemData> hotItemsHellWeekly = new List<ItemData>();
    public List<ItemData> coldItemsWeekly = new List<ItemData>();
    public List<ItemData> coldItemsHellWeekly = new List<ItemData>();

    //hot and cold items lists Universal, these never change
    public List<ItemData> hotItemsUniversal = new List<ItemData>();
    public List<ItemData> hotItemsHellUniversal = new List<ItemData>();
    public List<ItemData> coldItemsUniversal = new List<ItemData>();
    public List<ItemData> coldItemsHellUniversal = new List<ItemData>();
    [Tooltip("is the player currently in hell, only used in singleplayer")]
    public bool playerInHell;
    [Tooltip("is the hell shop open")]
    public bool hellShopEnabled;
    [Tooltip("is the human shop open")]
    public bool humanShopEnabled;
    [Tooltip("is the shop game currently running, are customers coming into the shop")]
    public bool shopRunning;
    [Tooltip("are we currently in a menu")]
    public bool inMenu;
    [Tooltip("is player 1 haggling")]
    public bool inHaggle;
    [Tooltip("is player 2 haggling")]
    public bool player2InHaggle;
    [Tooltip("is player 2 currently in a menu")]
    public bool player2InMenu;
    MMF_TMPCountTo cashCounter;
    MMF_TMPCountTo cashCounterHell;
    //pedestals and bins are all collected at runtime since players can move/remove them
    [Tooltip("all the human pedestals not near windows")]
    public List<Pedestal> regularPedestals = new List<Pedestal>();
    [Tooltip("all the human pedestals near windows")]
    public List<Pedestal> windowPedestals = new List<Pedestal>();
    [Tooltip("all the hell pedestals not near windows")]
    public List<Pedestal> regularPedestalsHell = new List<Pedestal>();
    [Tooltip("all the hell pedestals near windows")]
    public List<Pedestal> windowPedestalsHell = new List<Pedestal>();
    [Tooltip("all the human bargain bins")]
    public List<BarginBin> barginBins = new List<BarginBin>();
    [Tooltip("all the hell bargain bins")]
    public List<BarginBin> barginBinsHell = new List<BarginBin>();
    [Tooltip("all the thieves currently in the shop")]
    public List<Thief> currentThieves=new List<Thief>();
    [Tooltip("test items")]
    public List<InventoryItem> debugItemsToAdd=new List<InventoryItem>();
    [Tooltip("test items 2")]
    public List<InventoryItem> debugItemsToAdd2=new List<InventoryItem>();
    [Tooltip("all the pedestals in all shops")]
    [SerializeField] private List<Pedestal> allPedestals = new List<Pedestal>();
    [Tooltip("all the bargin bins in all shops")]
    [SerializeField] private List<BarginBin> allBarginBins = new List<BarginBin>();

        [Header("Audios")]
    [Tooltip("audios that play when making money")]
    public AudioClip[] cashAudios;
    [Tooltip("bgms that play while the shop is inactive")]
    public List<AudioClip> BGMs = new List<AudioClip>();
    [Tooltip("bgms that play while the shop is active")]
    public List<AudioClip> ShopActiveBGMs = new List<AudioClip>();
    public AudioClip hoverUIAudio;
    public AudioClip clickUIAudio;
    public AudioClip sliderUIAudio;
    public AudioClip closeUIAudio;
    public AudioClip enterHellAudio;
    public AudioClip openShopAudio;
    private void Awake()
    {
        instance = this;
        CalculateHotItems();
        if (cashFeedback)
            cashCounter = cashFeedback.GetFeedbackOfType<MMF_TMPCountTo>();

        if (cashFeedbackHell)
            cashCounterHell = cashFeedbackHell.GetFeedbackOfType<MMF_TMPCountTo>();
        SetUpHaggleScreens();
    }
    /// <summary>
    /// copy the haggle screen for player 2
    /// </summary>
    private void SetUpHaggleScreens()
    {
        haggleScreenCopy = GameObject.Instantiate(haggleScreenOriginal.gameObject).GetComponent<HaggleUIHolder>();
        haggleScreenCopy.haggleScreen.isPlayer2 = true;
    }
    private void Start()
    {
        SetUpLevel();
    }
    /// <summary>
    /// Get the current player's input to control sliders
    /// </summary>
    public float GetSliderInput()
    {
        if(!player2InMenu)
        return players[0].movement.ReadValue<Vector2>().x;
        else
        {
            return players[1].movement.ReadValue<Vector2>().x;
        }

    }
    /// <summary>
    /// Get a player's input to control sliders
    /// </summary>
    public float GetPlayerSliderInputDirectly(int index)
    {
            return players[index].movement.ReadValue<Vector2>().x;
    }
    /// <summary>
    /// Load the previous layouts and set up the shop to how it was saved before
    /// </summary>
    public void SetUpLevel()
    {
        SetPedestalList();
        SetBarginBinList();
        LoadAllPedestals();
        LoadAllBarginBins();
        PlayRandomBGM();
        StartCoroutine(WaitAFrameBeforeRedoingNavmesh());
    }
    /// <summary>
    /// Try to open a menu if one is not open yet
    /// </summary>
    public bool OpenAMenu(PlayerController controller,bool isPlayer2=false)
    {
        if (inMenu)
        {
            PlayPopUpText("Other Player is in a menu", isPlayer2);
            return false;
        }
        if (isPlayer2)
            player2InMenu = true;
        shopUI.worldCamera = controller.myCam;
        return true;
    }
    /// <summary>
    /// Swap the UI canvas to the player that opened the menu's camera
    /// </summary>
    public void ChangeCameraForUI(PlayerController controller)
    {
        shopUI.worldCamera = controller.myCam;
    }
    /// <summary>
    /// Open the pedestal menu
    /// </summary>
    public void OpenPedestal(Pedestal p_, bool isPlayer2 = false)
    {
        pedScreen.gameObject.SetActive(true);
        pedScreen.OpenMenu(p_,isPlayer2);
        inMenu = true;
        tutScreen.SetActive(false);
        EnableExitMenuButton(true);
    }
    /// <summary>
    /// Open the bargain bin menu
    /// </summary>
    public void OpenBarginBin(BarginBin b_, bool isPlayer2 = false)
    {
        barginScreen.gameObject.SetActive(true);
        barginScreen.OpenMenu(b_,isPlayer2);
        inMenu = true;
        tutScreen.SetActive(false);
        EnableExitMenuButton(true);
    }
    /// <summary>
    /// Open the haggle menu
    /// </summary>
    public void OpenHaggleScreen(Pedestal p_,Customer c_,float haggleStart_, bool isPlayer2 = false)
    {
        Debug.Log("OpenHaggle");
        tutScreen.SetActive(false);
        if (!isPlayer2)
        {
            inHaggle = true;
            haggleScreenOriginal.haggleUICanvas.worldCamera = PlayerManager.instance.GetPlayers()[0].myCam;
            haggleScreenOriginal.haggleScreen.transform.parent.gameObject.SetActive(true);
            haggleScreenOriginal.haggleScreen.gameObject.SetActive(true);
            haggleScreenOriginal.haggleScreen.OpenMenu(p_, c_, haggleStart_);
        }
        else
        {
            player2InHaggle = true;
            haggleScreenCopy.haggleUICanvas.worldCamera = PlayerManager.instance.GetPlayers()[1].myCam;
            haggleScreenCopy.haggleScreen.transform.parent.gameObject.SetActive(true);
            haggleScreenCopy.haggleScreen.gameObject.SetActive(true);
            haggleScreenCopy.haggleScreen.OpenMenu(p_, c_, haggleStart_,true);
        }
    }
    /// <summary>
    /// Open the movable object inventory
    /// </summary>
    public void OpenMoveableObjectScreen()
    {
        moveableObjectScreen.gameObject.SetActive(true);
        moveableObjectScreen.OpenMenu();
        inMenu = true;
        tutScreen.SetActive(false);
        EnableExitMenuButton(true);
    }
    /// <summary>
    /// Close all open menus
    /// </summary>
    public void CloseMenu(bool player2=false)
    {
        if (!player2&&inHaggle)
        {
            haggleScreenOriginal.haggleScreen.gameObject.SetActive(false);
            haggleScreenOriginal.haggleScreen.CloseMenu();
            inHaggle = false;
            if (ShopTutorialManager.instance.inTut)
                tutScreen.SetActive(true);
            return;
        }
        if(player2 && player2InHaggle)
        {
            haggleScreenCopy.haggleScreen.gameObject.SetActive(false);
            haggleScreenCopy.haggleScreen.CloseMenu();
            player2InHaggle = false;
            if (ShopTutorialManager.instance.inTut)
                tutScreen.SetActive(true);
            return;
        }
        inMenu = false;
        pedScreen.gameObject.SetActive(false);
        barginScreen.gameObject.SetActive(false);
        barginScreen.CloseMenu();
        moveableObjectScreen.gameObject.SetActive(false);
        moveableObjectScreen.CloseMenu();
        invScreen.OpenMenu(false);
        if(ShopTutorialManager.instance.inTut)
        tutScreen.SetActive(true);
        EnableExitMenuButton(false);
        player2InMenu = false;

    }
    /// <summary>
    /// Toggle a small UI piece that says press B to close the menu
    /// </summary>
    private void EnableExitMenuButton(bool enable_=false)
    {
        exitMenuUI.SetActive(enable_);
        cashTextUI.SetActive(!enable_);
    }
    /// <summary>
    /// Pressed a button to close the menu
    /// </summary>
    public void MenuBackButton(bool player2=false)
    {
        CloseMenu(player2);
    }
    /// <summary>
    /// Reset the cash earned in a day
    /// </summary>
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
    /// <summary>
    /// Find an exit for the NPC to head towards
    /// </summary>
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
    /// <summary>
    /// Find the store room for thieves to head to
    /// </summary>
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
    /// <summary>
    /// Remove an interactable object from the player's lists to prevent errors
    /// </summary>
    public void RemoveInteractableObject(GameObject obj)
    {
        foreach (StorePlayer playa in players)
        {
            if (playa.myInteractableObjects.Contains(obj))
                playa.RemoveInteractableObject(obj);
        }
            
    }
    /// <summary>
    /// Set up a small text to pop up on a player's screen
    /// </summary>
    public void PlayPopUpText(string text,bool isPlayer2=false)
    {
        if(!isPlayer2)
        {
            playerTextPopUps[0].text = text;
            playerTextPopUps[0].gameObject.SetActive(true);
        }
        else
        {
            playerTextPopUps[1].text = text;
            playerTextPopUps[1].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Find a random pedestal for targeting
    /// </summary>
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
    /// <summary>
    /// Find a random bargin bin for targeting
    /// </summary>
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
    /// <summary>
    /// Send an NPC to the register to cash out
    /// </summary>
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
    /// <summary>
    /// Open the shops and start the game if no one is in moving mode
    /// </summary>
    public void OpenShop()
    {
        if(players[0].isInMovingMode)
        {
            foreach (ShopDoor door_ in mydoors)
            {
                door_.ResetDoor();
            }
            PlayPopUpText("Exit Moving Mode first");
            PlayPopUpText("Exit Moving Mode first",true);
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
        exitDetector.OpenShop();
        MMSoundManager.Instance.PlaySound(openShopAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, 1, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);

    }
    /// <summary>
    /// Put an item back to the players inventory 
    /// </summary>
    public void ReturnItemToInventory(ItemData item_,int amount)
    {
        if(amount>0)
        {
            invScreen.AddItemToInventory(item_, amount);
        }
    }
    /// <summary>
    /// Set the UI indicators to show you are being robbed
    /// </summary>
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
    public void EnterHell(GameObject obj)
    {
        if (!twoPlayerMode)
        {
            playerInHell = true;
            foreach (Thief t_ in currentThieves)
            {
                t_.CheckSpeed();
            }
        }
        foreach (ParticleSystem sys in teleportEffectsHell)
        {
            sys.Play();
        }
        MMSoundManager.Instance.PlaySound(enterHellAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, 1, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    public void ExitHell(GameObject obj)
    {
        if (!twoPlayerMode)
        {

            playerInHell = false;
            foreach (Thief t_ in currentThieves)
            {
                t_.CheckSpeed();
            }
        }
        foreach(ParticleSystem sys in teleportEffectsHuman)
        {
            sys.Play();
        }
        MMSoundManager.Instance.PlaySound(enterHellAudio, MMSoundManager.MMSoundManagerTracks.Sfx, transform.position,
   false, 1.0f, 0, false, 0, 1, null, false, null, null, 1, 0, 0.0f, false, false, false, false, false, false, 128, 1f,
   1f, 0, AudioRolloffMode.Logarithmic, 1f, 500f, false, 0f, 0f, null, false, null, false, null, false, null, false, null);
    }
    /// <summary>
    /// Save the layout and contents of all items
    /// </summary>
    public void DebugSaveItems()
    {
        PlayerInventory.instance.UpdateItems(invScreen.slots);
        PlayerInventory.instance.UpdateMoveableItems(moveableObjectScreen.invUI.slots);
        PlayerInventory.instance.SaveItems();
        SaveAllPedestals();
        SaveAllBarginBins();
        
    }
    //Used in pause screen 
    public void SaveEverything()
    {
        if (!ShopTutorialManager.instance.inTut)
        {
            MoveableObjectManager.instance.SaveAllSlots();
            SetPedestalList();
            SetBarginBinList();
            DebugSaveItems();
        }
    }
    /// <summary>
    /// Add some items for testings
    /// </summary>
    public void DebugAddItems()
    {
        foreach(InventoryItem item_ in debugItemsToAdd)
        invScreen.AddItemToInventory(PlayerInventory.instance.GetItem(item_.myItemName), item_.amount);
    }
    /// <summary>
    /// Add some items for testings
    /// </summary>
    public void DebugAddItems2()
    {
        foreach (InventoryItem item_ in debugItemsToAdd2)
            invScreen.AddItemToInventory(PlayerInventory.instance.GetItem(item_.myItemName), item_.amount);
    }
    /// <summary>
    /// Grab all the pedestals from the moveable object list and set up our lists
    /// </summary>
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
    /// <summary>
    /// Grab all the bins from the moveable object list and set up our lists
    /// </summary>
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
    /// <summary>
    /// Save all the pedestal's contents
    /// </summary>
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
    /// <summary>
    /// Load all the pedestals previous items
    /// </summary>
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
    /// <summary>
    /// GSave all the bargain bin contents
    /// </summary>
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
    /// <summary>
    /// Load all the bargain bins previous contents
    /// </summary>
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
    /// <summary>
    /// Any events that need to happen when closing the shop
    /// </summary>
    public void EndShopEvent()
    {
        foreach (ShopDoor door_ in mydoors)
        {
            door_.ResetDoor();
        }
    }
    /// <summary>
    /// Set the shop as Open or closed
    /// </summary>
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
    /// <summary>
    /// Close the shop
    /// </summary>
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
            door_.ResetDoor();
        }
        exitDetector.CloseShop();
        CustomerManager.instance.CloseShop();
    }
    /// <summary>
    /// Check if a shop is open
    /// </summary>
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
    /// <summary>
    /// Warp a player to the shop they are currently not in
    /// </summary>
    public void WarpPlayerToOtherStore(GameObject obj)
    {
        if(obj.GetComponent<StorePlayer>().inHell)
        {
            obj.GetComponent<StorePlayer>().inHell = false;
            ExitHell(obj);
            obj.transform.position = teleportLocationHuman.position;
        }
        else
        {
            obj.GetComponent<StorePlayer>().inHell = true;
            EnterHell(obj);
            obj.transform.position = teleportLocationHell.position;
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
    /// <summary>
    /// Play a particular audio for the UI
    /// </summary>
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
    /// <summary>
    /// Enter or exit moving mode
    /// </summary>
    public void ToggleMoveMode()
    {
        if (!shopRunning)
        {
            foreach(StorePlayer playa in players)
                playa.ToggleMoveMode();
        }
    }
    public void RedoNavMesh()
    {
        surface.BuildNavMesh();
    }
    /// <summary>
    /// Wait to redo the navmesh to allow objects to be enabled
    /// </summary>
    IEnumerator WaitAFrameBeforeRedoingNavmesh()
    {
        yield return new WaitForSeconds(0.001f);
        RedoNavMesh();
    }
    /// <summary>
    /// Return the object held by player 1
    /// </summary>
    public MoveableObject GetPlayerHeldMoveableItem()
    {
       return players[0].GetHeldObject();
    }
    /// <summary>
    /// Set the object held by player 1
    /// </summary>
    public void SetPlayerHeldMoveableItem(MoveableObject obj_)
    {
        players[0].SetHeldObject(obj_);
    }
    /// <summary>
    /// Check if an item is hot or cold. Hot items sell for more, cold for less.     //1=hot, 2=cold 0= neutral;
    /// </summary>
    public int CheckIfItemIsHot(ItemData itemToCheck,bool inHell=false)
    {
        if(ShopTutorialManager.instance.inTut)
        {
            if(ShopTutorialManager.instance.hotItem.itemName==itemToCheck.itemName)
            {
                return 1;
            }
            else if(ShopTutorialManager.instance.coldItem.itemName == itemToCheck.itemName)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
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
    /// <summary>
    /// Try to leave the shop and head to town, only works when shop is closed
    /// </summary>
    public void TryToLeaveShop()
    {
        if (!PlayerIsNearExit)
            return;
        if (shopRunning)
            return;
        if (isLeaving)
            return;
        isLeaving = true;
        townLevelLoader.LoadMyLevel();
    }
    private void CalculateHotItems()
    {
        hotItems.Clear();
        coldItems.Clear();
        hotItemsHell.Clear();
        coldItemsHell.Clear();

        hotItems.AddRange(hotItemsUniversal);
        hotItems.AddRange(hotItemsWeekly);

        coldItems.AddRange(coldItemsUniversal);
        coldItems.AddRange(coldItemsWeekly);

        hotItemsHell.AddRange(hotItemsHellUniversal);
        hotItemsHell.AddRange(hotItemsHellWeekly);

        coldItemsHell.AddRange(coldItemsHellUniversal);
        coldItemsHell.AddRange(coldItemsHellWeekly);
    }
    public float GetColdItemMultiplier()
    {
        return 0.6f;
    }
    public float GetHotItemMultiplier()
    {
        return 1.3f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
