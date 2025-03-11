
using UnityEngine;

namespace Shopkeeper.Menus
{
    public class MainMenu_buttons : MonoBehaviour
    {
        MenuManager menuManager;
        public MenuBase settingsMenu;
        private void Start()
        {
            menuManager = MenuManager.instance;
            
        }
        public void BtnStart()
        {
            ManagerUniversal.GetManager<UniversalSaveManager>().New();
        }
        public void BtnLoad()
        {
            Debug.Log("Load button!");
        }
        public void BtnGuide()
        {
            Debug.Log("Guide button!");
        }
        public void BtnSettings()
        {
            Debug.Log("Settings button clicked!");
            Debug.Log("Settings menu prefab: " + (settingsMenu.prefab_ != null ? settingsMenu.prefab_.name : "NULL"));
            //Debug.Log("Menu parent: " + (menuManager.menuParent != null ? menuManager.menuParent.name : "NULL"));
            MenuManager.instance.OpenMenu(settingsMenu);
            Debug.Log("OpenMenu called!");
            //menuManager.OpenMenu(settingsMenu);
            //Debug.Log("Settings button!");
        }
        public void BtnLanguage()
        {
            Debug.Log("language button!");
        }
        public void BtnQuit()
        {
            menuManager.CloseMenu();
            Application.Quit();
        }
    }
}