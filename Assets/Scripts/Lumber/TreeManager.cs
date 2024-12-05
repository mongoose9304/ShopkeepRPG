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
    private void Start()
    {
        instance = this;
        currentComboText.text = currentCombo.ToString();
        comboObject.SetActive(false);
    }
    public void SetCombo(int combo)
    {
        currentCombo = combo;
        currentComboText.text = currentCombo.ToString();
        PlayComboAnimation();
        comboObject.SetActive(true);
    }
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    public void ResetCombo()
    {
        currentCombo = 0;
        currentComboText.text = currentCombo.ToString();
        comboObject.SetActive(false);
    }
    private void PlayComboAnimation()
    {
        foreach (MMF_Player player_ in comboFeedBacks)
        {
            player_.PlayFeedbacks();
        }
    }
}
