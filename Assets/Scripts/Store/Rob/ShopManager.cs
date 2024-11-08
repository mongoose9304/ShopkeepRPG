using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public bool playerInHell;
    public bool hellShopEnabled;
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
    public CashRegister cashRegister;
    public CashRegister cashRegisterHell;
    public List<Thief> currentThieves=new List<Thief>();
    public List<InventoryItem> debugItemsToAdd=new List<InventoryItem>();
    [SerializeField] private List<Pedestal> allPedestals = new List<Pedestal>();
    [SerializeField] private List<BarginBin> allBarginBins = new List<BarginBin>();
    private void Awake()
    {
        instance = this;
        InitPedestalList();
        InitBarginBinList();
        LoadAllPedestals();
        LoadAllBarginBins();
        if (cashFeedback)
            cashCounter = cashFeedback.GetFeedbackOfType<MMF_TMPCountTo>();

        if (cashFeedbackHell)
            cashCounterHell = cashFeedbackHell.GetFeedbackOfType<MMF_TMPCountTo>();
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
    public void CloseMenu()
    {
        inMenu = false;
        pedScreen.gameObject.SetActive(false);
        barginScreen.gameObject.SetActive(false);
        barginScreen.CloseMenu();
        haggleScreen.gameObject.SetActive(false);
        haggleScreen.CloseMenu();
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
    public GameObject GetRandomTargetBarginBin( bool inHell = false)
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
        CustomerManager.instance.OpenShop(8,4);
        CustomerManager.instance.OpenShop(8,0,true);
        foreach(ShopDoor door_ in mydoors)
        {
            door_.RotateDoor();
        }
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
            }
            else
            {
                cashSymbolHell.gameObject.SetActive(false);
                stealAlertHell.gameObject.SetActive(true);
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
    }
    public void ExitHell()
    {
        playerInHell = false;
        foreach (Thief t_ in currentThieves)
        {
            t_.CheckSpeed();
        }
    }
    public void DebugSaveItems()
    {
        PlayerInventory.instance.UpdateItems(invScreen.slots);
        PlayerInventory.instance.SaveItems();
        SaveAllPedestals();
        SaveAllBarginBins();
        
    }
    public void DebugAddItems()
    {
        foreach(InventoryItem item_ in debugItemsToAdd)
        invScreen.AddItemToInventory(item_.myItem, item_.amount);
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
                
                item_.myItem = allPedestals[i].myItem;
                item_.amount = allPedestals[i].amount;


                masterItemList_.Add(item_);
            }
            else
            {
                item_.myItem = null;
                item_.amount = 0;


                masterItemList_.Add(item_);
            }
            if (allPedestals[i].myItemPrevious)
            {

                item_.myItem = allPedestals[i].myItemPrevious;
                item_.amount = allPedestals[i].amountPrevious;


                masterPreviousItemList_.Add(item_);
            }
            else
            {
                item_.myItem = null;
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
                if(masterItemList_[i].myItem)
                {
                    allPedestals[i].SetItem(masterItemList_[i].myItem, masterItemList_[i].amount);
                }
            }
            for (int i = 0; i < masterItemListPrevious_.Count; i++)
            {
                if (masterItemListPrevious_[i].myItem)
                {
                    allPedestals[i].SetPreviousItem(masterItemListPrevious_[i].myItem, masterItemListPrevious_[i].amount);
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

                    item_.myItem = allBarginBins[i].binSlots[x].myItem;
                    item_.amount = allBarginBins[i].binSlots[x].amount;


                    
                }
                else
                {
                    item_.myItem = null;
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

                    item_.myItem = allBarginBins[i].binSlotsPrevious[x].myItem;
                    item_.amount = allBarginBins[i].binSlotsPrevious[x].amount;



                }
                else
                {
                    item_.myItem = null;
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
                    if(masterItemList_[i].myList[x].myItem)
                    allBarginBins[i].SetSlot(x, masterItemList_[i].myList[x].myItem, masterItemList_[i].myList[x].amount);
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
                    if (masterItemListPrevious_[i].myList[x].myItem)
                        allBarginBins[i].SetPreviousSlot(x, masterItemListPrevious_[i].myList[x].myItem, masterItemListPrevious_[i].myList[x].amount);
                }
            }
        }
    }
}
