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

    public Transform menuParent; //canvas

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenMenu(MenuBase menu) {
        if (menu.prefab_ == null) {
            return;
        }
     
        if (menuStack.Count > 0)
        {
            CloseMenu();
        }
        GameObject newMenu = Instantiate(menu.prefab_);
        menuStack.Push(newMenu);
    }

    public void CloseMenu() {
        if (menuStack.Count > 0)
        {
            GameObject topMenu = menuStack.Pop();
            Destroy(topMenu);
        }
    }
}
