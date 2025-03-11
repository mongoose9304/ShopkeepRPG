using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper.Menus
{
    public class SettingsMenu_buttons : MonoBehaviour
    {
    
        public void BtnAudio()
        {
            Debug.Log("Audio settings!");
        }
        public void BtnGraphics()
        {
            Debug.Log("Graphics settings");
        }
        public void BtnControls()
        {
            Debug.Log("Controls settings");
        }
        public void BtnCredits()
        {
            Debug.Log("Credits");
        }

        public void BtnQuit()
        {
            Debug.Log("Back to menu");
            Application.Quit();
        }
    }
}
