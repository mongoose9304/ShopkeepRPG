using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct PopUp
{
    public string title;
    public string bodyText;
}
public class TextPopUpManager : MonoBehaviour
{
    public static TextPopUpManager instance;
    public List<PopUp> waitingPopUps = new List<PopUp>();
    [SerializeField] float maxTimeForPopUp;
    [SerializeField] float currentTimeForPopUp;
    [SerializeField] TextMeshProUGUI bodyText;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] GameObject popUpObject;
    private void Awake()
    {
        instance = this; 
    }
    public void AddText(string title_,string body_)
    {
        PopUp p = new PopUp();
        p.title = title_;
        p.bodyText = body_;
        if (waitingPopUps.Count == 0&& !popUpObject.activeInHierarchy)
        {
            popUpObject.SetActive(true);
            bodyText.text = title_;
            titleText.text = body_;
            currentTimeForPopUp = maxTimeForPopUp;
        }
        else
        {
            waitingPopUps.Add(p);
        }
    }
    private void Update()
    {
        if(popUpObject.activeInHierarchy)
        {
            currentTimeForPopUp -= Time.deltaTime;
            if(currentTimeForPopUp<=0)
            {
                DisplayWaitingText();
                currentTimeForPopUp = maxTimeForPopUp;
            }
        }

    }
    private void DisplayWaitingText()
    {
        if (waitingPopUps.Count == 0)
        {
            popUpObject.SetActive(false);
            return;
        }
        bodyText.text = waitingPopUps[0].bodyText;
        titleText.text = waitingPopUps[0].title;
        waitingPopUps.RemoveAt(0);
        
    }
}
