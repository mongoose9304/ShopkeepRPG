using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    public Color tabSelected;

    public void ResetTabs(){
        foreach (TabButton button in tabButtons){
            button.background.color = tabIdle;
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
        tabButton.background.color = tabHover;
    }
    
    public void OnTabExit(TabButton tabButton){
        ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton){
        ResetTabs();
        tabButton.background.color = tabSelected;
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
