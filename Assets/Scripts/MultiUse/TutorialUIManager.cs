using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] GameObject joyStickButton;
    [SerializeField] GameObject wasdButton;
    [SerializeField] GameObject gamepadButton;
    //Left,Right
   [SerializeField] Sprite[] joyStickTypes;
    //A,B,X,Y,LB,RB,LT,RT
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
    public void SetJoystickMessage(string text = "",int joystickType=0 ,bool useWASD = false)
    {
        ResetTutorialMessage();
        if (useWASD)
        {
            wasdButton.SetActive(true);
        }
        else
        {
            joyStickButton.GetComponent<Image>().sprite = gamePadButtons[joystickType];
            joyStickButton.SetActive(true);
        }
        tutorialText.text = text;
    }

}
