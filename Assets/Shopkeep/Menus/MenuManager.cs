using Shopkeeper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance { get; private set; }
    //Menu stack
    public Stack<GameObject> menuStack = new Stack<GameObject>();
    public GameObject topMenu;
    public GameObject bottomMenu;
    public Transform menuParent; //canvas

    //will use it a sort order in Canvas section
    public float stackCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //ADD STACK COUNT FOR THESORT ORDER
       
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        menuStack.Push(topMenu);
    }

    public void OpenMenu(MenuBase menu) {
        if (menu.prefab_ == null) {
            return;
        }


       
        if (menuStack.Count > 0)
        {
            bottomMenu = menuStack.Peek(); // Get the previous top menu
            bottomMenu.SetActive(false);
        }
        else {
            bottomMenu = null;
        }


        GameObject newMenu = Instantiate(menu.prefab_);
        menuStack.Push(newMenu);
        topMenu = newMenu;
    }

    public void CloseMenu() {
        if (menuStack.Count > 0)
        {
            topMenu = menuStack.Pop();
            Destroy(topMenu);


            if (menuStack.Count > 0)
            {
                topMenu = menuStack.Peek();
                topMenu.SetActive(true);
            }
            else
            {
                topMenu = null; // No menus left
            }

        }
    }
}
