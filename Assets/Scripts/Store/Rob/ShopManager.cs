using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;
    public bool hellShopEnabled;
    public TextMeshProUGUI cashEarnedText;
    public TextMeshProUGUI cashEarnedTextHell;
    public StorePlayer player;
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
    private void Awake()
    {
        instance = this;
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
}
