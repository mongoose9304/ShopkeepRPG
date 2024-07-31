using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the tutorial and its possible states 
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Tooltip("The singleton instance")]
    public static TutorialManager instance_;
    int tutorialState;
    [Tooltip("All the messages for the tutorial in order of appearance")]
    [SerializeField] string[] tutorialMessages;
    [Tooltip("REFERNCE to the controller of the UI for the tutorial")]
    [SerializeField] TutorialUIManager tutUIManager;
    private void Awake()
    {
        instance_ = this;
    }
    /// <summary>
    /// Set the tutorial to a new state and each state can have its own seperate logic depending on what's needed
    /// </summary>
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
    private void OnDisable()
    {
        tutUIManager.gameObject.SetActive(false);
    }
}
