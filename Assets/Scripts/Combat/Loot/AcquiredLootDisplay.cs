using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine.UI;

public class AcquiredLootDisplay : MonoBehaviour
{
    MMF_TMPCountTo inventoryCounter;
    MMF_TMPCountTo acquiredCounter;
    [SerializeField] MMF_Player inventoryFeedback;
    [SerializeField] MMF_Player acquiredFeedback;
    [SerializeField] Image resourceImage;
    private void Awake()
    {
        inventoryCounter = inventoryFeedback.GetFeedbackOfType<MMF_TMPCountTo>();
        acquiredCounter = acquiredFeedback.GetFeedbackOfType<MMF_TMPCountTo>();
        
    }

    public void StartCountEffect(int inventoryStart,int acquiredStart,Sprite resourceSprite,float duration_,bool lost_=false)
    {
        resourceImage.sprite = resourceSprite;
        inventoryCounter.Duration = duration_;
        inventoryCounter.CountFrom = inventoryStart;
        inventoryCounter.CountTo = inventoryStart+ acquiredStart;
        inventoryFeedback.GetComponent<TextMeshProUGUI>().text = inventoryCounter.CountFrom.ToString();
        acquiredCounter.CountFrom = acquiredStart;
        acquiredCounter.CountTo = 0;
        acquiredFeedback.GetComponent<TextMeshProUGUI>().text = inventoryCounter.CountFrom.ToString();
        acquiredCounter.Duration = duration_;
        if (!lost_)
        inventoryFeedback.PlayFeedbacks();
        acquiredFeedback.PlayFeedbacks();
    }

}
