using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorialManager : TutorialManager
{
    public float tutAFKTimeMax;
    public int[] tutNonAFKStates;
    [SerializeField] float tutAFKTimeCurrent;
    private void Update()
    {
        foreach(int x in tutNonAFKStates)
        {
            if (tutorialState == x)
                return;
        }
        tutAFKTimeCurrent -= Time.deltaTime;
        if(tutAFKTimeCurrent<=0)
        {
            tutAFKTimeCurrent = tutAFKTimeMax;
            SetAltTutorialState(tutorialState);
        }
    }
    /// <summary>
    /// Set the tutorial to a new state and each state can have its own seperate logic depending on what's needed
    /// </summary>
    public override void SetTutorialState(int tutorialState_)
    {
        tutorialState = tutorialState_;
        tutAFKTimeCurrent = tutAFKTimeMax;
        switch (tutorialState_)
        {
            case 0:
                tutUIManager.SetJoystickMessage(tutorialMessages[tutorialState]);
                CombatPlayerManager.instance.EnableFamiliars(false);
                tutUIManager.ChangeSpeakerEmotion(0);
                break;
            case 1:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 1, true);
                tutUIManager.ChangeSpeakerEmotion(0);
                break;
            case 2:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 0, true);
                tutUIManager.ChangeSpeakerEmotion(0);
                break;
            case 3:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                tutUIManager.ChangeSpeakerEmotion(1);
                break;
            case 4:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 2, true);
                tutUIManager.ChangeSpeakerEmotion(0);
                break;
            case 5:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 4, true);
                tutUIManager.ChangeSpeakerEmotion(0);
                break;
            case 6:
                tutUIManager.SetMessage(tutorialMessages[tutorialState], 6, true);
                tutUIManager.ChangeSpeakerEmotion(0);
                CombatPlayerManager.instance.EnableFamiliars(true);
                CombatPlayerManager.instance.ReturnFamiliars();
                break;
            case 7:
                tutUIManager.SetMessage(tutorialMessages[tutorialState]);
                tutUIManager.ChangeSpeakerEmotion(1);
                break;

        }
    }
    /// <summary>
    /// Set the tutorial to a new state and each state can have its own seperate logic depending on what's needed
    /// </summary>
    public override void SetAltTutorialState(int tutorialState_)
    {
        tutorialState = tutorialState_;
        switch (tutorialState_)
        {
            case 0:
                tutUIManager.SetJoystickMessage(tutorialAlternateMessages[tutorialState]);
                tutUIManager.ChangeSpeakerEmotion(2);
                break;
            case 1:
                tutUIManager.SetMessage(tutorialAlternateMessages[tutorialState], 1, true);
                tutUIManager.ChangeSpeakerEmotion(3);
                break;
            case 2:
                tutUIManager.SetMessage(tutorialAlternateMessages[tutorialState], 0, true);
                tutUIManager.ChangeSpeakerEmotion(2);
                break;
            case 3:
                 return;
                break;
            case 4:
                tutUIManager.SetMessage(tutorialAlternateMessages[tutorialState], 2, true);
                tutUIManager.ChangeSpeakerEmotion(2);
                break;
            case 5:
                tutUIManager.SetMessage(tutorialAlternateMessages[tutorialState], 4, true);
                tutUIManager.ChangeSpeakerEmotion(3);
                break;
            case 6:
                return;
                break;
            case 7:
                tutUIManager.SetMessage(tutorialAlternateMessages[tutorialState]);
                tutUIManager.ChangeSpeakerEmotion(3);
                break;

        }
    }
    public override void EndTutorial()
    {
        foreach (GameObject obj in objectsToDisableDuringTut)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisablePostTut)
        {
            obj.SetActive(false);
        }
        CombatPlayerManager.instance.EnableFamiliars(true);
        inTut = false;
    }
    public void FailedDash()
    {
        if(tutorialState==1)
        {
            SetAltTutorialState(1);
        }
    }
}
