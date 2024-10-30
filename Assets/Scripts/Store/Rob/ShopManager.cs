using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Feedbacks;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI cashEarnedText;
    public StorePlayer player;
    public GameObject[] exitSpots;
    public int currentCashEarned;
    public static ShopManager instance;
    public PedestalScreen pedScreen;
    public InventoryUI invScreen;
    public BarginBinScreen barginScreen;
    public HaggleUI haggleScreen;
    public bool inMenu;
    MMF_TMPCountTo cashCounter;
    [SerializeField] MMF_Player cashFeedback;
    private void Awake()
    {
        instance = this;
        if (cashFeedback)
            cashCounter = cashFeedback.GetFeedbackOfType<MMF_TMPCountTo>();
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
        haggleScreen.gameObject.SetActive(false);
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
    }
    public void AddCash(int cash)
    {
        cashCounter.CountFrom = currentCashEarned;
        currentCashEarned += cash;
        cashCounter.CountTo = currentCashEarned;
        cashFeedback.PlayFeedbacks();
    }
    public GameObject GetRandomNPCExit()
    {
        return exitSpots[Random.Range(0, exitSpots.Length)];
    }
    public void RemoveInteractableObject(GameObject obj)
    {
        if (player.myInteractableObjects.Contains(obj))
            player.RemoveInteractableObject(obj);
    }
}
