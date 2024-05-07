using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;

    public void ResetTabs(){
        foreach (TabButton button in tabButtons){
            button.background.sprite = tabIdle;
        }
    }
    
    public void SubToList(TabButton tabButton)
    {
        if (tabButtons == null){
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(tabButton);
    }

    public void OnTabEnter(TabButton tabButton){
        ResetTabs();
        tabButton.background.sprite = tabHover;
    }
    
    public void OnTabExit(TabButton tabButton){
        ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton){
        ResetTabs();
        tabButton.background.sprite = tabSelected;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
