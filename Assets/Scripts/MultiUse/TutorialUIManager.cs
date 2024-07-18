using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    TextMeshProUGUI tutorialText;
    GameObject joyStickButton;
    GameObject wasdButton;
    GameObject gamepadButton;
   [SerializeField] Sprite[] joyStickTypes;
   [SerializeField] Sprite[] gamePadButtons;

    public void ResetTutorialMessage()
    {
        tutorialText.text = "";
        wasdButton.SetActive(false);
        gamepadButton.SetActive(false);
        joyStickButton.SetActive(false);

    }
    public void SetMessage(string text = "", int gamePadButton_ = 0,bool useButton=false)
    {
        ResetTutorialMessage();
        if(useButton)
        {
            gamepadButton.GetComponent<Image>().sprite = gamePadButtons[gamePadButton_];
        }
        tutorialText.text = text;
    }
    public void SetJoystickMessage(string text = "", bool useWASD = false)
    {
        ResetTutorialMessage();
        if (useWASD)
        {
            wasdButton.SetActive(true);
        }
        else
        {
            joyStickButton.SetActive(true);
        }
        tutorialText.text = text;
    }

}
