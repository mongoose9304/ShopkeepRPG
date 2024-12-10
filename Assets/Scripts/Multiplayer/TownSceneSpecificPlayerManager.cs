using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSceneSpecificPlayerManager : SceneSpecificPlayerManager
{
    public TownPlayer player1;
    public TownPlayer player2;
    public override void CreatePlayer1(PlayerController controller)
    {
        foreach (GameObject obj in objectsToDisableWhenPlayer1Joins)
        {
            obj.SetActive(false);
        }
        foreach (Canvas canv in player1Canvases)
        {
            canv.worldCamera = controller.myCam;
        }
        player1.SetUpControls(controller.input);
        player1.gameObject.SetActive(true);
    }
    public override void CreatePlayer2(PlayerController controller)
    {
        foreach (GameObject obj in objectsToEnableWhenPlayer2Joins)
        {
            obj.SetActive(true);
        }
        foreach (Canvas canv in player2Canvases)
        {
            canv.worldCamera = controller.myCam;
        }
        player2.SetUpControls(controller.input);
        player2.transform.position = player1.transform.position;
        player2.gameObject.SetActive(true);
        ShopManager.instance.twoPlayerMode = true;
    }
}
