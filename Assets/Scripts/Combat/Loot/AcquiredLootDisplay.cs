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

    public void StartCountEffect(int inventoryStart,int acquiredStart,Sprite resourceSprite)
    {
        resourceImage.sprite = resourceSprite;
        inventoryCounter.CountFrom = inventoryStart;
        inventoryCounter.CountTo = inventoryStart+ acquiredStart;
        inventoryFeedback.GetComponent<TextMeshProUGUI>().text = inventoryCounter.CountFrom.ToString();
        acquiredCounter.CountFrom = acquiredStart;
        acquiredCounter.CountTo = 0;
        inventoryFeedback.GetComponent<TextMeshProUGUI>().text = inventoryCounter.CountTo.ToString();
        inventoryFeedback.PlayFeedbacks();
        acquiredFeedback.PlayFeedbacks();
    }

}
