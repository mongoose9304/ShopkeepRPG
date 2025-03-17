using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu_buttons : MonoBehaviour
{
    MenuManager menuManager;
    public MenuBase settingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = MenuManager.instance;
    }

    public void Btn_Back() {
        menuManager.CloseMenu();
        Application.Quit();
        Debug.Log("Leaving the Audio Menu");
    }
}
