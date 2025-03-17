using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class ControlsMenu_buttons : MonoBehaviour
{
    MenuManager menuManager;
    public MenuBase settingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = MenuManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RemapButtonClicked(InputAction action) {
        Debug.Log(action.GetBindingDisplayString());
        var rebindoperation = action.PerformInteractiveRebinding().Start();
        Debug.Log(action.GetBindingDisplayString());
    }

    public void Btn_Back()
    {
        menuManager.CloseMenu();
        Application.Quit();
    }
}
