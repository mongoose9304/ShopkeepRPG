using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    
    public void SubToList(TabButton tabButton)
    {
        if (tabButtons == null){
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(tabButton);
    }

    public void OnTabEnter(TabButton tabButton){

    }
    
    public void OnTabExit(TabButton tabButton){

    }

    public void OnTabSelected(TabButton tabButton){

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
