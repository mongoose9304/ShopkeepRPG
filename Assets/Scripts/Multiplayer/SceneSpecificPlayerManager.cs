using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpecificPlayerManager : MonoBehaviour
{
    public static SceneSpecificPlayerManager instance;
    public List<GameObject> objectsToDisableWhenPlayer1Joins = new List<GameObject>();
    public List<Canvas> player1Canvases = new List<Canvas>();
    private void Awake()
    {
        instance = this;
    }
    public virtual void CreatePlayer1(PlayerController controller)
    {
        foreach(GameObject obj in objectsToDisableWhenPlayer1Joins)
        {
            obj.SetActive(false);
        }
        foreach(Canvas canv in player1Canvases)
        {
            canv.worldCamera = controller.myCam;
        }
    }
    public virtual void CreatePlayer2(PlayerController controller)
    {

    }
}
