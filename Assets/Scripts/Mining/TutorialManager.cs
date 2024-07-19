using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance_;
    int tutorialState;
    [SerializeField] string[] tutorialMessages;
    [SerializeField] TutorialUIManager tutUIManager;
    private void Awake()
    {
        instance_ = this;
    }
    public void SetTutorialState(int tutorialState_)
    {
        tutorialState = tutorialState_;
        switch (tutorialState_)
        {
            case 0:
                tutUIManager.SetJoystickMessage(tutorialMessages[tutorialState]);
                break;
            case 1:
                tutUIManager.SetMessage(tutorialMessages[tutorialState],0,true);
                break;
            case 2:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 2, true);
                break;
            case 3:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                break;
            case 4:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                break;
            case 5:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 1, true);
                break;
            case 6:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 3, true);
                break;
            case 7:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                break;

        }
    }
}
