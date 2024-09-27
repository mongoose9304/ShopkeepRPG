using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorialManager : TutorialManager
{
    public GameObject[] objectsToDisableDuringTut;

    public void StartTutorial()
    {
        foreach(GameObject obj in objectsToDisableDuringTut)
        {
            obj.SetActive(false);
        }
    }
    public void EndTutorial()
    {
        foreach (GameObject obj in objectsToDisableDuringTut)
        {
            obj.SetActive(true);
        }
    }
    /// <summary>
    /// Set the tutorial to a new state and each state can have its own seperate logic depending on what's needed
    /// </summary>
    public override void SetTutorialState(int tutorialState_)
    {
        tutorialState = tutorialState_;
        switch (tutorialState_)
        {
            case 0:
                tutUIManager.SetJoystickMessage(tutorialMessages[tutorialState]);
                break;
            case 1:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
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