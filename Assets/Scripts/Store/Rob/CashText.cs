using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CashText : MonoBehaviour
{
    MMF_TMPCountTo cashCounter;
    [SerializeField] MMF_Player cashFeedback;
    public TextMeshProUGUI cashText;
    private void Awake()
    {
        if (cashFeedback)
            cashCounter = cashFeedback.GetFeedbackOfType<MMF_TMPCountTo>();
    }
    private void OnEnable()
    {
        if(cashCounter.CountTo!=float.Parse(cashText.text))
        cashFeedback.PlayFeedbacks();
    }
}
