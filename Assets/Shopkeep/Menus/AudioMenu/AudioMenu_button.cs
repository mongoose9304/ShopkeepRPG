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

        public MenuBase settingsMenu;

        //Volume sliders:
        public Slider slider_masterVolume;
        public Slider slider_musicVolume;
        public Slider slider_SFXVolume;
        private void Start()
        {
            LoadAudioSettings();
            menuManager = MenuManager.instance;
            devices_dropDown.onValueChanged.AddListener(BtnDevicesDropDown);

            //Setting up the sliders
            slider_masterVolume.onValueChanged.AddListener(SlMasterVolume);
            slider_musicVolume.onValueChanged.AddListener (SlMusicVolume);
            slider_SFXVolume.onValueChanged.AddListener(SlSFXVolume);

        }

        public void BtnDevicesDropDown(int index) { 
            string selectedOption = devices_dropDown.options[index].text;
            Debug.Log("Selected: " + selectedOption);
        }

        public void SlMasterVolume(float value) {

            Debug.Log("Master Volume: " + value);
        }
        public void SlMusicVolume(float value)
        {
            Debug.Log("Music Volume: " + value);
        }

        public void SlSFXVolume(float value)
        {
            Debug.Log("SFX Volume: " + value);
        }

        public void BtnBack() {
            SaveAudioSettings();
            menuManager.CloseMenu();
            Application.Quit();
            Debug.Log("Leaving the Audio Menu");
        }


        //Safe and Load sliders bars values
        public void SaveAudioSettings() {
            PlayerPrefs.SetFloat( "MasterVolume", slider_masterVolume.value);
            PlayerPrefs.SetFloat( "MusicVolume", slider_musicVolume.value);
            PlayerPrefs.SetFloat( "SFXVolume", slider_SFXVolume.value);
        }

        private void LoadAudioSettings()
        {
            slider_masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
            slider_musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            slider_SFXVolume.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        }
    }
}