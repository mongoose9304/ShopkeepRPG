using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using MoreMountains.Feedbacks;

public class TreeManager : MonoBehaviour
{
    public static TreeManager instance;
    int currentCombo;
    public MMF_Player[] comboFeedBacks;
    public TextMeshProUGUI currentComboText;
    public GameObject comboObject;

    public MMF_Player[] comboFeedBacksTwo;
    public TextMeshProUGUI currentComboTextTwo;
    public GameObject comboObjectTwo;
    private void Start()
    {
        instance = this;
        currentComboText.text = currentCombo.ToString();
        currentComboTextTwo.text = currentCombo.ToString();
        comboObject.SetActive(false);
        comboObjectTwo.SetActive(false);
    }
    public void SetCombo(int combo, bool isPlayer2 = false)
    {
        if (!isPlayer2)
        {
            currentCombo = combo;
            currentComboText.text = currentCombo.ToString();
            PlayComboAnimation();
            comboObject.SetActive(true);
        }
        else
        {
            currentCombo = combo;
            currentComboTextTwo.text = currentCombo.ToString();
            PlayComboAnimation(true);
            comboObjectTwo.SetActive(true);
        }
    }
    public void ResetCombo(bool isPlayer2=false)
    {
        if (!isPlayer2)
        {
            currentCombo = 0;
            currentComboText.text = currentCombo.ToString();
            comboObject.SetActive(false);
        }
        else
        {
            currentCombo = 0;
            currentComboTextTwo.text = currentCombo.ToString();
            comboObjectTwo.SetActive(false);
        }
    }
    private void PlayComboAnimation(bool isPlayer2=false)
    {
        if (!isPlayer2)
        {
            foreach (MMF_Player player_ in comboFeedBacks)
            {
                player_.PlayFeedbacks();
            }
        }
        else
        {
            foreach (MMF_Player player_ in comboFeedBacksTwo)
            {
                player_.PlayFeedbacks();
            }
        }

    }
}
