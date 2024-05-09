using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PageHolder{
    public GameObject page;
}

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> pages;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabSelected;
    public TabButton selectedTab;

    public void ResetTabs(){
        foreach (TabButton button in tabButtons){
            if (selectedTab != null && button == selectedTab){
                continue;
            }
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
        if (selectedTab == null || tabButton != selectedTab){
            tabButton.background.sprite = tabHover;
        }
    }
    
    public void OnTabExit(TabButton tabButton){
        ResetTabs();
    }

    public void OnTabSelected(TabButton tabButton){
        selectedTab = tabButton;
        ResetTabs();
        tabButton.background.sprite = tabSelected;
        int index = tabButton.transform.GetSiblingIndex();
        for(int i = 0; i < pages.Count; i++) {
            if (i == index){
                pages[i].SetActive(true);
            }
            else {
                pages[i].SetActive(false);
            }
        }
    }
}
