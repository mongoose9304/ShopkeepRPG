using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Menus
{
    public class SettingsMenu_buttons : MonoBehaviour
    {
        MenuManager menuManager;
        public MenuBase audioMenu;
        public MenuBase creditsMenu;
        public MenuBase controlsMenu;
        public MenuBase graphicsMenu;

        private void Start()
        {
            menuManager = MenuManager.instance;

        }
        public void BtnAudio()
        {
            MenuManager.instance.OpenMenu(audioMenu);
            Debug.Log("Audio settings!");
        }
        public void BtnGraphics()
        {
            
            MenuManager.instance.OpenMenu(graphicsMenu);
            Debug.Log("Graphics settings");
        }
        public void BtnControls()
        {
            MenuManager.instance.OpenMenu(controlsMenu);
            Debug.Log("Controls settings");
        }
        public void BtnCredits()
        {
            MenuManager.instance.OpenMenu(creditsMenu);
            Debug.Log("Credits");
        }

        public void BtnQuit()
        {
            menuManager.CloseMenu();
            Application.Quit();
            Debug.Log("Back to menu");
        }
    }
}
