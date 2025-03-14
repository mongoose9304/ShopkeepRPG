using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace Shopkeeper.Menus
{
    public class AudioMenu_button : MonoBehaviour {

        MenuManager menuManager;
        public TMP_Dropdown devices_dropDown;
        public Slider slider_audioVolume;
        public MenuBase settingsMenu;

    
        private void Start()
        {
            menuManager = MenuManager.instance;
            devices_dropDown.onValueChanged.AddListener(BtnDevicesDropDown);
            slider_audioVolume.onValueChanged.AddListener(SlAudioVolume);
        }
        public void BtnStart()
        {
            ManagerUniversal.GetManager<UniversalSaveManager>().New();
        }

        public void BtnDevicesDropDown(int index) { 
            string selectedOption = devices_dropDown.options[index].text;
            Debug.Log("Selected: " + selectedOption);
        }

        public void SlAudioVolume(float value) {
            Debug.Log("Audio Volume: " + value);
        }

        public void BtnBack() {
            menuManager.CloseMenu();
            Debug.Log("Leaving the Audio Menu");
        }
    }
}